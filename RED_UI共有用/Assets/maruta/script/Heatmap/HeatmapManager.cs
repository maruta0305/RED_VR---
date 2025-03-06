using System.Collections.Generic;
using UnityEngine;

public class HeatmapManager : MonoBehaviour
{
    public int gridSize = 10;          // �O���b�h�̃Z�����i10�~10�j
    public float cellSize = 1.0f;     // �e�Z���̃T�C�Y
    public GameObject cellPrefab;     // �O���b�h�Z���̃v���n�u
    public List<GameObject> prefabsToMonitor; // �ǐՂ��镡���̃v���n�u���Ǘ�

    private GameObject[,] gridObjects; // �O���b�h�̃Z�����i�[����2D�z��

    void Start()
    {
        // �O���b�h��������
        gridObjects = new GameObject[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, z * cellSize);
                gridObjects[x, z] = Instantiate(cellPrefab, position, Quaternion.identity);
                gridObjects[x, z].transform.localScale = new Vector3(cellSize, 0.1f, cellSize); // �Z���̑傫���𒲐�
            }
        }
    }

    void Update()
    {
        // ���ׂẴZ���̐F�����Z�b�g
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                gridObjects[x, z].GetComponent<Renderer>().material.color = Color.white;
            }
        }

        // ���ׂẴv���n�u���`�F�b�N
        foreach (GameObject prefab in prefabsToMonitor)
        {
            if (prefab == null) continue; // null�`�F�b�N

            // �v���n�u�̌��݈ʒu����ɃZ���̃C���f�b�N�X���v�Z
            int cellX = Mathf.FloorToInt(prefab.transform.position.x / cellSize);
            int cellZ = Mathf.FloorToInt(prefab.transform.position.z / cellSize);

            // �O���b�h�͈͓��ɂ���ꍇ�����F��ύX
            if (cellX >= 0 && cellX < gridSize && cellZ >= 0 && cellZ < gridSize)
            {
                gridObjects[cellX, cellZ].GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }
}
