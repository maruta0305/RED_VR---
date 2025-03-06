using UnityEngine;
using UnityEngine.InputSystem;

public class UIChange : MonoBehaviour
{
    public GameObject UI; // 表示するUIオブジェクト
    public InputActionReference rightHandGripAction; // InputActionReferenceをインスペクターで設定

    void Start()
    {
        UI.SetActive(false); // 初期状態でUIを非表示にする
    }

    void Update()
    {
        // グリップボタンが押されたかをチェック
        if (rightHandGripAction.action.WasPressedThisFrame())
        {
            ChangeUI();
        }
    }

    public void ChangeUI()
    {
        // UIの状態をトグル
        UI.SetActive(!UI.activeSelf);
    }
}
