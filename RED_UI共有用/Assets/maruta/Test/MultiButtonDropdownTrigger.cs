using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MultiButtonDropdownTrigger : MonoBehaviour
{
    public List<Button> buttons; // 管理するボタンのリスト
    public List<Dropdown> dropdowns; // 管理するプルダウンのリスト
    public Transform controller; // コントローラーのTransform
    public float positionThreshold = 0.1f; // 一致とみなす距離のしきい値
    public List<ScrollRect> scrollViews; // 複数のScrollView

    private HashSet<Button> clickedButtons = new HashSet<Button>(); // クリック済みのボタンを記録

    void Update()
    {
        // コントローラーの位置を取得
        Vector3 controllerPosition = controller.position;

        // ボタンの処理
        foreach (var button in buttons)
        {
            Vector3 buttonPosition = button.transform.position;

            // ボタンとコントローラーの位置が一致しているかをチェック
            if (Vector3.Distance(buttonPosition, controllerPosition) < positionThreshold)
            {
                // クリック済みのボタンでない場合
                if (!clickedButtons.Contains(button))
                {
                    button.onClick.Invoke();
                    clickedButtons.Add(button); // ボタンをクリック済みとして記録
                    Debug.Log($"{button.name} clicked programmatically!");
                }
            }
            else
            {
                // コントローラーが離れた場合はクリック状態をリセット
                clickedButtons.Remove(button);
            }
        }

        // プルダウンの処理
        foreach (var dropdown in dropdowns)
        {
            Vector3 dropdownPosition = dropdown.transform.position;

            if (Vector3.Distance(dropdownPosition, controllerPosition) < positionThreshold)
            {
                dropdown.Show();

                int optionsCount = dropdown.options.Count;

                // 選択肢の数をログに出力
                Debug.Log($"Dropdown '{dropdown.name}' has {optionsCount} options.");

                // ドロップダウンのテンプレートの位置を取得
                RectTransform templateRect = dropdown.GetComponent<Dropdown>().template.GetComponent<RectTransform>();

                for (int i = 0; i < optionsCount; i++)
                {
                    // 選択肢の位置を計算
                    RectTransform optionRect = templateRect.GetChild(0).GetChild(i).GetComponent<RectTransform>();
                    Vector3 optionPosition = optionRect.position;

                    Debug.Log($"Option {i}: {optionPosition}");

                    if (Vector3.Distance(optionPosition, controllerPosition) < positionThreshold)
                    {
                        dropdown.value = i;
                        Debug.Log($"Dropdown '{dropdown.name}' changed to {dropdown.options[i].text}");
                        Debug.Log($"Touched option: {dropdown.options[i].text}");
                    }
                }
            }
        }

        // 複数のScrollViewの処理
        foreach (var scrollView in scrollViews)
        {
            // スクロールビューの位置を取得
            Vector3 scrollViewPosition = scrollView.transform.position;

            // コントローラーとScrollViewの位置が一致するかをチェック
            if (Vector3.Distance(scrollViewPosition, controllerPosition) < positionThreshold)
            {
                // コントローラーの移動量に基づいてScrollViewをスクロールさせる
                float scrollAmount = controllerPosition.y - scrollViewPosition.y; // Y軸の差を使用
                scrollView.verticalNormalizedPosition -= scrollAmount * 0.5f; // スクロール量を調整
                Debug.Log($"ScrollView '{scrollView.name}' moved by {scrollAmount}");
            }
        }
    }
}
