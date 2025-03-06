using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CSVWriterWithUIElements : MonoBehaviour
{
    public string fileName = "output.csv";

    // ボタン、メッセージ、InputField、Dropdownを一括管理するクラス
    [System.Serializable]
    public class UIElement
    {
        public Button button;           // ボタン
        public string message;          // メッセージ
        public InputField inputField;   // InputField
        public Dropdown dropdown;       // Dropdown
    }

    // UI要素を管理する配列
    public UIElement[] uiElements;

    void Start()
    {
        // 各ボタンにリスナーを追加
        foreach (UIElement element in uiElements)
        {
            element.button.onClick.AddListener(() => WriteToCSV(element));
        }
    }

    void WriteToCSV(UIElement element)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            // CSVに記録するメッセージを構築
            string csvCell = $"\"{element.message}"; // ボタンのメッセージ

            // InputFieldの値を追加（入力があれば）
            if (element.inputField != null && !string.IsNullOrEmpty(element.inputField.text))
            {
                csvCell += $" {element.inputField.text}";
            }

            // Dropdownの選択結果を追加（選択肢があれば）
            if (element.dropdown != null && element.dropdown.options.Count > 0)
            {
                csvCell += $" {element.dropdown.options[element.dropdown.value].text}";
            }

            csvCell += "\""; // ダブルクォートで閉じる

            // CSVファイルに追記
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(csvCell);
            }

            Debug.Log($"CSVに書き込み完了: {csvCell}");
        }
        catch (IOException ex)
        {
            Debug.LogError($"CSVファイルの書き込み中にエラーが発生しました: {ex.Message}");
        }
    }
}
