using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Text txtWin;
    public Text txtInfo;
    public Text txtMoney;

    public Button btnSure;

    public override void Init()
    {
        btnSure.onClick.AddListener(() => {
            // パネルを非表示にする
            UIManager.Instance.HidePanel<GameOverPanel>();
            UIManager.Instance.HidePanel<GamePanel>();
            // 現在のステージデータをクリア
            GameLevelMgr.Instance.ClearInfo();
            // シーンを切り替える
            SceneManager.LoadScene("BeginScene");
        });
    }

    public void InitInfo(int money, bool isWin)
    {
        txtWin.text = isWin ? "Victory" : "Defeat";
        txtInfo.text = isWin ? "Receive a victory reward" : "Receive a defeat reward";
        txtMoney.text = "￥" + money;

        // 報酬に応じてプレイヤーデータを更新
        GameDataMgr.Instance.playerData.haveMoney += money;
        GameDataMgr.Instance.SavePlayerData();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;
    }
}
