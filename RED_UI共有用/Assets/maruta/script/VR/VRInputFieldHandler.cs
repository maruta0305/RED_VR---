using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class VRInputFieldHandler : MonoBehaviour
{
    public List<InputField> inputFields; // 管理するInputFieldのリスト
    public Transform rightController; // 右コントローラーのTransform
    public float positionThreshold = 0.05f; // 一致とみなす距離のしきい値

    void Update()
    {
        // 右コントローラーの位置を取得
        Vector3 controllerPosition = rightController.position;

        foreach (var inputField in inputFields)
        {
            // InputFieldの位置を取得
            Vector3 inputFieldPosition = inputField.transform.position;

            // コントローラーの位置がInputFieldに近い場合
            if (Vector3.Distance(inputFieldPosition, controllerPosition) < positionThreshold)
            {
                // InputFieldを選択状態にする
                EventSystem.current.SetSelectedGameObject(inputField.gameObject);

                // 入力フィールドにフォーカスを当てる
                if (!inputField.isFocused)
                {
                    inputField.ActivateInputField();
                }

                // Enterキーで入力を確定（例）
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    inputField.DeactivateInputField(); // 入力フィールドを非アクティブ化
                }
            }
            else
            {
                // コントローラーが離れた場合、InputFieldの選択を解除
                if (EventSystem.current.currentSelectedGameObject == inputField.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }
    }
}
