using UnityEngine;
using UnityEngine.UI;

public class Savepoint : MonoBehaviour
{
    public Text resultText;
    public GameObject particlePrefab;
    public Transform[] targetObjects; // 距離を測定する対象のオブジェクト（5つ）

    private bool hasTriggered = false;
    private static int save = 0;

    void Start()
    {
        UpdateUIText();
    }

    void Update()
    {
        if (hasTriggered || save >= 10) return; // すでに処理済み、または上限なら何もしない

        foreach (Transform target in targetObjects)
        {
            if (target == null) continue; // 念のためnullチェック

            float distanceSquared = (transform.position - target.position).sqrMagnitude;

            if (distanceSquared <= 16f) // 5f * 5f の距離内
            {
                save++;
                hasTriggered = true;
                UpdateUIText();

                if (particlePrefab != null)
                {
                    Instantiate(particlePrefab, transform.position, Quaternion.identity);
                }

                Debug.Log($"SavePoint triggered by {target.name}! Current save count: {save}");
                break; // 一度加算されたらループ終了
            }
        }
    }

    private void UpdateUIText()
    {
        if (resultText != null)
        {
            resultText.text = $"Save: {save}/10";
        }
        else
        {
            Debug.LogWarning("ResultTextが設定されていません。");
        }
    }
}
