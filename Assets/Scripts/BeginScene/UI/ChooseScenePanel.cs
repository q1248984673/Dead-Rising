using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    // 4つのボタン
    public Button btnLeft;
    public Button btnRight;
    public Button btnStart;
    public Button btnBack;

    // テキストと画像の更新に使用
    public Text txtInfo;
    public Image imgScene;

    // 現在のデータインデックスを記録
    private int nowIndex;
    // 現在選択中のデータを記録
    private SceneInfo nowSceneInfo;

    public override void Init()
    {
        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if (nowIndex < 0)
                nowIndex = GameDataMgr.Instance.sceneInfoList.Count - 1;
            ChangeScene();
        });
        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if (nowIndex >= GameDataMgr.Instance.sceneInfoList.Count)
                nowIndex = 0;
            ChangeScene();
        });

        btnStart.onClick.AddListener(() =>
        {
            // 現在のパネルを非表示にする
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            // シーンを切り替える
            AsyncOperation ao = SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);
            // ステージの初期化を行う
            ao.completed += (obj) =>
            {
                GameLevelMgr.Instance.InitInfo(nowSceneInfo);
            };
        });

        btnBack.onClick.AddListener(() =>
        {
            // 自身を非表示にする
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            // 前のパネル（キャラクター選択パネル）を表示
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });

        // パネルを開いたときにデータを初期化・更新
        ChangeScene();
    }

    /// <summary>
    /// 画面に表示するシーン情報を切り替える
    /// </summary>
    public void ChangeScene()
    {
        nowSceneInfo = GameDataMgr.Instance.sceneInfoList[nowIndex];
        // 画像と表示テキストを更新
        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);
        txtInfo.text = "Name:\n" + nowSceneInfo.name + "\n" + "Description:\n" + nowSceneInfo.tips;
    }
}
