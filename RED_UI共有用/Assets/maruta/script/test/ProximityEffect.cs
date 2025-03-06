using UnityEngine;

public class ProximityEffect : MonoBehaviour
{
    public GameObject targetPrefab; // 対象となるPrefab
    public GameObject effectPrefab; // 光るエフェクトのPrefab
    public float triggerDistance = 10f; // 発動する距離

    private GameObject currentEffect;

    void Update()
    {
        if (targetPrefab != null)
        {
            // 距離を計算
            float distance = Vector3.Distance(transform.position, targetPrefab.transform.position);

            // 距離がtriggerDistance以下の場合にエフェクトを表示
            if (distance <= triggerDistance)
            {
                if (currentEffect == null)
                {
                    // エフェクトを生成
                    currentEffect = Instantiate(effectPrefab, targetPrefab.transform.position, Quaternion.identity);
                }
                // エフェクトをターゲットの位置に追従させる
                currentEffect.transform.position = targetPrefab.transform.position;
            }
            else
            {
                // 距離が離れたらエフェクトを非表示
                if (currentEffect != null)
                {
                    Destroy(currentEffect);
                    currentEffect = null;
                }
            }
        }
    }
}
