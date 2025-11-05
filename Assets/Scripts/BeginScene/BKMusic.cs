using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instacne => instance;

    private AudioSource bkSource;

    // Start は最初のフレームの前に呼び出される
    void Awake()
    {
        instance = this;

        bkSource = this.GetComponent<AudioSource>();

        // データに基づいて音楽の音量とオン/オフ状態を設定する
        MusicData data = GameDataMgr.Instance.musicData;
        SetIsOpen(data.musicOpen);
        ChangeValue(data.musicValue);
    }

    // バックグラウンドミュージックのオン/オフを切り替えるメソッド
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;
    }

    // バックグラウンドミュージックの音量を調整するメソッド
    public void ChangeValue(float v)
    {
        bkSource.volume = v;
    }
}
