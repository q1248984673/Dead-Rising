using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーデータ
/// </summary>
public class PlayerData
{
    // 現在所持しているゲーム内通貨の数
    public int haveMoney = 300;
    // 現在アンロック済みのキャラクター一覧
    public List<int> buyHero = new List<int>();
}