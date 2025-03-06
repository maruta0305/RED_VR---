using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleCanvas : MonoBehaviour
{
    // 対象のCanvasをアタッチする
    public GameObject targetCanvas;

    // InputActionReferenceを設定
    public InputActionReference rightHandGripAction;

    void OnEnable()
    {
        // アクションが有効になったときにイベントを登録
        if (rightHandGripAction != null)
        {
            rightHandGripAction.action.performed += OnGripActionPerformed;
        }
    }

    void OnDisable()
    {
        // アクションが無効になったときにイベントを解除
        if (rightHandGripAction != null)
        {
            rightHandGripAction.action.performed -= OnGripActionPerformed;
        }
    }

    private void OnGripActionPerformed(InputAction.CallbackContext context)
    {
        // Canvasの有効/無効を切り替える
        if (targetCanvas != null)
        {
            bool isActive = targetCanvas.activeSelf;
            targetCanvas.SetActive(!isActive);
        }
    }
}
