using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // カメラが注視するターゲットオブジェクト
    public Transform target;
    // カメラがターゲットに対して持つXYZ方向のオフセット位置
    public Vector3 offsetPos;
    // 注視位置のY方向オフセット
    public float bodyHeight;

    // 移動および回転速度
    public float moveSpeed;
    public float rotationSpeed;

    private Vector3 targetPos;
    private Quaternion targetRotation;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;
        // ターゲットオブジェクトに基づいてカメラの位置と角度を計算
        // 位置の計算
        // Z軸方向（後方）オフセット
        targetPos = target.position + target.forward * offsetPos.z;
        // Y軸方向（上方）オフセット
        targetPos += Vector3.up * offsetPos.y;
        // X軸方向（左右）オフセット
        targetPos += target.right * offsetPos.x;
        // 線形補間でカメラをターゲット位置に近づける
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, moveSpeed * Time.deltaTime);

        // 回転の計算
        // 最終的に注視すべき方向を表すクォータニオンを取得
        targetRotation = Quaternion.LookRotation(target.position + Vector3.up * bodyHeight - this.transform.position);
        // カメラの回転を徐々にターゲット方向へ補間
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// カメラが注視するターゲットオブジェクトを設定
    /// </summary>
    /// <param name="player"></param>
    public void SetTarget(Transform player)
    {
        target = player;
    }
}
