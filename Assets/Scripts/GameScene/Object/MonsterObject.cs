using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour
{
    // アニメーション関連
    private Animator animator;
    // 移動関連（ナビメッシュエージェント）
    private NavMeshAgent agent;
    // 変更されない基本データ
    private MonsterInfo monsterInfo;

    // 現在のHP
    private int hp;
    // モンスターが死亡しているかどうか
    public bool isDead = false;

    // 前回の攻撃時間
    private float frontTime;

    // Start は最初のフレームの前に呼び出される
    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
    }

    // 初期化
    public void InitInfo(MonsterInfo info)
    {
        monsterInfo = info;
        // ステートマシン（アニメーション）を読み込む
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        // 現在HPを設定
        hp = info.hp;
        // 速度と加速度を同じ値に設定（加速せず一定速度で移動させるため）
        agent.speed = agent.acceleration = info.moveSpeed;
        // 回転速度
        agent.angularSpeed = info.roundSpeed;
    }

    // ダメージを受ける処理
    public void Wound(int dmg)
    {
        if (isDead)
            return;

        // HPを減少
        hp -= dmg;
        // 被弾アニメーションを再生
        animator.SetTrigger("Wound");

        if (hp <= 0)
        {
            // 死亡
            Dead();
        }
        else
        {
            // 効果音を再生
            GameDataMgr.Instance.PlaySound("Music/Wound");
        }
    }

    // 死亡処理
    public void Dead()
    {
        isDead = true;
        // 移動を停止
        //agent.isStopped = true;
        agent.enabled = false;
        // 死亡アニメーションを再生
        animator.SetBool("Dead", true);

        // 効果音を再生
        GameDataMgr.Instance.PlaySound("Music/dead");
        // お金を加算 — 後にレベル管理クラスを通してプレイヤーに加算される
        GameLevelMgr.Instance.player.AddMoney(10);
    }

    // 死亡アニメーション終了後に呼び出されるイベントメソッド
    public void DeadEvent()
    {
        // 死亡アニメーション再生後にオブジェクトを削除
        // 後にレベルマネージャで処理する予定
        //GameLevelMgr.Instance.ChangeMonsterNum(-1);

        // モンスターをリストから削除
        GameLevelMgr.Instance.RemoveMonster(this);

        // シーンから死亡したオブジェクトを削除
        Destroy(this.gameObject);

        // モンスター死亡時に勝利条件をチェック
        if (GameLevelMgr.Instance.CheckOver())
        {
            // ゲーム終了画面を表示
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(GameLevelMgr.Instance.player.money, true);
        }
    }

    // 出現後に移動を開始
    // 移動（ナビメッシュエージェント）
    public void BornOver()
    {
        // 出現完了後、目的地へ移動させる
        agent.SetDestination(MainTowerObject.Instance.transform.position);
        // 移動アニメーションを再生
        animator.SetBool("Run", true);
    }

    // 攻撃処理
    void Update()
    {
        // 攻撃タイミングを監視
        if (isDead)
            return;
        // 速度に応じてアニメーションを変更
        animator.SetBool("Run", agent.velocity != Vector3.zero);
        // 目標点に近づいたら攻撃を実行
        if (Vector3.Distance(this.transform.position, MainTowerObject.Instance.transform.position) < 5 &&
            Time.time - frontTime >= monsterInfo.atkOffset)
        {
            // 攻撃時刻を記録
            frontTime = Time.time;
            animator.SetTrigger("Atk");
        }
    }

    // 攻撃判定
    public void AtkEvent()
    {
        // 範囲判定を行い、ダメージ処理を実施
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1, 1 << LayerMask.NameToLayer("MainTower"));

        // 効果音を再生
        GameDataMgr.Instance.PlaySound("Music/Eat");

        for (int i = 0; i < colliders.Length; i++)
        {
            if (MainTowerObject.Instance.gameObject == colliders[i].gameObject)
            {
                // 防衛エリアにダメージを与える
                MainTowerObject.Instance.Wound(monsterInfo.atk);
            }
        }
    }
}
