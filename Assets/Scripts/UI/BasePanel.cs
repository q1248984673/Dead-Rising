using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    // パネルの透明度を制御するコンポーネント
    private CanvasGroup canvasGroup;
    // フェードイン・フェードアウトの速度
    private float alphaSpeed = 10;

    // 現在表示中かどうか
    public bool isShow = false;

    // 非表示が完了した後に実行したい処理
    private UnityAction hideCallBack = null;

    protected virtual void Awake()
    {
        // 起動時にパネルにアタッチされているコンポーネントを取得
        canvasGroup = this.GetComponent<CanvasGroup>();
        // もし付け忘れていた場合は自動的に追加
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// コントロールのイベント登録用メソッド  
    /// すべての子パネルでボタンなどのイベント登録を行う必要があるため、抽象メソッドとして定義
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// パネルを表示するときの処理
    /// </summary>
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }

    /// <summary>
    /// パネルを非表示にするときの処理
    /// </summary>
    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;

        hideCallBack = callBack;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // 表示状態のとき、透明度が1未満なら徐々に上げていく（フェードイン）
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        // 非表示状態のとき、透明度が0より大きければ徐々に下げていく（フェードアウト）
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                // パネルが完全に透明になった後にコールバックを実行
                hideCallBack?.Invoke();
            }
        }
    }
}
