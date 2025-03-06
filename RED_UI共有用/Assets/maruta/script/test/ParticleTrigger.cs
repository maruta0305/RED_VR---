using UnityEngine;
using UnityEngine.UI; // UIコンポーネントを使用するための名前空間

public class ParticleTrigger : MonoBehaviour
{
    public GameObject particlePrefab; // パーティクルのPrefab
    public Text resultText; // UIのTextコンポーネントを指定する
    private bool hasTriggered = false; // このオブジェクトで一度だけ処理するためのフラグ
    private static int result = 0; // 全オブジェクト間で共有される静的変数

    void Start()
    {
        // 初期状態でTextに0を表示
        UpdateUIText();
    }

    void Update()
    {
        // メインカメラの位置を取得
        Transform mainCameraTransform = Camera.main.transform;

        // このオブジェクトとカメラの距離を計算
        float distance = Vector3.Distance(transform.position, mainCameraTransform.position);

        // 距離が5f以内で、まだ処理が実行されていない場合
        if (distance <= 7f && !hasTriggered)
        {
            // パーティクルを生成
            Instantiate(particlePrefab, transform.position, Quaternion.identity);

            // resultを増加
            result++;

            // 処理済みフラグを設定
            hasTriggered = true;

            // UIのTextを更新
            UpdateUIText();

            // resultの値をデバッグログに表示（必要に応じて削除）
            Debug.Log($"Result: {result}");
        }
    }

    private void UpdateUIText()
    {
        if (resultText != null)
        {
            resultText.text = $"Result: {result}";
        }
        else
        {
            Debug.LogWarning("ResultTextが設定されていません。");
        }
    }
}
