using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SaveTest : MonoBehaviour
{
    public InputActionReference rightHandGripAction; // InputActionReferenceをインスペクターで設定
    public Text displayText; // UIのTextコンポーネントをインスペクターで設定

    private string csvFilePath;

    void Start()
    {
        // CSVファイルのパスを設定
        csvFilePath = Path.Combine(Application.persistentDataPath, "data.csv");

        // ファイルが存在しない場合は、ヘッダーを書き込む
        if (!File.Exists(csvFilePath))
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(csvFilePath))
                {
                    sw.WriteLine("Timestamp,DisplayedText");
                }
            }
            catch (IOException e)
            {
                Debug.LogError("Error writing to CSV: " + e.Message);
            }
        }
    }

    void Update()
    {
        // グリップボタンが押されたかをチェック
        if (rightHandGripAction.action.WasPressedThisFrame())
        {
            Debug.Log("Grip button pressed!"); // デバッグログ
            SaveData();
        }
    }

    void SaveData()
    {
        // 現在のタイムスタンプを取得
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Textコンポーネントの内容を取得
        string displayedText = displayText.text;

        // デバッグログ
        Debug.Log("Displayed Text: " + displayedText);

        // CSVに書き込むデータを構築
        string data = $"{timestamp},\"{displayedText}\""; // カンマを含む場合のためにクォートで囲む

        // CSVファイルにデータを追加
        try
        {
            using (StreamWriter sw = new StreamWriter(csvFilePath, true))
            {
                sw.WriteLine(data);
            }
            Debug.Log("Data saved: " + data);
        }
        catch (IOException e)
        {
            Debug.LogError("Error writing to CSV: " + e.Message);
        }
    }
}
