using UnityEngine;

public class AddColliderToChildren : MonoBehaviour
{
    void Start()
    {
        foreach (Transform child in transform)
        {
            // 子オブジェクトにBox Colliderを追加
            if (child.GetComponent<Collider>() == null) // 既にコライダーがない場合
            {
                child.gameObject.AddComponent<BoxCollider>();
            }
        }
    }
}
