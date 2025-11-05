using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;

    //アニメーション再生が完了した後に実行したい処理を記録する関数
    private UnityAction overAction;
    // Startは最初のフレームの前に呼び出される
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    //左回転
    public void TurnLeft(UnityAction action)
    {
        animator.SetTrigger("Left");
        overAction = action;
    }

    //右回転
    public void TurnRgiht(UnityAction action)
    {
        animator.SetTrigger("Right");
        overAction = action;
    }

    //アニメーション再生が完了したときに呼び出されるメソッド
    public void PlayerOver()
    {
        overAction?.Invoke();
        overAction = null;
    }

}
