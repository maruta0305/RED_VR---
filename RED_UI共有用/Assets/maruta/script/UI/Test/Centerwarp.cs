using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Centerwarp : MonoBehaviour
{
    public List<GameObject> WarpPointObjects;  // ワープ先のオブジェクト群
    public Camera playerCamera;  // VRカメラ（通常はXRカメラまたはMain Camera）
    public Transform xrOriginTransform;  // XROriginのTransform

    // UIボタンを参照するリスト（ボタン5つ分）
    public List<Button> warpButtons;  // ボタンリスト

    private void OnEnable()
    {
        // 各ボタンに対してイベントを登録
        for (int i = 0; i < warpButtons.Count; i++)
        {
            int index = i;  // ローカル変数としてインデックスを渡す
            warpButtons[i].onClick.AddListener(() => WarpToPoint(index));
        }
    }

    private void OnDisable()
    {
        // 各ボタンのイベントを解除
        for (int i = 0; i < warpButtons.Count; i++)
        {
            int index = i;  // ローカル変数としてインデックスを渡す
            warpButtons[i].onClick.RemoveListener(() => WarpToPoint(index));
        }
    }

    // ボタンが押された時にワープする
    void WarpToPoint(int index)
    {
        if (index < 0 || index >= WarpPointObjects.Count)
        {
            Debug.LogWarning("Invalid warp point index!");
            return;
        }

        // 選ばれたワープ地点を取得
        Vector3 warpPosition = WarpPointObjects[index].transform.position;

        // カメラをワープ先の位置に移動
        playerCamera.transform.position = warpPosition;

        // XROriginもワープ先の位置に移動
        xrOriginTransform.position = warpPosition;

        // 必要であれば、カメラの回転もターゲットオブジェクトに合わせて調整することができます
        // playerCamera.transform.rotation = WarpPointObjects[index].transform.rotation;
        // xrOriginTransform.rotation = WarpPointObjects[index].transform.rotation;
    }
}
