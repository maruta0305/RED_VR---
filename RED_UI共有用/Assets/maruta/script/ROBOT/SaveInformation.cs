using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveInformation : MonoBehaviour
{
    public Dropdown dropdown1;
    public Dropdown dropdown2;
    private string selectedOption1;
    private string selectedOption2;

    // ExploreParam クラスを定義
    public class ExploreParam
    {
        public string name;  // 1列目: 文字列
        public float value1;   // 6列目: 整数
        public float value2;   // 7列目: 整数

        // コンストラクタ
        public ExploreParam(string name, int value1, int value2)
        {
            this.name = name;
            this.value1 = value1;
            this.value2 = value2;
        }
    }

    public static List<ExploreParam> exploreList = new List<ExploreParam>();


    private string groupsCSV;
    private string parametersCSV;

    // Start is called before the first frame update
    void Start()
    {
        // ドロップダウンの選択が変更された時の処理を設定
        dropdown1.onValueChanged.AddListener(OnDropdown1ValueChanged);
        dropdown2.onValueChanged.AddListener(OnDropdown2ValueChanged);

        // CSVファイルを読み込む
        groupsCSV = LoadCSV(Path.Combine(Application.persistentDataPath, "RED_groups.csv"));
        parametersCSV = LoadCSV(Path.Combine(Application.persistentDataPath, "RED_parameter_sets.csv"));

        // 読み込んだ内容をデバッグログに表示
        if (!string.IsNullOrEmpty(groupsCSV))
        {
            Debug.Log("RED_groups.csv の内容が正常に読み込まれました:\n" + groupsCSV);
        }
        else
        {
            Debug.LogError("RED_groups.csv の読み込みに失敗しました。");
        }

        if (!string.IsNullOrEmpty(parametersCSV))
        {
            Debug.Log("RED_parameter_sets.csv の内容が正常に読み込まれました:\n" + parametersCSV);
        }
        else
        {
            Debug.LogError("RED_parameter_sets.csv の読み込みに失敗しました。");
        }
    }

    // ボタンがクリックされた時に呼ばれる処理
    public void OnSearchButtonClicked()
    {
        // CSVの内容を処理
        if (!string.IsNullOrEmpty(groupsCSV))
        {
            ProcessGroupsCSV(groupsCSV);
        }

        // まずはgroupsCSVを処理後にparametersCSVの処理を行う
        if (!string.IsNullOrEmpty(groupsCSV) && !string.IsNullOrEmpty(parametersCSV))
        {
            ProcessParameterSets(parametersCSV);
        }

        // 最終的なリストをログで表示
        LogFinalList();
    }

    // CSVファイルを読み込むメソッド
    string LoadCSV(string filePath)
    {
        if (File.Exists(filePath))
        {
            string fileContent = File.ReadAllText(filePath); // ファイルの内容を文字列として読み込む
            if (string.IsNullOrEmpty(fileContent))
            {
                Debug.LogWarning($"ファイル {filePath} の内容が空です。");
            }
            return fileContent;
        }
        else
        {
            Debug.LogError($"ファイル {filePath} が見つかりません。");
            return string.Empty;
        }
    }

    // ドロップダウン1の選択肢が変更された時の処理
    void OnDropdown1ValueChanged(int index)
    {
        selectedOption1 = dropdown1.options[index].text;
        Debug.Log("選択されたドロップダウン1のオプション: " + selectedOption1);
    }

    // ドロップダウン2の選択肢が変更された時の処理
    void OnDropdown2ValueChanged(int index)
    {
        selectedOption2 = dropdown2.options[index].text;
        Debug.Log("選択されたドロップダウン2のオプション: " + selectedOption2);
    }

    // RED_groups.csv を処理するメソッド
    void ProcessGroupsCSV(string groupsCSV)
    {
        string[] lines = groupsCSV.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            // 行をカンマで分割して列にする
            string[] columns = line.Split(',');

            if (columns.Length >= 7) // 7列以上の場合
            {
                string name = columns[0].Trim();  // 1列目の名前
                string selectedOption = selectedOption1.Trim();  // ドロップダウン1の選択肢

                // name が selectedOption1 と一致するかチェック
                if (name == selectedOption)
                {
                    // 一致する場合、nameに2列目から7列目を代入し、value1, value2 は 0
                    for (int i = 1; i <= 6; i++) // 2列目から7列目まで
                    {
                        string paramName = columns[i].Trim();
                        ExploreParam param = new ExploreParam(paramName, 0, 0);
                        exploreList.Add(param);  // リストに追加
                        Debug.Log($"名前: {paramName}, value1: {param.value1}, value2: {param.value2}");
                    }
                }
            }
        }
    }

    // RED_parameter_sets.csv を処理するメソッド
    void ProcessParameterSets(string parametersCSV)
    {
        Debug.Log("1");
        string[] lines = parametersCSV.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            Debug.Log("3");
            // 行をカンマで分割して列にする
            string[] columns = line.Split(',');

            if (columns.Length >= 7) // 7列以上の場合
            {
                Debug.Log("4");
                string paramName = columns[0].Trim();  // 1列目の名前
                if (paramName == selectedOption2)
                {
                    Debug.Log("5");
                    // 一致する場合、その行の6列目と7列目を value1 と value2 に代入
                    int value1 = int.Parse(columns[5].Trim());  // 6列目
                    int value2 = int.Parse(columns[6].Trim());  // 7列目

                    // すでにリストに格納した ExploreParam の value1 と value2 を更新
                    for (int i = 0; i < exploreList.Count; i++)
                    {
                        // 更新: value1, value2 を設定
                        exploreList[i].value1 = value1;
                        exploreList[i].value2 = value2;
                        Debug.Log($"更新された値: name: {exploreList[i].name}, value1: {value1}, value2: {value2}");
                        //break;
                        //if (exploreList[i].name == paramName)
                        //{
                            
                        //}
                    }
                }
            }
        }
    }

    // 最終的なリストの内容をログで表示するメソッド
    void LogFinalList()
    {
        Debug.Log("最終的なリスト:");
        foreach (var param in exploreList)
        {
            Debug.Log($"name: {param.name}, value1: {param.value1}, value2: {param.value2}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 必要に応じてUpdate内で処理を追加できます
    }
}
