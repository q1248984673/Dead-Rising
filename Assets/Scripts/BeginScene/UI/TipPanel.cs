using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public Text txtInfo;

    public Button btnSure;

    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<TipPanel>();
        });
    }

    /// <summary>
    /// メッセージ内容を変更するメソッド
    /// </summary>
    /// <param name="str"></param>
    public void ChangeInfo(string str)
    {
        txtInfo.text = str;
    }
}
