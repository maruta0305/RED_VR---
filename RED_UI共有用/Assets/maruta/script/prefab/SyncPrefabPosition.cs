using UnityEngine;

public class SyncPrefabPosition : MonoBehaviour
{
    public GameObject[] prefabs;  // プレハブの配列
    public GameObject[] prefabInstances;  // シーンに配置されているインスタンスの配列
    private Vector3[] previousPositions;  // 各インスタンスの前回の位置

    void Start()
    {
        // プレハブとインスタンスの配列が同じ数であることを確認
        if (prefabs.Length != prefabInstances.Length)
        {
            Debug.LogError("プレハブとインスタンスの数が一致しません！");
            return;
        }

        previousPositions = new Vector3[prefabs.Length];

        // インスタンスの前回位置を初期化
        for (int i = 0; i < prefabInstances.Length; i++)
        {
            previousPositions[i] = prefabInstances[i].transform.position;
        }
    }

    void Update()
    {
        // 各インスタンスの位置が変わった場合、対応するプレハブの位置を更新
        for (int i = 0; i < prefabInstances.Length; i++)
        {
            // インスタンスの位置が変更された場合
            if (prefabInstances[i].transform.position != previousPositions[i])
            {
                // プレハブの位置をインスタンスの位置に合わせる
                prefabs[i].transform.position = prefabInstances[i].transform.position;
                previousPositions[i] = prefabInstances[i].transform.position; // 前回位置を更新
            }
        }
    }
}
