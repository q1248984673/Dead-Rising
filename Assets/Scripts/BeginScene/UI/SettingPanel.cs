using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;

    public override void Init()
    {
        // パネルの表示内容を初期化し、ローカルに保存された設定データに基づいて設定する
        MusicData data = GameDataMgr.Instance.musicData;
        // トグル（オン/オフスイッチ）の初期状態を設定
        togMusic.isOn = data.musicOpen;
        togSound.isOn = data.soundOpen;
        // スライダー（音量調整バー）の初期値を設定
        sliderMusic.value = data.musicValue;
        sliderSound.value = data.soundValue;

        btnClose.onClick.AddListener(() =>
        {
            // パフォーマンスを考慮し、設定完了後パネルを閉じるタイミングでのみデータを保存（実際にファイルへ書き込む）
            GameDataMgr.Instance.SaveMusicData();
            // 自分自身（設定パネル）を非表示にする
            UIManager.Instance.HidePanel<SettingPanel>();
        });

        togMusic.onValueChanged.AddListener((v) =>
        {
            // バックグラウンドミュージックのオン/オフを切り替える
            BKMusic.Instacne.SetIsOpen(v);
            // ミュージックのスイッチ状態を記録
            GameDataMgr.Instance.musicData.musicOpen = v;
        });

        togSound.onValueChanged.AddListener((v) =>
        {
            // 効果音のオン/オフ状態を記録
            GameDataMgr.Instance.musicData.soundOpen = v;
        });

        sliderMusic.onValueChanged.AddListener((v) =>
        {
            // バックグラウンドミュージックの音量を変更
            BKMusic.Instacne.ChangeValue(v);
            // 音楽音量データを記録
            GameDataMgr.Instance.musicData.musicValue = v;
        });

        sliderSound.onValueChanged.AddListener((v) =>
        {
            // 効果音音量データを記録
            GameDataMgr.Instance.musicData.soundValue = v;
        });
    }
}
