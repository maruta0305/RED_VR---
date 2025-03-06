using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MoveObject : MonoBehaviour
{
    public GameObject[] targetObjects; // 移動先のゲームオブジェクト
    public float interval = 5f;         // 移動の間隔（秒）

    private int currentTargetIndex = 0; // 現在の移動先のインデックス
    public InputActionReference rightHandGripAction; // InputActionReferenceをインスペクターで設定

    private void Update()
    {
        if (rightHandGripAction.action.WasPressedThisFrame())
        {
            StartCoroutine(MoveToTargets());
        }
        // 繰り返し実行するためのコルーチンを開始
        //StartCoroutine(MoveToTargets());
    }

    private System.Collections.IEnumerator MoveToTargets()
    {
        while (true)
        {
            // 現在のターゲットの位置を取得
            if (currentTargetIndex < targetObjects.Length)
            {
                Vector3 targetPosition = targetObjects[currentTargetIndex].transform.position;
                transform.position = targetPosition; // 移動
                Debug.Log("Moved to: " + targetPosition);

                // 次のターゲットのインデックスを更新
                currentTargetIndex++;
            }
            else
            {
                // 全てのターゲットに移動したらリセット
                currentTargetIndex = 0;
            }

            // 指定された時間だけ待機
            yield return new WaitForSeconds(interval);
        }
    }
}


