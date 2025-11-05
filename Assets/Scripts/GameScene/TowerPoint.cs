using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    // タワー建設ポイントに紐づくタワーオブジェクト
    private GameObject towerObj = null;
    // タワー建設ポイントに紐づくタワーデータ
    public TowerInfo nowTowerInfo = null;
    // 建設可能な3種類のタワーID
    public List<int> chooseIDs;

    /// <summary>
    /// タワーを建設
    /// </summary>
    /// <param name="id"></param>
    public void CreateTower(int id)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        // 所持金が足りない場合は建設しない
        if (info.money > GameLevelMgr.Instance.player.money)
            return;

        // 代金を差し引く
        GameLevelMgr.Instance.player.AddMoney(-info.money);
        // タワーの生成
        // 既にタワーがある場合はいったん削除
        if (towerObj != null)
        {
            Destroy(towerObj);
            towerObj = null;
        }
        // タワーオブジェクトを生成
        towerObj = Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);
        // タワーを初期化
        towerObj.GetComponent<TowerObject>().InitInfo(info);

        // 現在のタワーデータを記録
        nowTowerInfo = info;

        // タワー建設完了後、ゲームUIを更新
        if (nowTowerInfo.nextLev != 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 既にタワーがあり、かつ最大レベルでアップグレード不可ならUIを表示しない
        if (nowTowerInfo != null && nowTowerInfo.nextLev == 0)
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }

    private void OnTriggerExit(Collider other)
    {
        // 画面下部の建設UIを非表示にしたい場合はnullを渡す
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
}
