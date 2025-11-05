using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    // 砲台のヘッド（回転してターゲットの方向を向く）
    public Transform head;
    // 発射点（攻撃エフェクトを出す位置）
    public Transform gunPoint;
    // 砲台ヘッドの回転速度（固定でもデータ表からでもよい）
    private float roundSpeed = 20;

    // 砲台に紐づくデータ
    private TowerInfo info;

    // 現在攻撃対象のターゲット
    private MonsterObject targetObj;
    // 現在攻撃対象のターゲット群
    private List<MonsterObject> targetObjs;

    // 攻撃間隔を判定するためのタイマー
    private float nowTime;

    // モンスター位置の記録用
    private Vector3 monsterPos;

    /// <summary>
    /// 砲台関連データの初期化
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(TowerInfo info)
    {
        this.info = info;
    }

    // Update is called once per frame
    void Update()
    {
        // 単体攻撃ロジック
        if (info.atkType == 1)
        {
            // ターゲットがいない／死亡している／攻撃範囲外に出た
            if (targetObj == null ||
                targetObj.isDead ||
                Vector3.Distance(this.transform.position, targetObj.transform.position) > info.atkRange)
            {
                targetObj = GameLevelMgr.Instance.FindMonster(this.transform.position, info.atkRange);
            }

            // 攻撃可能な対象が見つからない場合、砲台は回転しない
            if (targetObj == null)
                return;

            // モンスターの位置を取得。ヘッドの上下チルトを避けるためYを合わせる
            monsterPos = targetObj.transform.position;
            monsterPos.y = head.position.y;
            // 砲台ヘッドを回転させる
            head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(monsterPos - head.position), roundSpeed * Time.deltaTime);

            // ヘッドの向きとターゲット方向の角度が一定以下、かつ攻撃間隔を満たすときに攻撃
            if (Vector3.Angle(head.forward, monsterPos - head.position) < 5 &&
                Time.time - nowTime >= info.offsetTime)
            {
                // ターゲットにダメージ
                targetObj.Wound(info.atk);
                // 効果音を再生
                GameDataMgr.Instance.PlaySound("Music/Tower");
                // 発射エフェクトを生成
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);
                // エフェクトを遅延削除
                Destroy(effObj, 0.2f);

                // 発射時刻を記録
                nowTime = Time.time;
            }
        }
        // 範囲（群体）攻撃ロジック
        else
        {
            targetObjs = GameLevelMgr.Instance.FindMonsters(this.transform.position, info.atkRange);

            if (targetObjs.Count > 0 &&
                Time.time - nowTime >= info.offsetTime)
            {
                // 発射エフェクトを生成
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);
                // エフェクトを遅延削除
                Destroy(effObj, 0.2f);

                // 対象群にダメージ
                for (int i = 0; i < targetObjs.Count; i++)
                {
                    targetObjs[i].Wound(info.atk);
                }

                // 発射時刻を記録
                nowTime = Time.time;
            }
        }
    }
}
