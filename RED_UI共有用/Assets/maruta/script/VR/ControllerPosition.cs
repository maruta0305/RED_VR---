using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ControllerPosition : MonoBehaviour
{
    public Transform hmd; // HMDのTransform
    public Transform leftController; // 左手コントローラーのTransform
    public Transform rightController; // 右手コントローラーのTransform

    public GameObject axisPrefab; // 座標軸のPrefab
    private GameObject xAxis;
    private GameObject yAxis;
    private GameObject zAxis;

    // UI用のTextオブジェクト
    public Text leftControllerText; // 左手コントローラーの相対位置を表示するText
    public Text rightControllerText; // 右手コントローラーの相対位置を表示するText

    private List<string[]> dataList = new List<string[]>(); // データを保存するリスト
    private float saveInterval = 1f; // 保存の間隔
    private float nextSaveTime = 0f;

    void Start()
    {
        // 座標軸を生成
        xAxis = Instantiate(axisPrefab, hmd.position, Quaternion.Euler(0, 0, 0));
        yAxis = Instantiate(axisPrefab, hmd.position, Quaternion.Euler(0, 90, 0));
        zAxis = Instantiate(axisPrefab, hmd.position, Quaternion.Euler(90, 0, 0));

        // 色を設定
        xAxis.GetComponent<Renderer>().material.color = Color.red;   // X軸
        yAxis.GetComponent<Renderer>().material.color = Color.green; // Y軸
        zAxis.GetComponent<Renderer>().material.color = Color.blue;  // Z軸
    }

    void Update()
    {
        // HMDの位置に基づいて座標軸を更新
        Vector3 hmdPosition = hmd.position;

        xAxis.transform.position = hmdPosition;
        yAxis.transform.position = hmdPosition;
        zAxis.transform.position = hmdPosition;

        // HMDの回転に合わせて座標軸を回転
        Quaternion hmdRotation = hmd.rotation;
        xAxis.transform.rotation = hmdRotation;
        yAxis.transform.rotation = hmdRotation;
        zAxis.transform.rotation = hmdRotation;

        // コントローラーの相対位置を表示
        Vector3 leftRelativePosition = leftController.position - hmdPosition;
        Vector3 rightRelativePosition = rightController.position - hmdPosition;

        // テキストに相対位置を表示
        leftControllerText.text = $"Left Controller Position: {leftRelativePosition}";
        rightControllerText.text = $"Right Controller Position: {rightRelativePosition}";

        // 一定間隔でCSVに保存
        SaveToCSV(rightRelativePosition);
        /*if (Time.time >= nextSaveTime)
        {
            SaveToCSV(rightRelativePosition);
            nextSaveTime = Time.time + saveInterval;
        }*/
    }

    private void SaveToCSV(Vector3 rightRelativePosition)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Positiondata.csv");

        // 現在の時間を取得
        string currentTime = Time.time.ToString("F2"); // 小数点以下2桁まで

        // 右手コントローラーの相対位置をCSVに追加
        string[] data = new string[]
        {
            currentTime,
            rightRelativePosition.x.ToString(),
            rightRelativePosition.y.ToString(),
            rightRelativePosition.z.ToString()
        };

        // CSVファイルを作成または追記
        using (StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.UTF8))
        {
            writer.WriteLine(string.Join(",", data));
        }

        Debug.Log("Data saved to: " + filePath);
    }
}


