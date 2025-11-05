using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    // モンスターの総ウェーブ数
    public int maxWave;
    // 各ウェーブのモンスター数
    public int monsterNumOneWave;
    // 現在のウェーブで未生成の残り数を記録
    private int nowNum;

    // モンスターID（複数可）。ランダム生成で多様性を持たせる
    public List<int> monsterIDs;
    // 現在のウェーブで生成するモンスターIDを記録
    private int nowID;

    // 個体生成の間隔時間
    public float createOffsetTime;

    // ウェーブ間のインターバル
    public float delayTime;

    // 第1ウェーブの生成までの遅延
    public float firstDelayTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateWave", firstDelayTime);

        // 出現ポイントを登録
        GameLevelMgr.Instance.AddMonsterPoint(this);
        // 総ウェーブ数を更新
        GameLevelMgr.Instance.UpdatgeMaxNum(maxWave);
    }

    /// <summary>
    /// 1ウェーブの生成を開始
    /// </summary>
    private void CreateWave()
    {
        // このウェーブでのモンスターIDを決定
        nowID = monsterIDs[Random.Range(0, monsterIDs.Count)];
        // このウェーブの体数
        nowNum = monsterNumOneWave;
        // モンスターを生成
        CreateMonster();
        // 残りウェーブ数を減らす
        --maxWave;
        // レベル管理に1ウェーブ出現を通知
        GameLevelMgr.Instance.ChangeNowWaveNum(1);
    }

    /// <summary>
    /// モンスターを生成
    /// </summary>
    private void CreateMonster()
    {
        // モンスターを直接生成
        // モンスターデータを取得
        MonsterInfo info = GameDataMgr.Instance.monsterInfoList[nowID - 1];

        // モンスタープレハブを生成
        GameObject obj = Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);
        // 生成したプレハブにMonsterObjectを付与し初期化
        MonsterObject monsterObj = obj.AddComponent<MonsterObject>();
        monsterObj.InitInfo(info);

        // マネージャにモンスター数+1を通知
        //GameLevelMgr.Instance.ChangeMonsterNum(1);
        // モンスターをリストに登録
        GameLevelMgr.Instance.AddMonster(monsterObj);

        // 1体生成したら残り生成数を1減らす
        --nowNum;
        if (nowNum == 0)
        {
            if (maxWave > 0)
                Invoke("CreateWave", delayTime);
        }
        else
        {
            Invoke("CreateMonster", createOffsetTime);
        }
    }

    /// <summary>
    /// この出現ポイントでの出現が完了したか
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}
