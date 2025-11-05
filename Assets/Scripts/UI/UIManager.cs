using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    // 表示中のパネルを保存する辞書。パネルを表示するたびにここへ格納
    // パネルを隠す際は辞書から該当パネルを取り出して非表示にする
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    // シーン内のCanvasオブジェクト（パネルの親に設定する）
    private Transform canvasTrans;

    private UIManager()
    {
        // シーン内のCanvasオブジェクトを取得
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        // シーン遷移でも破棄されないようにして、ゲーム中はCanvasを1つだけに保つ
        GameObject.DontDestroyOnLoad(canvas);
    }

    // パネルを表示
    public T ShowPanel<T>() where T : BasePanel
    {
        // ジェネリックTの型名とプレハブ名を一致させるというルールで、汎用的に扱える
        string panelName = typeof(T).Name;

        // 辞書にすでに表示中の同パネルがあるか確認
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        // パネル名に基づいてプレハブを生成し、親を設定
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        // 生成したオブジェクトをCanvasの子にする
        panelObj.transform.SetParent(canvasTrans, false);

        // パネルの表示ロジックを参照し、保持しておく
        T panel = panelObj.GetComponent<T>();
        // 以後の取得/非表示のためにパネルスクリプトを辞書に保存
        panelDic.Add(panelName, panel);
        // 自身の表示処理を呼ぶ
        panel.ShowMe();

        return panel;
    }

    /// <summary>
    /// パネルを非表示
    /// </summary>
    /// <typeparam name="T">パネルクラス名</typeparam>
    /// <param name="isFade">フェードアウト完了後に削除するか（デフォルトtrue）</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        // ジェネリック型名から名前を取得
        string panelName = typeof(T).Name;
        // 現在表示中の中に対象パネルがあるか確認
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                // フェードアウト完了後に削除する
                panelDic[panelName].HideMe(() =>
                {
                    // オブジェクトを削除
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    // 辞書に保持しているパネルスクリプトを削除
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                // オブジェクトを削除
                GameObject.Destroy(panelDic[panelName].gameObject);
                // 辞書に保持しているパネルスクリプトを削除
                panelDic.Remove(panelName);
            }
        }
    }

    // パネルを取得
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        // 対応するパネルが表示されていなければnullを返す
        return null;
    }

}
