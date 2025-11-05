using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;

    public PlayerObject player;

    // すべての出現ポイント
    private List<MonsterPoint> points = new List<MonsterPoint>();
    // 現在残っているウェーブ数を記録
    private int nowWaveNum = 0;
    // 総ウェーブ数を記録
    private int maxWaveNum = 0;

    //// 現在のシーン上のモンスター数を記録
    //private int nowMonsterNum = 0;

    // 現在のシーン上にいるモンスターの一覧を記録
    private List<MonsterObject> monsterList = new List<MonsterObject>();

    private GameLevelMgr()
    {

    }

    // 1. ゲームシーンへ切り替える際にプレイヤーを動的に生成
    public void InitInfo(SceneInfo info)
    {
        // ゲームUIを表示
        UIManager.Instance.ShowPanel<GamePanel>();

        // プレイヤーの生成
        // 事前に選択されたプレイヤーデータを取得
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;
        // シーン内のプレイヤー出現位置を取得
        Transform heroPos = GameObject.Find("HeroBornPos").transform;
        // プレイヤープレハブを生成し、出現地点の位置・回転に合わせる
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res), heroPos.position, heroPos.rotation);
        // プレイヤーオブジェクトを初期化
        player = heroObj.GetComponent<PlayerObject>();
        // プレイヤーの基礎属性を初期化
        player.InitPlayerInfo(roleInfo.atk, info.money);

        // カメラが生成したプレイヤーを注視/追従するよう設定
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);

        // 中央防衛エリアのHPを初期化
        MainTowerObject.Instance.UpdateHp(info.towerHp, info.towerHp);
    }

    // 2. ゲームマネージャで勝利判定を行う
    //    シーン内に未出現のモンスターが残っているか、未撃破のモンスターがいるかを確認

    // 出現ポイントを登録するメソッド
    public void AddMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
    }

    /// <summary>
    /// 総ウェーブ数を更新
    /// </summary>
    /// <param name="num"></param>
    public void UpdatgeMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        // UIを更新
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }

    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
        // UIを更新
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }

    /// <summary>
    /// 勝利かどうかを判定
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        for (int i = 0; i < points.Count; i++)
        {
            // いずれかの出現ポイントでまだ出現が残っている場合は未勝利
            if (!points[i].CheckOver())
                return false;
        }

        if (monsterList.Count > 0)
            return false;

        Debug.Log("Victory");
        return true;
    }

    ///// <summary>
    ///// シーン上のモンスター数を変更
    ///// </summary>
    ///// <param name="num"></param>
    //public void ChangeMonsterNum(int num)
    //{
    //    nowMonsterNum += num;
    //}

    /// <summary>
    /// モンスターをリストに登録
    /// </summary>
    /// <param name="obj"></param>
    public void AddMonster(MonsterObject obj)
    {
        monsterList.Add(obj);
    }

    /// <summary>
    /// モンスターをリストから削除（死亡時に使用）
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveMonster(MonsterObject obj)
    {
        monsterList.Remove(obj);
    }

    /// <summary>
    /// 距離条件を満たす単体モンスターを検索
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public MonsterObject FindMonster(Vector3 pos, int range)
    {
        // タワー攻撃用に、距離条件を満たすモンスターをモンスター一覧から探して返す
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                return monsterList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 条件を満たす全モンスターを検索
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public List<MonsterObject> FindMonsters(Vector3 pos, int range)
    {
        // 条件を満たす全モンスターを探し、リストにまとめて返す
        List<MonsterObject> list = new List<MonsterObject>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                list.Add(monsterList[i]);
            }
        }
        return list;
    }

    /// <summary>
    /// 現在のステージ記録をクリアし、次回のステージ切り替えに影響しないようにする
    /// </summary>
    public void ClearInfo()
    {
        points.Clear();
        monsterList.Clear();
        nowWaveNum = maxWaveNum = 0;
        player = null;
    }
}
