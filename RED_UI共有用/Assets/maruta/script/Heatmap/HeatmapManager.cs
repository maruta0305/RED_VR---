using System.Collections.Generic;
using UnityEngine;

public class HeatmapManager : MonoBehaviour
{
    public int gridSize = 10;          // グリッドのセル数（10×10）
    public float cellSize = 1.0f;     // 各セルのサイズ
    public GameObject cellPrefab;     // グリッドセルのプレハブ
    public List<GameObject> prefabsToMonitor; // 追跡する複数のプレハブを管理

    private GameObject[,] gridObjects; // グリッドのセルを格納する2D配列

    void Start()
    {
        // グリッドを初期化
        gridObjects = new GameObject[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, z * cellSize);
                gridObjects[x, z] = Instantiate(cellPrefab, position, Quaternion.identity);
                gridObjects[x, z].transform.localScale = new Vector3(cellSize, 0.1f, cellSize); // セルの大きさを調整
            }
        }
    }

    void Update()
    {
        // すべてのセルの色をリセット
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                gridObjects[x, z].GetComponent<Renderer>().material.color = Color.white;
            }
        }

        // すべてのプレハブをチェック
        foreach (GameObject prefab in prefabsToMonitor)
        {
            if (prefab == null) continue; // nullチェック

            // プレハブの現在位置を基にセルのインデックスを計算
            int cellX = Mathf.FloorToInt(prefab.transform.position.x / cellSize);
            int cellZ = Mathf.FloorToInt(prefab.transform.position.z / cellSize);

            // グリッド範囲内にある場合だけ色を変更
            if (cellX >= 0 && cellX < gridSize && cellZ >= 0 && cellZ < gridSize)
            {
                gridObjects[cellX, cellZ].GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }
}
