using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音楽データ構造クラス
/// </summary>
public class MusicData
{
    // バックグラウンドミュージックと効果音のオン/オフ設定
    public bool musicOpen = true;
    public bool soundOpen = true;
    // バックグラウンドミュージックと効果音の音量
    public float musicValue = 0.2f;
    public float soundValue = 0.2f;
}
