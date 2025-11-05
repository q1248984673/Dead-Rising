using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;

    public override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            //カメラの左回転アニメーションを再生してから、キャラクター選択パネルを表示する
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });

            //開始画面を非表示にする
            UIManager.Instance.HidePanel<BeginPanel>();
        });

        btnSetting.onClick.AddListener(() =>
        {
            //後でここに設定画面を表示する処理を追加する予定
            UIManager.Instance.ShowPanel<SettingPanel>();
        });

        btnAbout.onClick.AddListener(() =>
        {
            //自作の「About（情報）」パネルを作成して、ここで表示できるようにする
        });

        btnQuit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
