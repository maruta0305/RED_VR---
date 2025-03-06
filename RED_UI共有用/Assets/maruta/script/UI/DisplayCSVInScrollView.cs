using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class DisplayCSVInScrollView : MonoBehaviour
{
    public ScrollRect scrollView;  // スクロールビュー
    public GameObject contentPanel;  // スクロールビュー内のコンテンツパネル
    public GameObject textPrefab;  // CSVの各行を表示するためのTextプレハブ

    private string fileName = "output.csv";  // CSVファイル名
    private float updateInterval = 5f;  // 更新間隔
    private float nextUpdateTime = 0f;  // 次の更新時間

    void Start()
    {
        DisplayCSVContent();
        //StartCoroutine(UpdateCSVContent());
    }

    void Update()
    {
        // 現在の時間が次の更新時間を超えた場合、CSVを更新
        if (Time.time >= nextUpdateTime)
        {
            DisplayCSVContent();
            nextUpdateTime = Time.time + updateInterval;  // 次回の更新時間を設定
        }
    }
    //IEnumerator UpdateCSVContent()
    //{
    //    while (true)
    //    {
    //        DisplayCSVContent();
    //        yield return new WaitForSeconds(5f);  // 5秒ごとに更新
    //    }
    //}

    void DisplayCSVContent()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);  // ファイルパス

        if (File.Exists(filePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // Contentパネル内の既存UI要素を削除
                foreach (Transform child in contentPanel.transform)
                {
                    Destroy(child.gameObject);
                }

                // 各行をそのままTextに反映
                foreach (string line in lines)
                {
                    // CSVの行がカンマ区切りの場合、必要に応じて分割
                    string[] columns = line.Split(',');

                    // 各カラム（セル）の内容をTextとして表示
                    string fullText = string.Join("  |  ", columns);  // 例: カンマを「 | 」に変換

                    // プレハブから新しいTextオブジェクトを生成
                    GameObject newText = Instantiate(textPrefab);
                    newText.transform.SetParent(contentPanel.transform, false);

                    // TextコンポーネントにCSVの内容をセット
                    Text textComponent = newText.GetComponent<Text>();
                    textComponent.text = fullText;

                    // Textのサイズ調整（必要に応じて）
                    textComponent.horizontalOverflow = HorizontalWrapMode.Wrap;
                    textComponent.verticalOverflow = VerticalWrapMode.Overflow;

                    // Textコンポーネントに合わせてコンテンツのサイズを調整
                    ContentSizeFitter contentFitter = newText.GetComponent<ContentSizeFitter>();
                    if (contentFitter == null)
                    {
                        contentFitter = newText.AddComponent<ContentSizeFitter>();
                    }
                    contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                    contentFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                }

                // Contentパネルのサイズを自動調整
                contentPanel.GetComponent<ContentSizeFitter>().SetLayoutVertical();

                // スクロールビューを一番上にスクロール
                Canvas.ForceUpdateCanvases();
                scrollView.verticalNormalizedPosition = 1f;
            }
            catch (IOException ex)
            {
                Debug.LogError($"CSVファイルの読み込み中にエラーが発生しました: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("CSVファイルが見つかりません: " + filePath);
        }
    }
}
