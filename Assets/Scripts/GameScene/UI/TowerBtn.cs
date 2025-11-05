using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 複合UIコンポーネント。タワー建設関連UIの更新ロジックをまとめて制御するためのもの
/// </summary>
public class TowerBtn : MonoBehaviour
{
    public Image imgPic;

    public Text txtTip;

    public Text txtMoney;

    /// <summary>
    /// ボタン情報を初期化するメソッド
    /// </summary>
    /// <param name="id"></param>
    /// <param name="inputStr"></param>
    public void InitInfo(int id, string inputStr)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        imgPic.sprite = Resources.Load<Sprite>(info.imgRes);
        txtMoney.text = "￥" + info.money;
        txtTip.text = inputStr;
        // 所持金が足りるかを判定
        if (info.money > GameLevelMgr.Instance.player.money)
            txtMoney.text = "Not enough money";
    }
}
