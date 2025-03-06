using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VIVEButtonCounter : MonoBehaviour
{
    //時間計測とコントローラーの操作回数のカウント(テスト用)
    public ActionBasedController leftController;  // 左手コントローラー
    public ActionBasedController rightController; // 右手コントローラー

    private int buttonPressCount = 0; // 総ボタン押下回数
    private float startTime;

    void Start()
    {
        startTime = Time.time;
        if (leftController != null)
        {
            RegisterControllerInputs(leftController);
        }
        if (rightController != null)
        {
            RegisterControllerInputs(rightController);
        }
    }

    private void RegisterControllerInputs(ActionBasedController controller)
    {
        if (controller.selectAction.action != null)
        {
            controller.selectAction.action.performed += ctx => CountButtonPress();
        }
        if (controller.activateAction.action != null)
        {
            controller.activateAction.action.performed += ctx => CountButtonPress();
        }
        if (controller.uiPressAction.action != null)
        {
            controller.uiPressAction.action.performed += ctx => CountButtonPress();
        }
    }

    private void CountButtonPress()
    {
        buttonPressCount++;
    }

    private void OnApplicationQuit()
    {
        float elapsedTime = Time.time - startTime;
        Debug.Log($"🔴 ゲーム終了！総ボタン押下回数: {buttonPressCount}");
        Debug.Log($"ゲームプレイ時間: {elapsedTime:F2} 秒");
    }
}
