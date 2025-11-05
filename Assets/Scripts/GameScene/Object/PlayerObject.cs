using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private Animator animator;

    // 1. プレイヤー属性の初期化
    // プレイヤーの攻撃力
    private int atk;
    // 所持金
    public int money;
    // 回転速度
    private float roundSpeed = 50;

    // 銃を持つ場合のみ存在する発射点
    public Transform gunPoint;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    /// <summary>
    /// プレイヤーの基本属性を初期化
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="money"></param>
    public void InitPlayerInfo(int atk, int money)
    {
        this.atk = atk;
        this.money = money;
        // UI上の所持金表示を更新
        UpdateMoney();
    }

    // Update is called once per frame
    void Update()
    {
        // 2. 移動によるアニメーション変化
        // 移動アニメの切り替え。アニメにルートモーションがあるためそれを適用。
        // この2つの値を変えるだけでアニメと速度が変化する
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        // 回転
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * roundSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
            animator.SetTrigger("Roll");

        if (Input.GetMouseButtonDown(0))
            animator.SetTrigger("Fire");

    }


    // 3. 攻撃アクションごとの処理
    /// <summary>
    /// 近接（刀）攻撃のダメージ判定イベント用
    /// </summary>
    public void KnifeEvent()
    {
        // ダメージ判定を行う
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1, 1 << LayerMask.NameToLayer("Monster"));

        // 効果音を再生
        GameDataMgr.Instance.PlaySound("Music/Knife");

        // 一時的にこれ以上のロジックは書けない。モンスターのスクリプトがないため
        for (int i = 0; i < colliders.Length; i++)
        {
            // 衝突相手のMonsterObjectを取得してダメージを与える
            MonsterObject monster = colliders[i].gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                monster.Wound(this.atk);
                break;
            }
        }
    }

    public void ShootEvent()
    {
        // レイキャストで照準判定
        // 発射点が必要という前提
        RaycastHit[] hits = Physics.RaycastAll(new Ray(gunPoint.position, this.transform.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));

        // 発砲SEを再生
        GameDataMgr.Instance.PlaySound("Music/Gun");

        for (int i = 0; i < hits.Length; i++)
        {
            // 対象のMonsterObjectを取得してダメージを与える
            // 衝突相手のMonsterObjectを取得してダメージを与える
            MonsterObject monster = hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                // ヒットエフェクトを生成
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1);

                monster.Wound(this.atk);
                break;
            }
        }
    }


    // 4. 所持金の更新ロジック
    public void UpdateMoney()
    {
        // UI上の所持金表示を更新
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    /// <summary>
    /// 外部から所持金を加算するためのメソッド
    /// </summary>
    /// <param name="money"></param>
    public void AddMoney(int money)
    {
        // お金を加算
        this.money += money;
        UpdateMoney();
    }
}
