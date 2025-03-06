using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using System.Collections.Generic;

//REDのパラメータをグループ化
public class RED_ParamGroup
{
    public string ParamSet_Name_data { get; set; } = "";
    public string IsExploring_data { get; set; } = "";
    public string TransitTime_data { get; set; } = "";
    public string Mu_data { get; set; } = "";
    public string Sigma_data { get; set; } = "";
    public string Outer_Rth_data { get; set; } = "";
    public string Inner_Rth_data { get; set; } = "";
    public string Height_data { get; set; } = "";
    public string BetweenMarkers_data { get; set; } = "";
    public string Height_Correction_data { get; set; } = "";
    public string Reject_data { get; set; } = "";
    public string MarkerFreq_A_data { get; set; } = "";
    public string MarkerFreq_B_data { get; set; } = "";
    public string ShutterSpeed_data { get; set; } = "";
    public string LeftPWM_data { get; set; } = "";
    public string RightPWM_data { get; set; } = "";
    public string Xcoord_data { get; set; } = "";
    public string Ycoord_data { get; set; } = "";
}

public class RED_ParamGroup_Memory
{
    public string ParamSet_Name_data_Memory { get; set; } = "";
    public string IsExploring_data_Memory { get; set; } = "";
    public string TransitTime_data_Memory { get; set; } = "";
    public string Mu_data_Memory { get; set; } = "";
    public string Sigma_data_Memory { get; set; } = "";
    public string Outer_Rth_data_Memory { get; set; } = "";
    public string Inner_Rth_data_Memory { get; set; } = "";
    public string Height_data_Memory { get; set; } = "";
    public string BetweenMarkers_data_Memory { get; set; } = "";
    public string Height_Correction_data_Memory { get; set; } = "";
    public string Reject_data_Memory { get; set; } = "";
    public string MarkerFreq_A_data_Memory { get; set; } = "";
    public string MarkerFreq_B_data_Memory { get; set; } = "";
    public string ShutterSpeed_data_Memory { get; set; } = "";
    public string LeftPWM_data_Memory { get; set; } = "";
    public string RightPWM_data_Memory { get; set; } = "";
    public string Xcoord_data_Memory { get; set; } = "";
    public string Ycoord_data_Memory { get; set; } = "";
}

public class RED_Json_Data
{
    public bool IsExploring;
    public float TransitTime;
    public float Mu;
    public float Sigma;
    public float Outer_Rth;
    public float Inner_Rth;
    public float Height;
    public float BetweenMarkers;
    public bool Height_Correction;
    public string Reject;
    public string MarkerColor;
    public int ShutterSpeed;
    public int LeftPWM;
    public int RightPWM;
    public float Xcoord;
    public float Ycoord;
}

public class RED_ExcelAdd_Data
{
    public string ParamSet_Name_Add;
    public string IsExploring;
    public string TransitTime;
    public string Mu;
    public string Sigma;
    public string Outer_Rth;
    public string Inner_Rth;
    public string Height;
    public string BetweenMarkers;
    public string Height_Correction;
    public string Reject;
    public string MarkerColor_A;
    public string MarkerColor_B;
    public string ShutterSpeed;
    public string LeftPWM;
    public string RightPWM;
    public string Xcoord;
    public string Ycoord;
}

public class RED_Group
{
    public static List<List<string>> RED_Gr = new List<List<string>>();
    public static int RED_Group_Command = 0;
    public static string RED_IP = null;
    public static int RED_Group_num;
}

public class send_data_Algo 
{
    public static string send_all_param = null;
    public static int send_param_count = 0;
    public static string send_json_param = null;
}

public class Algorithm_Mode : MonoBehaviour
{
    bool true_false_judgement = false;
    int Algo_mode = 0;
    int Algo_count = 0;

    int record_ParamGroup_name = 0;

    int null_freq_command = 0;

    int i = 0;
    int z = 0;
    int y = 0;
    int check_duplicate = 0;

    int initial_param_count = 0;
    int excelUpdate_count = 0;

    public GameObject Group_name_dropdown;
    public GameObject Group_IP_dropdown;
    Dropdown ddtmp_Group_name;
    Dropdown ddtmp_Group_Ip;
    public GameObject ParamList_Dropdown;
    public GameObject ParamList_Dropdown_ShutterSpeed;
    public GameObject ParamList_Dropdown_Reject;
    Dropdown ddtmp_Param;
    Dropdown ddtmp_Param_Shutter_Speed;
    Dropdown ddtmp_Param_Reject;
    int ShutterSpeed_Command = 0;
    int Reject_Command = 0;

    //excel読み込み部分//////////////////////////////////////////////////////////////////////////
    string csvFilePath_ParamSet;
    string csvFilePath_ParamSet_2;
    string csvFilePath_REDGroup;
    public string RED_Group_CSV;
    public string RED_Parameter_CSV;
    public List<List<string>> paramSets = new List<List<string>>();
    public List<RED_ParamGroup> RED_Para_List = new List<RED_ParamGroup>();    
    public List<List<string>> paramSets_Memory = new List<List<string>>();
    public List<RED_ParamGroup_Memory> RED_Para_List_Memory = new List<RED_ParamGroup_Memory>();
    RED_ExcelAdd_Data red_para_add = new RED_ExcelAdd_Data();
    //paramSets[][0]=>ParamSet_Name
    //paramSets[][1]=>IsExploring
    //paramSets[][2]=>TransitTime
    //paramSets[][3]=>Mu
    //paramSets[][4]=>Sigma
    //paramSets[][5]=>Outer_Rth
    //paramSets[][6]=>Inner_Rth
    //paramSets[][7]=>Height
    //paramSets[][8]=>BetweenMarkers
    //paramSets[][9]=>Height_Correction
    //paramSets[][10]=>Reject
    //paramSets[][11]=>MarkerColor
    //paramSets[][12]=>ShutterSpeed
    //paramSets[][13]=>LeftPWM
    //paramSets[][14]=>RightPWM
    //paramSets[][15]=>Xcoord
    //paramSets[][16]=>Ycoord
    /// ////////////////////////////////////////////////////////////////////////////////////////////

    /// //////////////////////// jsonファイル作成のためのやつ  ///////////////////////////////////////
    RED_Json_Data red_para = new RED_Json_Data();

    [SerializeField] private Image backgroundImage_Algo;
    [SerializeField] private RectTransform handle_Algo;
    [SerializeField] private Image backgroundImage_Speed;
    [SerializeField] private RectTransform handle_Speed;
    /// <summary>
    /// トグルの値
    /// </summary>
    [NonSerialized] public bool Value_Algo;
    [NonSerialized] public bool Value_Speed;

    private float handlePosX_Algo;
    private Sequence sequence_Algo;
    private float handlePosX_Speed;
    private Sequence sequence_Speed;

    private static readonly Color OFF_BG_COLOR = new Color(255.0f, 255.0f, 255.0f);
    private static readonly Color ON_BG_COLOR = new Color(0.2f, 0.84f, 0.3f);

    private const float SWITCH_DURATION = 0.36f;

    //パラメータを入出力するための親オブジェクト，ここから子を探索して書き込みとかする
    public GameObject Param_Input_List;
    Transform[] children_Param; // 子オブジェクト達を入れる配列
    public InputField[] input_field = new InputField[10];

    //public GameObject test;
    //public GameObject test1;

    //////////  パラメータを送った後に確認する画面関係　　///////
    public GameObject hide_Panel;
    public GameObject Confirm_Parame;
    public GameObject hide_Miss_Panel;
    public GameObject Confirm_Error_Parame;
    public GameObject hide_ExcelUpdate_Panel;
    public GameObject hide_SetName_Panel;

    /////////  Inputfieldで値を入力するところ  ////////////////
    public GameObject Inputfield_ParamSetName;
    public GameObject Inputfield_TransitTime;
    public GameObject Inputfield_Inner;
    public GameObject Inputfield_Mu;
    public GameObject Inputfield_Outer;
    public GameObject Inputfield_Height;
    public GameObject Inputfield_BetweenMark;
    public GameObject Inputfield_MarkerFreq_A;
    public GameObject Inputfield_MarkerFreq_B;
    public GameObject Inputfield_VirtualMarker_x;
    public GameObject Inputfield_VirtualMarker_y;
    InputField input_ParamSetName;
    InputField input_TransitTime;
    InputField input_Inner;
    InputField input_Mu;
    InputField input_Outer;
    InputField input_Height;
    InputField input_BetweenMark;
    InputField input_MarkerFreq_A;
    InputField input_MarkerFreq_B;
    InputField input_VirtualMarker_x;
    InputField input_VirtualMarker_y;

    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log("@@: " + (float)Math.Round(10.111, 1, MidpointRounding.AwayFromZero));
        //InputFieldコンポーネントを取得
        input_ParamSetName = Inputfield_ParamSetName.GetComponent<InputField>();
        input_TransitTime = Inputfield_TransitTime.GetComponent<InputField>();
        input_Inner = Inputfield_Inner.GetComponent<InputField>();
        input_Mu = Inputfield_Mu.GetComponent<InputField>();
        input_Outer = Inputfield_Outer.GetComponent<InputField>();
        input_Height = Inputfield_Height.GetComponent<InputField>();
        input_BetweenMark = Inputfield_BetweenMark.GetComponent<InputField>();
        input_MarkerFreq_A = Inputfield_MarkerFreq_A.GetComponent<InputField>();
        input_MarkerFreq_B = Inputfield_MarkerFreq_B.GetComponent<InputField>();
        input_VirtualMarker_x = Inputfield_VirtualMarker_x.GetComponent<InputField>();
        input_VirtualMarker_y = Inputfield_VirtualMarker_y.GetComponent<InputField>();

        hide_Panel.SetActive(false);
        hide_Miss_Panel.SetActive(false);
        hide_ExcelUpdate_Panel.SetActive(false);

        ddtmp_Group_Ip = Group_IP_dropdown.GetComponent<Dropdown>();
        ddtmp_Param = ParamList_Dropdown.GetComponent<Dropdown>();
        ddtmp_Group_name = Group_name_dropdown.GetComponent<Dropdown>();

        csvFilePath_ParamSet = UnityEngine.Application.persistentDataPath + "/" + RED_Parameter_CSV; // CSVファイルのパス
        csvFilePath_ParamSet_2 = UnityEngine.Application.persistentDataPath + "/" + RED_Parameter_CSV; // CSVファイルのパス
        csvFilePath_REDGroup = UnityEngine.Application.persistentDataPath + "/" + RED_Group_CSV; // CSVファイルのパス
        Debug.Log("csv path: " + UnityEngine.Application.persistentDataPath);

        //Text itemText00 = test.GetComponentInChildren<Text>();
        //itemText00.text = UnityEngine.Application.persistentDataPath;
        // 子オブジェクト達を入れる配列の初期化
        children_Param = new Transform[Param_Input_List.transform.childCount];

        // 検索方法１;
        for (int i = 0; i < Param_Input_List.transform.childCount; i++)
        {
            children_Param[i] = Param_Input_List.transform.GetChild(i); // GetChild()で子オブジェクトを取得
            input_field[i] = children_Param[i].GetComponent<InputField>(); //inputfieldコンポーネントを取得
            //Debug.Log($"検索方法１： {i} 番目の子供は {children_Param[i].name} です");
        }

        handlePosX_Algo = Mathf.Abs(handle_Algo.anchoredPosition.x);
        UpdateToggle_Algo(0);
        handlePosX_Speed = Mathf.Abs(handle_Speed.anchoredPosition.x);
        UpdateToggle_Speed(0);

        ddtmp_Param_Shutter_Speed = ParamList_Dropdown_ShutterSpeed.GetComponent<Dropdown>();
        ddtmp_Param_Reject = ParamList_Dropdown_Reject.GetComponent<Dropdown>();

        ///// excel　/////////////
        ReadCSVFile();
        ReadCSVFile_REDGroup();
       // Debug.Log("++++" + ddtmp_Param.value);

    }

    public void Read()
    {
        ReadCSVFile_REDGroup();
    }

    //public void Pressed()
    //{
    //    StreamReader reader = null;
    //    reader = new StreamReader(csvFilePath_REDGroup);
    //    reader.Close();
    //}

    // Update is called once per frame
    void Update()
    {

        if (initial_param_count < 2)
        {
            initial_param_view();
            initial_param_count += 1;
        }


    }


    /////////////// パラメータセットのデータを可視化する関数　　////////////////////////////////////////////////////////
    public void param_view(int set_num)
    {

        Debug.Log("TransitTime: " + input_TransitTime.text);
        //タブレットかするとき0に変更しないといけないかも（PCの時は1かも？？）
        
        children_Param[0].transform.GetChild(1).gameObject.GetComponent<Text>().text = RED_Para_List[set_num].TransitTime_data;
        children_Param[1].transform.GetChild(1).gameObject.GetComponent<Text>().text = RED_Para_List[set_num].Inner_Rth_data;
        children_Param[2].transform.GetChild(1).gameObject.GetComponent<Text>().text = RED_Para_List[set_num].Mu_data;
        children_Param[3].transform.GetChild(1).gameObject.GetComponent<Text>().text = RED_Para_List[set_num].Outer_Rth_data;
        children_Param[4].transform.GetChild(1).gameObject.GetComponent<Text>().text = RED_Para_List[set_num].Height_data;
        children_Param[5].transform.GetChild(1).gameObject.GetComponent<Text>().text = RED_Para_List[set_num].BetweenMarkers_data;
        children_Param[6].transform.GetChild(1).gameObject.GetComponent<Text>().text = RED_Para_List[set_num].MarkerFreq_A_data;
        children_Param[7].transform.GetChild(1).gameObject.GetComponent<Text>().text = RED_Para_List[set_num].MarkerFreq_B_data;
        children_Param[8].transform.GetChild(1).gameObject.GetComponent<Text>().text = RED_Para_List[set_num].Xcoord_data;
        children_Param[9].transform.GetChild(1).gameObject.GetComponent<Text>().text = RED_Para_List[set_num].Ycoord_data;

        //if (RED_Para_List[set_num].Height_Correction_data.Equals("TRUE") && true_false_judgement == false)
        //{
        //    SwitchToggle_Speed();
        //    Debug.Log("koko1");
        //}
        //else
        //{
        //    Debug.Log("Height Collection False");
        //}

        ddtmp_Param_Shutter_Speed.captionText.text = RED_Para_List[set_num].ShutterSpeed_data;
        ddtmp_Param_Reject.captionText.text = RED_Para_List[set_num].Reject_data;

    }

    /// パラメータセットを増やすドロップダウン
    public void dropdown_paramset(List<RED_ParamGroup> data_paramset, int data_size)
    {
        List<string> ParamList = new List<string>();

        for (int j = 0; j < data_size - 1; j++)
        {
            //Optionsに表示する文字列をリストに追加
            ParamList.Add(data_paramset[j].ParamSet_Name_data);
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp_Param = ParamList_Dropdown.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Param.ClearOptions();

        //リストを追加
        ddtmp_Param.AddOptions(ParamList);

        //Debug.Log("ddtmp_Param.value: " + ddtmp_Param.value);

    }
    /// </summary>
    /// 
    public void dropdown_param_call(int num)
    {
        Debug.Log("++++" + ddtmp_Param.value);
        Debug.Log(ddtmp_Param.options[ddtmp_Param.value].text);
        if (RED_Para_List[ddtmp_Param.value].IsExploring_data.Equals("TRUE") && RED_Para_List[record_ParamGroup_name].IsExploring_data.Equals("TRUE"))
        {
            Debug.Log("TRUE");
        }
        else if (RED_Para_List[ddtmp_Param.value].IsExploring_data.Equals("TRUE") && RED_Para_List[record_ParamGroup_name].IsExploring_data.Equals("FALSE"))
        {
            SwitchToggle_Algo();
            Debug.Log("change1");
        }
        else if (RED_Para_List[ddtmp_Param.value].IsExploring_data.Equals("FALSE") && RED_Para_List[record_ParamGroup_name].IsExploring_data.Equals("TRUE"))
        {
            SwitchToggle_Algo();
            Debug.Log("change2");
        }
        else if (RED_Para_List[ddtmp_Param.value].IsExploring_data.Equals("FALSE") && RED_Para_List[record_ParamGroup_name].IsExploring_data.Equals("FALSE"))
        {
            Debug.Log("TRUE");
        }

        if (input_TransitTime.text != "")
        {
            RED_Para_List[record_ParamGroup_name].TransitTime_data = input_TransitTime.text;
        }
        if (input_Inner.text != "")
        {
            RED_Para_List[record_ParamGroup_name].Inner_Rth_data = input_Inner.text;
        }
        if (input_Mu.text != "")
        {
            RED_Para_List[record_ParamGroup_name].Mu_data = input_Mu.text;
        }
        if (input_Outer.text != "")
        {
            RED_Para_List[record_ParamGroup_name].Outer_Rth_data = input_Outer.text;
        }
        if (input_Height.text != "")
        {
            RED_Para_List[record_ParamGroup_name].Height_data = input_Height.text;
        }
        if (input_BetweenMark.text != "")
        {
            RED_Para_List[record_ParamGroup_name].BetweenMarkers_data = input_BetweenMark.text;
        }
        if (input_MarkerFreq_A.text != "")
        {
            RED_Para_List[record_ParamGroup_name].MarkerFreq_A_data = input_MarkerFreq_A.text;
        }
        if (input_MarkerFreq_B.text != "")
        {
            RED_Para_List[record_ParamGroup_name].MarkerFreq_B_data = input_MarkerFreq_B.text;
        }
        if (input_VirtualMarker_x.text != "")
        {
            RED_Para_List[record_ParamGroup_name].Xcoord_data = input_VirtualMarker_x.text;
        }
        if (input_VirtualMarker_y.text != "")
        {
            RED_Para_List[record_ParamGroup_name].Ycoord_data = input_VirtualMarker_y.text;
        }
        if (ShutterSpeed_Command == 1)
        {
            RED_Para_List[record_ParamGroup_name].ShutterSpeed_data = ddtmp_Param_Shutter_Speed.options[ddtmp_Param_Shutter_Speed.value].text;
            ShutterSpeed_Command = 0;
        }
        if (Reject_Command == 1)
        {
            RED_Para_List[record_ParamGroup_name].Reject_data = ddtmp_Param_Reject.options[ddtmp_Param_Reject.value].text;
            ShutterSpeed_Command = 0;
        }

        if (RED_Para_List[ddtmp_Param.value].Height_Correction_data.Equals("TRUE") && RED_Para_List[record_ParamGroup_name].Height_Correction_data.Equals("TRUE"))
        {
            Debug.Log("TRUE");
        }
        else if (RED_Para_List[ddtmp_Param.value].Height_Correction_data.Equals("TRUE") && RED_Para_List[record_ParamGroup_name].Height_Correction_data.Equals("FALSE"))
        {
            SwitchToggle_Speed();
            Debug.Log("change1");
        }
        else if (RED_Para_List[ddtmp_Param.value].Height_Correction_data.Equals("FALSE") && RED_Para_List[record_ParamGroup_name].Height_Correction_data.Equals("TRUE"))
        {
            SwitchToggle_Speed();
            Debug.Log("change2");
        }
        else if (RED_Para_List[ddtmp_Param.value].Height_Correction_data.Equals("FALSE") && RED_Para_List[record_ParamGroup_name].Height_Correction_data.Equals("FALSE"))
        {
            Debug.Log("TRUE");
        }

        if (excelUpdate_count == 0)
        {
            intialize_inputfield();

            record_ParamGroup_name = ddtmp_Param.value;

            param_view(ddtmp_Param.value);
        }

        //dropdown_paramset(paramSets, ddtmp_Param.value);

    }

    public void dropdown_redgroup(List<List<string>> data_redgroup, int data_group_size)
    {
        List<string> REDGrList = new List<string>();

        REDGrList.Add("Select RED Group");
        for (int j = 0; j < data_group_size; j++)
        {
            //Optionsに表示する文字列をリストに追加
            REDGrList.Add(data_redgroup[j][0]);
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp_Group_name = Group_name_dropdown.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_name.ClearOptions();

        //リストを追加
        ddtmp_Group_name.AddOptions(REDGrList);
    }

    public void dropdown_Group_call(int num)
    {
        if (ddtmp_Group_name.value == 0)
        {
            Debug.Log("Skip");
            dropdown_redgroupIP_nothing();
        }
        else
        {
            RED_Group.RED_Group_num = ddtmp_Group_name.value - 1;
            Debug.Log("++++" + RED_Group.RED_Group_num);
            Debug.Log(ddtmp_Group_name.options[ddtmp_Group_name.value].text);
            dropdown_redgroupIP(RED_Group.RED_Gr, ddtmp_Group_name.value - 1);

        }
    }

    public void dropdown_redgroupIP(List<List<string>> data_redgroupIP, int data_group_num)
    {
        List<string> REDGrIP_List = new List<string>();

        for (int j = 0; j < data_redgroupIP[data_group_num].Count + 1; j++)
        {
            if(j == 0)
            {
                //Optionsに表示する文字列をリストに追加
                REDGrIP_List.Add("Select RED Robots");
            }
            else if (j == data_redgroupIP[data_group_num].Count)
            {
                REDGrIP_List.Add("ALL RED");
            }
            else
            {
                //Optionsに表示する文字列をリストに追加
                REDGrIP_List.Add(data_redgroupIP[data_group_num][j]);
            }
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp_Group_Ip = Group_IP_dropdown.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_Ip.ClearOptions();

        //リストを追加
        ddtmp_Group_Ip.AddOptions(REDGrIP_List);
    }

    public void dropdown_redgroupIP_nothing()
    {
        List<string> REDGrIP_List = new List<string>();

        for (int j = 0; j < 2; j++)
        {
            if (j == 0)
            {
                //Optionsに表示する文字列をリストに追加
                REDGrIP_List.Add("Select RED Robots");
            }
            else
            {
                //Optionsに表示する文字列をリストに追加
                REDGrIP_List.Add("None");
            }
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp_Group_Ip = Group_IP_dropdown.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_Ip.ClearOptions();

        //リストを追加
        ddtmp_Group_Ip.AddOptions(REDGrIP_List);
    }

    public void dropdown_RobotIP_call(int num)
    {
        Debug.Log(ddtmp_Group_Ip.options[ddtmp_Group_Ip.value].text);
        RED_Group.RED_IP = ddtmp_Group_Ip.options[ddtmp_Group_Ip.value].text;
        tcpip_data.ip_addr = ddtmp_Group_Ip.options[ddtmp_Group_Ip.value].text;
    }

    public void dropdown_RejectMode(int num)
    {
        Reject_Command = 1;
        Debug.Log("++++" + ddtmp_Param_Reject.value);
        Debug.Log(ddtmp_Param_Reject.options[ddtmp_Param_Reject.value].text);
    }

    public void dropdown_ShutterSpeed(int num)
    {
        ShutterSpeed_Command = 1;
        Debug.Log("++++" + ddtmp_Param_Shutter_Speed.value);
        Debug.Log(ddtmp_Param_Shutter_Speed.options[ddtmp_Param_Shutter_Speed.value].text);
    }

    public void send_param_button()
    {
        //if(Algo_mode == 1)
        //{
            red_para.IsExploring = System.Convert.ToBoolean(RED_Para_List[ddtmp_Param.value].IsExploring_data);
            if(input_TransitTime.text != "")
            {
            red_para.TransitTime = (float)Math.Round(float.Parse(input_TransitTime.text), 1, MidpointRounding.AwayFromZero);
            }
            else
            {
                red_para.TransitTime = (float)Math.Round(float.Parse(RED_Para_List[ddtmp_Param.value].TransitTime_data), 1, MidpointRounding.AwayFromZero);
            }
            if (input_Inner.text != "")
            {
                red_para.Inner_Rth = (float)Math.Round(float.Parse(input_Inner.text), 1, MidpointRounding.AwayFromZero);
            }
            else
            {
                red_para.Inner_Rth = (float)Math.Round(float.Parse(RED_Para_List[ddtmp_Param.value].Inner_Rth_data), 1, MidpointRounding.AwayFromZero);
            }
            if (input_Mu.text != "")
            {
                red_para.Mu = (float)Math.Round(float.Parse(input_Mu.text), 1, MidpointRounding.AwayFromZero);
            }
            else
            {
                red_para.Mu = (float)Math.Round(float.Parse(RED_Para_List[ddtmp_Param.value].Mu_data), 1, MidpointRounding.AwayFromZero);
            }
            if (input_Outer.text != "")
            {
                red_para.Outer_Rth = (float)Math.Round(float.Parse(input_Outer.text), 1, MidpointRounding.AwayFromZero);
            }
            else
            {
                red_para.Outer_Rth = (float)Math.Round(float.Parse(RED_Para_List[ddtmp_Param.value].Outer_Rth_data), 1, MidpointRounding.AwayFromZero);
            }
            if (input_Height.text != "")
            {
                red_para.Height = (float)Math.Round(float.Parse(input_Height.text), 1, MidpointRounding.AwayFromZero);
            }
            else
            {
                red_para.Height = (float)Math.Round(float.Parse(RED_Para_List[ddtmp_Param.value].Height_data), 1, MidpointRounding.AwayFromZero);
            }
            if (input_BetweenMark.text != "")
            {
                red_para.BetweenMarkers = (float)Math.Round(float.Parse(input_BetweenMark.text), 1, MidpointRounding.AwayFromZero);
            }
            else
            {
                red_para.BetweenMarkers = (float)Math.Round(float.Parse(RED_Para_List[ddtmp_Param.value].BetweenMarkers_data), 1, MidpointRounding.AwayFromZero);
            }

            red_para.Height_Correction = System.Convert.ToBoolean(RED_Para_List[ddtmp_Param.value].Height_Correction_data);

            if (input_VirtualMarker_x.text != "")
            {
                red_para.Xcoord = (float)Math.Round(float.Parse(input_VirtualMarker_x.text), 1, MidpointRounding.AwayFromZero);
            }
            else
            {
                red_para.Xcoord = (float)Math.Round(float.Parse(RED_Para_List[ddtmp_Param.value].Xcoord_data), 1, MidpointRounding.AwayFromZero);
            }
            if (input_VirtualMarker_y.text != "")
            {
                red_para.Ycoord = (float)Math.Round(float.Parse(input_VirtualMarker_y.text), 1, MidpointRounding.AwayFromZero);
            }
            else
            {
                red_para.Ycoord = (float)Math.Round(float.Parse(RED_Para_List[ddtmp_Param.value].Ycoord_data), 1, MidpointRounding.AwayFromZero);
            }

            if(Reject_Command == 1)
            {
                red_para.Reject = ddtmp_Param_Reject.options[ddtmp_Param_Reject.value].text;
                Reject_Command = 0;
            }
            else
            {
                red_para.Reject = RED_Para_List[ddtmp_Param.value].Reject_data;
            }
            if(ShutterSpeed_Command == 1)
            {
                red_para.ShutterSpeed = int.Parse(ddtmp_Param_Shutter_Speed.options[ddtmp_Param_Shutter_Speed.value].text);
                ShutterSpeed_Command = 0;
            }
            else
            {
                red_para.ShutterSpeed = int.Parse(RED_Para_List[ddtmp_Param.value].ShutterSpeed_data);
            }

            red_para.Sigma = (float)Math.Round(float.Parse(RED_Para_List[ddtmp_Param.value].Sigma_data), 1, MidpointRounding.AwayFromZero);

            if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                null_freq_command = 1;
                Debug.Log("Freq null");
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para.MarkerColor = RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para.MarkerColor = RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para.MarkerColor = RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data + "_" + RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para.MarkerColor = input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para.MarkerColor = input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para.MarkerColor = RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data + "_" + input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para.MarkerColor = RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data + "_" + input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para.MarkerColor = input_MarkerFreq_A.text;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para.MarkerColor = input_MarkerFreq_A.text + "_" + RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para.MarkerColor = input_MarkerFreq_A.text;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para.MarkerColor = input_MarkerFreq_A.text + "_" + RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para.MarkerColor = input_MarkerFreq_A.text + "_" + input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para.MarkerColor = input_MarkerFreq_A.text + "_" + input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para.MarkerColor = input_MarkerFreq_A.text + "_" + input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para.MarkerColor = input_MarkerFreq_A.text + "_" + input_MarkerFreq_B.text;
            }

            red_para.LeftPWM = int.Parse(RED_Para_List[ddtmp_Param.value].LeftPWM_data);
            red_para.RightPWM = int.Parse(RED_Para_List[ddtmp_Param.value].RightPWM_data);

            // JSON形式に変換
            //send_data_Algo.send_json_param = JsonSerializer.Serialize(new { RoundedValue = red_para });
            send_data_Algo.send_json_param = JsonUtility.ToJson(red_para);

            // 結果を表示
            Debug.Log("@@@@@@@@@@@@@@@@@: " + red_para.TransitTime);
            Debug.Log("@@@@@@@@@@@@@@@@@: " + red_para.Xcoord);
            Debug.Log("@@@@@@@@@@@@@@@@@: " + send_data_Algo.send_json_param);

            if(null_freq_command == 1)
            {
                Confirm_Error_Parame.GetComponent<Text>().text = "Please input at least one Marker Frequency";
                hide_Miss_Panel.SetActive(true);
                null_freq_command = 0;
            }
            else
            {
                send_data_Algo.send_param_count = 1;
                Confirm_Parame.GetComponent<Text>().text = send_data_Algo.send_json_param;
                hide_Panel.SetActive(true);
                null_freq_command = 0;
            }
            
        //}
        //else
        //{
        //    hide_Miss_Panel.SetActive(true);
        //    Confirm_Error_Parame.GetComponent<Text>().text = "Please turn on the algorithm mode";
        //    Debug.Log("AlgoMode Off 送信失敗");
        //}
    }

    public void hidePanel_close_Button()
    {
        hide_Panel.SetActive(false);
    }
    public void hideMissPanel_close_Button()
    {
        hide_Miss_Panel.SetActive(false);
    }

    ///////////       excel 関係(読み込み関数) ////////////////////////////////////
    public void ReadCSVFile_REDGroup()
    {
        if (File.Exists(csvFilePath_REDGroup))
        {
            string[] lines_1 = File.ReadAllLines(csvFilePath_REDGroup);

            z = 0;

            foreach (string line_1 in lines_1)
            {
                if (z != 0)
                {
                    // ","で分割してリストに追加
                    List<string> row_1 = new List<string>(line_1.Split(','));
                    List<string> row_use = new List<string>();

                    for (int i = 0; i < row_1.Count; i++)
                    {
                        if(row_1[i] != "")
                        {
                            row_use.Add(row_1[i]);
//                            Debug.Log("CSV row: " + row_1[i]);
                        }
                    }
                    RED_Group.RED_Gr.Add(row_use);
                }
                z += 1;
            }
            //Debug.Log("@@@@@@: " + row.Count);
        }
        else
        {
            //Text itemText00 = test1.GetComponentInChildren<Text>();
            //itemText00.text = "RED_parameter_sets.csv does NOT exist";
            Debug.LogError("RED_parameter_sets.csv does NOT exist");
        }

        //for(int i = 0; i < RED_Group.RED_Gr.Count; i++)
        //{
        //    for(int j = 0; j < RED_Group.RED_Gr[i].Count; j++)
        //    {
        //        Debug.Log("data[" + i + "][" + j + "]: " + RED_Group.RED_Gr[i][j]);
        //    }
        //}

        dropdown_redgroup(RED_Group.RED_Gr, RED_Group.RED_Gr.Count);
        dropdown_redgroupIP_nothing();
        RED_Group.RED_Group_Command = 1;
        //Debug.Log("RED_Group.RED_Group_Command: " + RED_Group.RED_Group_Command);
        StreamReader reader = null;
        reader = new StreamReader(csvFilePath_REDGroup);
        reader.Close();
    }


    void ReadCSVFile()
    {
        if (File.Exists(csvFilePath_ParamSet))
        {
            string[] lines = File.ReadAllLines(csvFilePath_ParamSet);

            z = 0;

            foreach (string line in lines)
            {
                // ","で分割してリストに追加
                List<string> row = new List<string>(line.Split(','));
                List<string> freAB = new List<string>(row[11].Split('_'));
                row.RemoveAt(11);
                if(freAB.Count == 2)
                {
                }
                else
                {
                    freAB.Add("");       
                }
                row.InsertRange(11, freAB);
                paramSets.Add(row);
                paramSets_Memory.Add(row);

                z += 1;
                //Debug.Log("@@@@@@: " + row.Count);
            }
        }
        else
        {
            //Text itemText00 = test1.GetComponentInChildren<Text>();
            //itemText00.text = "RED_parameter_sets.csv does NOT exist";
            Debug.LogError("RED_parameter_sets.csv does NOT exist");
        }
        DebugparamSets();
    }

    //書き込み関数//
    public void Excel_UpdateCheck_Button()
    {
        hide_ExcelUpdate_Panel.SetActive(true);
    }
    public void No_Button()
    {
        hide_ExcelUpdate_Panel.SetActive(false);
    }
    public void OK_Button()
    {
        hide_SetName_Panel.SetActive(false);
    }
    public void Excel_Updata()
    {
        excelUpdate_count = 1;
        dropdown_param_call(ddtmp_Param.value);
        try
        {
            if (File.Exists(csvFilePath_ParamSet_2))
            {
                Debug.Log("Push excel");
                // CSVファイルのすべての行を読み込む
                string[] lines_2 = File.ReadAllLines(csvFilePath_ParamSet_2);
                i = 0;

                // 各行に対して処理を行う
                foreach (string line_22 in lines_2)
                {
                    // カンマで分割して各列を取得
                    string[] columns = line_22.Split(',');
                    if (i != 0)
                    {
                        columns[0] = RED_Para_List[i - 1].ParamSet_Name_data;
                        columns[1] = RED_Para_List[i - 1].IsExploring_data;
                        columns[2] = RED_Para_List[i - 1].TransitTime_data;
                        columns[3] = RED_Para_List[i - 1].Mu_data;
                        columns[4] = RED_Para_List[i - 1].Sigma_data;
                        columns[5] = RED_Para_List[i - 1].Outer_Rth_data;
                        columns[6] = RED_Para_List[i - 1].Inner_Rth_data;
                        columns[7] = RED_Para_List[i - 1].Height_data;
                        columns[8] = RED_Para_List[i - 1].BetweenMarkers_data;
                        columns[9] = RED_Para_List[i - 1].Height_Correction_data;
                        columns[10] = RED_Para_List[i - 1].Reject_data;
                        if (RED_Para_List[i - 1].MarkerFreq_A_data != "" && RED_Para_List[i - 1].MarkerFreq_B_data != "")
                        {
                            columns[11] = RED_Para_List[i - 1].MarkerFreq_A_data + "_" + RED_Para_List[i - 1].MarkerFreq_B_data;
                        }
                        else if (RED_Para_List[i - 1].MarkerFreq_A_data == "" && RED_Para_List[i - 1].MarkerFreq_B_data != "")
                        {
                            columns[11] = RED_Para_List[i - 1].MarkerFreq_B_data;
                        }
                        else if (RED_Para_List[i - 1].MarkerFreq_A_data != "" && RED_Para_List[i - 1].MarkerFreq_B_data == "")
                        {
                            columns[11] = RED_Para_List[i - 1].MarkerFreq_A_data;
                        }
                        else
                        {
                            columns[11] = "";
                        }
                        columns[12] = RED_Para_List[i - 1].ShutterSpeed_data;
                        columns[13] = RED_Para_List[i - 1].LeftPWM_data;
                        columns[14] = RED_Para_List[i - 1].RightPWM_data;
                        columns[15] = RED_Para_List[i - 1].Xcoord_data;
                        columns[16] = RED_Para_List[i - 1].Ycoord_data;
                        Debug.Log("Update transittime"+columns[2]);
                       
                    }
                    // カンマで連結して新しい行を作成
                    lines_2[i] = string.Join(",", columns);

                    i += 1;
                }

                // 書き換えたデータをファイルに書き込む
                File.WriteAllLines(csvFilePath_ParamSet, lines_2);

                Console.WriteLine("CSVファイルの書き換えが完了しました。");
            }
            else
            {
                Debug.LogError("RED_parameter_sets.csv does NOT exist");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("エラーが発生しました: " + ex.Message);
        }
        hide_ExcelUpdate_Panel.SetActive(false);
        excelUpdate_count = 0;
    }

    public void Excel_Add_Data()
    {
        if(input_ParamSetName.text == "")
        {
            hide_SetName_Panel.SetActive(true);
        }
        else
        {
            red_para_add.ParamSet_Name_Add = input_ParamSetName.text;
            red_para_add.IsExploring = RED_Para_List[ddtmp_Param.value].IsExploring_data;
            if (input_TransitTime.text != "")
            {
                red_para_add.TransitTime = input_TransitTime.text;
            }
            else
            {
                red_para_add.TransitTime = RED_Para_List[ddtmp_Param.value].TransitTime_data;
            }
            if (input_Inner.text != "")
            {
                red_para_add.Inner_Rth = input_Inner.text;
            }
            else
            {
                red_para_add.Inner_Rth = RED_Para_List[ddtmp_Param.value].Inner_Rth_data;
            }
            if (input_Mu.text != "")
            {
                red_para_add.Mu = input_Mu.text;
            }
            else
            {
                red_para_add.Mu = RED_Para_List[ddtmp_Param.value].Mu_data;
            }
            if (input_Outer.text != "")
            {
                red_para_add.Outer_Rth = input_Outer.text;
            }
            else
            {
                red_para_add.Outer_Rth = RED_Para_List[ddtmp_Param.value].Outer_Rth_data;
            }
            if (input_Height.text != "")
            {
                red_para_add.Height = input_Height.text;
            }
            else
            {
                red_para_add.Height = RED_Para_List[ddtmp_Param.value].Height_data;
            }
            if (input_BetweenMark.text != "")
            {
                red_para_add.BetweenMarkers = input_BetweenMark.text;
            }
            else
            {
                red_para_add.BetweenMarkers = RED_Para_List[ddtmp_Param.value].BetweenMarkers_data;
            }

            red_para_add.Height_Correction = RED_Para_List[ddtmp_Param.value].Height_Correction_data;

            if (input_VirtualMarker_x.text != "")
            {
                red_para_add.Xcoord = input_VirtualMarker_x.text;
            }
            else
            {
                red_para_add.Xcoord = RED_Para_List[ddtmp_Param.value].Xcoord_data;
            }
            if (input_VirtualMarker_y.text != "")
            {
                red_para_add.Ycoord = input_VirtualMarker_y.text;
            }
            else
            {
                red_para_add.Ycoord = RED_Para_List[ddtmp_Param.value].Ycoord_data;
            }

            if (Reject_Command == 1)
            {
                red_para_add.Reject = ddtmp_Param_Reject.options[ddtmp_Param_Reject.value].text;
                Reject_Command = 0;
            }
            else
            {
                red_para_add.Reject = RED_Para_List[ddtmp_Param.value].Reject_data;
            }
            if (ShutterSpeed_Command == 1)
            {
                red_para_add.ShutterSpeed = ddtmp_Param_Shutter_Speed.options[ddtmp_Param_Shutter_Speed.value].text;
                ShutterSpeed_Command = 0;
            }
            else
            {
                red_para_add.ShutterSpeed = RED_Para_List[ddtmp_Param.value].ShutterSpeed_data;
            }

            red_para_add.Sigma = RED_Para_List[ddtmp_Param.value].Sigma_data;


            if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                null_freq_command = 1;
                Debug.Log("Freq null");
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para_add.MarkerColor_A = "";
                red_para_add.MarkerColor_B = RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                //Debug.Log("kokoooooooo");
                red_para_add.MarkerColor_A = RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data;
                red_para_add.MarkerColor_B = "";
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para_add.MarkerColor_A = RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data;
                red_para_add.MarkerColor_B = RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para_add.MarkerColor_A = "";
                red_para_add.MarkerColor_B = input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para_add.MarkerColor_A = "";
                red_para_add.MarkerColor_B = input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para_add.MarkerColor_A = RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data;
                red_para_add.MarkerColor_B = input_MarkerFreq_B.text; ;
            }
            else if (input_MarkerFreq_A.text == "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para_add.MarkerColor_A = RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data;
                red_para_add.MarkerColor_B = input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para_add.MarkerColor_A = input_MarkerFreq_A.text;
                red_para_add.MarkerColor_B = "";
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para_add.MarkerColor_A = input_MarkerFreq_A.text;
                red_para_add.MarkerColor_B = RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para_add.MarkerColor_A = input_MarkerFreq_A.text;
                red_para_add.MarkerColor_B = "";
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para_add.MarkerColor_A = input_MarkerFreq_A.text;
                red_para_add.MarkerColor_B = RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para_add.MarkerColor_A = input_MarkerFreq_A.text;
                red_para_add.MarkerColor_B = input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data == "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para_add.MarkerColor_A = input_MarkerFreq_A.text;
                red_para_add.MarkerColor_B = input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data == "")
            {
                red_para_add.MarkerColor_A = input_MarkerFreq_A.text;
                red_para_add.MarkerColor_B = input_MarkerFreq_B.text;
            }
            else if (input_MarkerFreq_A.text != "" && input_MarkerFreq_B.text != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data != "" && RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data != "")
            {
                red_para_add.MarkerColor_A = input_MarkerFreq_A.text;
                red_para_add.MarkerColor_B = input_MarkerFreq_B.text;
            }

            red_para_add.LeftPWM = RED_Para_List[ddtmp_Param.value].LeftPWM_data;
            red_para_add.RightPWM = RED_Para_List[ddtmp_Param.value].RightPWM_data;

            for (int i = 0; i < RED_Para_List.Count; i++)
            {
                if (input_ParamSetName.text == RED_Para_List[i].ParamSet_Name_data)
                {
                    Debug.Log("Skip");
                }
                else
                {
                    check_duplicate += 1;
                }
                Debug.Log("Data: " + RED_Para_List[i].ParamSet_Name_data + " count; " + RED_Para_List.Count);
            }
            Debug.Log(check_duplicate);

            if (check_duplicate != RED_Para_List.Count)
            {
                hide_SetName_Panel.SetActive(true);
                Debug.Log("名前が重複してます");
            }
            else
            {
                RED_Para_List.Add(new RED_ParamGroup
                {
                    ParamSet_Name_data = red_para_add.ParamSet_Name_Add,
                    IsExploring_data = red_para_add.IsExploring,
                    TransitTime_data = red_para_add.TransitTime,
                    Mu_data = red_para_add.Mu,
                    Sigma_data = red_para_add.Sigma,
                    Outer_Rth_data = red_para_add.Outer_Rth,
                    Inner_Rth_data = red_para_add.Inner_Rth,
                    Height_data = red_para_add.Height,
                    BetweenMarkers_data = red_para_add.BetweenMarkers,
                    Height_Correction_data = red_para_add.Height_Correction,
                    Reject_data = red_para_add.Reject,
                    MarkerFreq_A_data = red_para_add.MarkerColor_A,
                    MarkerFreq_B_data = red_para_add.MarkerColor_B,
                    ShutterSpeed_data = red_para_add.ShutterSpeed,
                    LeftPWM_data = red_para_add.LeftPWM,
                    RightPWM_data = red_para_add.RightPWM,
                    Xcoord_data = red_para_add.Xcoord,
                    Ycoord_data = red_para_add.Ycoord
                });
                dropdown_paramset(RED_Para_List, RED_Para_List.Count + 1);

                try
                {
                    List<string> lines = new List<string>();

                    // 既存のデータを読み込む
                    if (File.Exists(csvFilePath_ParamSet_2))
                    {
                        lines.AddRange(File.ReadAllLines(csvFilePath_ParamSet_2));
                    }
                    string[] columns_Add = new string[17];

                    columns_Add[0] = RED_Para_List[RED_Para_List.Count - 1].ParamSet_Name_data;
                    columns_Add[1] = RED_Para_List[RED_Para_List.Count - 1].IsExploring_data;
                    columns_Add[2] = RED_Para_List[RED_Para_List.Count - 1].TransitTime_data;
                    columns_Add[3] = RED_Para_List[RED_Para_List.Count - 1].Mu_data;
                    columns_Add[4] = RED_Para_List[RED_Para_List.Count - 1].Sigma_data;
                    columns_Add[5] = RED_Para_List[RED_Para_List.Count - 1].Outer_Rth_data;
                    columns_Add[6] = RED_Para_List[RED_Para_List.Count - 1].Inner_Rth_data;
                    columns_Add[7] = RED_Para_List[RED_Para_List.Count - 1].Height_data;
                    columns_Add[8] = RED_Para_List[RED_Para_List.Count - 1].BetweenMarkers_data;
                    columns_Add[9] = RED_Para_List[RED_Para_List.Count - 1].Height_Correction_data;
                    columns_Add[10] = RED_Para_List[RED_Para_List.Count - 1].Reject_data;
                    if (RED_Para_List[RED_Para_List.Count - 1].MarkerFreq_A_data != "" && RED_Para_List[RED_Para_List.Count - 1].MarkerFreq_B_data != "")
                    {
                        columns_Add[11] = RED_Para_List[RED_Para_List.Count - 1].MarkerFreq_A_data + "_" + RED_Para_List[RED_Para_List.Count - 1].MarkerFreq_B_data;
                    }
                    else if (RED_Para_List[RED_Para_List.Count - 1].MarkerFreq_A_data == "" && RED_Para_List[RED_Para_List.Count - 1].MarkerFreq_B_data != "")
                    {
                        columns_Add[11] = RED_Para_List[RED_Para_List.Count - 1].MarkerFreq_B_data;
                    }
                    else if (RED_Para_List[RED_Para_List.Count - 1].MarkerFreq_A_data != "" && RED_Para_List[RED_Para_List.Count - 1].MarkerFreq_B_data == "")
                    {
                        columns_Add[11] = RED_Para_List[RED_Para_List.Count - 1].MarkerFreq_A_data;
                    }
                    else
                    {
                        columns_Add[11] = "";
                    }
                    columns_Add[12] = RED_Para_List[RED_Para_List.Count - 1].ShutterSpeed_data;
                    columns_Add[13] = RED_Para_List[RED_Para_List.Count - 1].LeftPWM_data;
                    columns_Add[14] = RED_Para_List[RED_Para_List.Count - 1].RightPWM_data;
                    columns_Add[15] = RED_Para_List[RED_Para_List.Count - 1].Xcoord_data;
                    columns_Add[16] = RED_Para_List[RED_Para_List.Count - 1].Ycoord_data;
                    // 新しい行を追加
                    string newLine = string.Join(",", columns_Add);
                    lines.Add(newLine);

                    // ファイルに書き込む
                    File.WriteAllLines(csvFilePath_ParamSet_2, lines.ToArray());

                    Debug.Log("新しい行がエクセルに追加されました。");
                }
                catch (Exception ex)
                {
                    Debug.LogError("エラーが発生しました: " + ex.Message);
                }
            }
        }

        check_duplicate = 0;
    }
    ///////////////////////////////////////////////////////////////////////////////////

    void DebugparamSets()
    {
        for (int i = 0; i < paramSets.Count - 1; i++)
        {
            //Debug.Log("Size: " + paramSets[i].Count);
            //for (int j = 0; j < paramSets[i].Count; j++)
            //{
            //    Debug.Log("csvDatas[" + i + "," + j + "] = " + paramSets[i][j]);
            //}
            RED_Para_List.Add(new RED_ParamGroup
            {
                ParamSet_Name_data = paramSets[i + 1][0],
                IsExploring_data = paramSets[i + 1][1],
                TransitTime_data = paramSets[i + 1][2],
                Mu_data = paramSets[i + 1][3],
                Sigma_data = paramSets[i + 1][4],
                Outer_Rth_data = paramSets[i + 1][5],
                Inner_Rth_data = paramSets[i + 1][6],
                Height_data = paramSets[i + 1][7],
                BetweenMarkers_data = paramSets[i + 1][8],
                Height_Correction_data = paramSets[i + 1][9],
                Reject_data = paramSets[i + 1][10],
                MarkerFreq_A_data = paramSets[i + 1][11]/*paramSets[i + 1][11]*/,
                MarkerFreq_B_data = paramSets[i + 1][12]/*paramSets[i + 1][12]*/, //分割できるようになったら追加
                ShutterSpeed_data = paramSets[i + 1][13],
                LeftPWM_data = paramSets[i + 1][14],
                RightPWM_data = paramSets[i + 1][15],
                Xcoord_data = paramSets[i + 1][16],
                Ycoord_data = paramSets[i + 1][17]
            });
            //記憶用に保存しているクラス部分
            RED_Para_List_Memory.Add(new RED_ParamGroup_Memory
            {
                ParamSet_Name_data_Memory = paramSets_Memory[i + 1][0],
                IsExploring_data_Memory = paramSets_Memory[i + 1][1],
                TransitTime_data_Memory = paramSets_Memory[i + 1][2],
                Mu_data_Memory = paramSets_Memory[i + 1][3],
                Sigma_data_Memory = paramSets_Memory[i + 1][4],
                Outer_Rth_data_Memory = paramSets_Memory[i + 1][5],
                Inner_Rth_data_Memory = paramSets_Memory[i + 1][6],
                Height_data_Memory = paramSets_Memory[i + 1][7],
                BetweenMarkers_data_Memory = paramSets_Memory[i + 1][8],
                Height_Correction_data_Memory = paramSets_Memory[i + 1][9],
                Reject_data_Memory = paramSets_Memory[i + 1][10],
                MarkerFreq_A_data_Memory = paramSets_Memory[i + 1][11]/*paramSets[i + 1][11]*/,
                MarkerFreq_B_data_Memory = paramSets_Memory[i + 1][12]/*paramSets[i + 1][12]*/, //分割できるようになったら追加
                ShutterSpeed_data_Memory = paramSets_Memory[i + 1][13],
                LeftPWM_data_Memory = paramSets_Memory[i + 1][14],
                RightPWM_data_Memory = paramSets_Memory[i + 1][15],
                Xcoord_data_Memory = paramSets_Memory[i + 1][16],
                Ycoord_data_Memory = paramSets_Memory[i + 1][17]
            });
        }
        //グループ化した結果を列挙しています。
        // リスト内の各 RED_ParamGroup インスタンスにアクセスしてプロパティを列挙
        //foreach (RED_ParamGroup paramGroup in RED_Para_List)
        //{
        //    Debug.Log($"ParamSet_Name: {paramGroup.ParamSet_Name}, Mu: {paramGroup.Mu}");
        //}
        //for(int u = 0; u < 5; u++)
        //{
        //    Debug.Log("ParamSet_Name: "+ RED_Para_List[u].Height_Correction_data);
        //}

        dropdown_paramset(RED_Para_List, paramSets.Count);
        initial_param_count = 1;

    }

    public void initial_param_view()
    {
        //for (int i = 0; i < 10; i++)
        //{
        //    children_Param[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = (i + 10).ToString();
        //}
        //Debug.Log("******"+ RED_Para_List[0].MarkerFreq_A_data + "*****"+ RED_Para_List[0].MarkerFreq_B_data + "****");

        if (RED_Para_List[0].IsExploring_data.Equals("TRUE"))
        {
            SwitchToggle_Algo();
            //Debug.Log("koko1");
        }
        else
        {
            Debug.Log("IsExplor False");
        }

        children_Param[0].transform.GetChild(0).gameObject.GetComponent<Text>().text = RED_Para_List[0].TransitTime_data;
        children_Param[1].transform.GetChild(0).gameObject.GetComponent<Text>().text = RED_Para_List[0].Inner_Rth_data;
        children_Param[2].transform.GetChild(0).gameObject.GetComponent<Text>().text = RED_Para_List[0].Mu_data;
        children_Param[3].transform.GetChild(0).gameObject.GetComponent<Text>().text = RED_Para_List[0].Outer_Rth_data;
        children_Param[4].transform.GetChild(0).gameObject.GetComponent<Text>().text = RED_Para_List[0].Height_data;
        children_Param[5].transform.GetChild(0).gameObject.GetComponent<Text>().text = RED_Para_List[0].BetweenMarkers_data;
        children_Param[6].transform.GetChild(0).gameObject.GetComponent<Text>().text = RED_Para_List[0].MarkerFreq_A_data;
        children_Param[7].transform.GetChild(0).gameObject.GetComponent<Text>().text = RED_Para_List[0].MarkerFreq_B_data;
        children_Param[8].transform.GetChild(0).gameObject.GetComponent<Text>().text = RED_Para_List[0].Xcoord_data;
        children_Param[9].transform.GetChild(0).gameObject.GetComponent<Text>().text = RED_Para_List[0].Ycoord_data;

        if (RED_Para_List[0].Height_Correction_data.Equals("TRUE"))
        {
            SwitchToggle_Speed();
            Debug.Log("koko1");
        }
        else
        {
            Debug.Log("Height Collection False");
        }

        ddtmp_Param_Shutter_Speed.captionText.text = RED_Para_List[0].ShutterSpeed_data;
        ddtmp_Param_Reject.captionText.text = RED_Para_List[0].Reject_data;

        //red_para.IsExploring_json = RED_Para_List[0].IsExploring;
        //red_para.TransitTime_json = RED_Para_List[0].TransitTime;
        //red_para.Mu_json = RED_Para_List[0].Mu;
        //red_para.Sigma_json = RED_Para_List[0].Sigma;
        //red_para.Outer_Rth_json = RED_Para_List[0].Outer_Rth;
        //red_para.Inner_Rth_json = RED_Para_List[0].Inner_Rth;
        //red_para.Height_json = RED_Para_List[0].Height;
        //red_para.BetweenMarkers_json = RED_Para_List[0].BetweenMarkers;
        //red_para.Height_Correction_json = RED_Para_List[0].Height_Correction;
        //red_para.Reject_json = RED_Para_List[0].Reject;
        //red_para.MarkerFreq_A_json = RED_Para_List[0].MarkerFreq_A;
        //red_para.MarkerFreq_B_json = RED_Para_List[0].MarkerFreq_B;
        //red_para.ShutterSpeed_json = RED_Para_List[0].ShutterSpeed;
        //red_para.LeftPWM_json = RED_Para_List[0].LeftPWM;
        //red_para.RightPWM_json = RED_Para_List[0].RightPWM;
        //red_para.Xcoord_json = RED_Para_List[0].Xcoord;
        //red_para.Ycoord_json = RED_Para_List[0].Ycoord;


        //// JSON形式に変換
        //send_data_Algo.send_json_param = JsonUtility.ToJson(red_para);

        //// 結果を表示
        //Debug.Log("@@@@@@@@@@@@@@@@@: " + red_para);
        //Debug.Log("@@@@@@@@@@@@@@@@@: " + red_para.IsExploring_json);
        //Debug.Log("@@@@@@@@@@@@@@@@@: " + send_data_Algo.send_json_param);

        //Debug.Log("---------------: ");

    }


    public void Marker_Freq_Clear()
    {
        input_MarkerFreq_A.text = ""; 
        input_MarkerFreq_B.text = "";
        RED_Para_List[ddtmp_Param.value].MarkerFreq_A_data = "";
        RED_Para_List[ddtmp_Param.value].MarkerFreq_B_data = "";
        dropdown_param_call(ddtmp_Param.value);

    }

    public void Data_Reflesh_Button()
    {
        intialize_inputfield();

        for (int a = 0; a < paramSets.Count - 1; a++)
        {
            RED_Para_List[a].ParamSet_Name_data = RED_Para_List_Memory[a].ParamSet_Name_data_Memory;
            RED_Para_List[a].IsExploring_data = RED_Para_List_Memory[a].IsExploring_data_Memory;
            RED_Para_List[a].TransitTime_data = RED_Para_List_Memory[a].TransitTime_data_Memory;
            RED_Para_List[a].Inner_Rth_data = RED_Para_List_Memory[a].Inner_Rth_data_Memory;
            RED_Para_List[a].Mu_data = RED_Para_List_Memory[a].Mu_data_Memory;
            RED_Para_List[a].Outer_Rth_data = RED_Para_List_Memory[a].Outer_Rth_data_Memory;
            RED_Para_List[a].Sigma_data = RED_Para_List_Memory[a].Sigma_data_Memory;
            RED_Para_List[a].Height_data = RED_Para_List_Memory[a].Height_data_Memory;
            RED_Para_List[a].Height_Correction_data = RED_Para_List_Memory[a].Height_Correction_data_Memory;
            RED_Para_List[a].Reject_data = RED_Para_List_Memory[a].Reject_data_Memory;
            RED_Para_List[a].MarkerFreq_A_data = RED_Para_List_Memory[a].MarkerFreq_A_data_Memory;
            RED_Para_List[a].MarkerFreq_B_data = RED_Para_List_Memory[a].MarkerFreq_B_data_Memory;
            RED_Para_List[a].BetweenMarkers_data = RED_Para_List_Memory[a].BetweenMarkers_data_Memory;
            RED_Para_List[a].ShutterSpeed_data = RED_Para_List_Memory[a].ShutterSpeed_data_Memory;
            RED_Para_List[a].LeftPWM_data = RED_Para_List_Memory[a].LeftPWM_data_Memory;
            RED_Para_List[a].RightPWM_data = RED_Para_List_Memory[a].RightPWM_data_Memory;
            RED_Para_List[a].Xcoord_data = RED_Para_List_Memory[a].Xcoord_data_Memory;
            RED_Para_List[a].Ycoord_data = RED_Para_List_Memory[a].Ycoord_data_Memory;
        }

        param_view(ddtmp_Param.value);
    }

    public void intialize_inputfield()
    {
        input_TransitTime.text = "";
        input_Inner.text = "";
        input_Mu.text = "";
        input_Outer.text = "";
        input_Height.text = "";
        input_BetweenMark.text = "";
        input_MarkerFreq_A.text = "";
        input_MarkerFreq_B.text = "";
        input_VirtualMarker_x.text = "";
        input_VirtualMarker_y.text = "";
    }
    ///////////////////  Algorithm Onの方の処理  //////////////////////////////////
    /// <summary>
    /// トグルのボタンアクションに設定しておく
    /// </summary>
    public void SwitchToggle_Algo()
    {
        if(Algo_count % 2 == 0)
        {
            RED_Para_List[ddtmp_Param.value].IsExploring_data = "TRUE";
            //Algo_mode = 1;
        }
        else
        {
            RED_Para_List[ddtmp_Param.value].IsExploring_data = "FALSE";
            //Algo_mode = 0;
        }
        Value_Algo = !Value_Algo;
        UpdateToggle_Algo(SWITCH_DURATION);
        Algo_count += 1;
    }
    /// <summary>
    /// 状態を反映させる
    /// </summary>
    private void UpdateToggle_Algo(float duration)
    {
        var bgColor = Value_Algo ? ON_BG_COLOR : OFF_BG_COLOR;
        var handleDestX = Value_Algo ? handlePosX_Algo : -handlePosX_Algo;

        sequence_Algo?.Complete();
        sequence_Algo = DOTween.Sequence();
        sequence_Algo.Append(backgroundImage_Algo.DOColor(bgColor, duration))
            .Join(handle_Algo.DOAnchorPosX(handleDestX, duration / 2));
    }
    //////////////////////////////////////////////////////////////////////////////


    ////////////////////////  Algorithm Onの方の処理  //////////////////////////////
    public void SwitchToggle_Speed()
    {
        if(true_false_judgement == true)
        {
            RED_Para_List[ddtmp_Param.value].Height_Correction_data = "FALSE";
            true_false_judgement = false;
        }
        else
        {
            RED_Para_List[ddtmp_Param.value].Height_Correction_data = "TRUE";
            true_false_judgement = true;
        }
        Value_Speed = !Value_Speed;
        UpdateToggle_Speed(SWITCH_DURATION);
        Debug.Log("koko2");
    }
    private void UpdateToggle_Speed(float duration)
    {
        var bgColor = Value_Speed ? ON_BG_COLOR : OFF_BG_COLOR;
        var handleDestX = Value_Speed ? handlePosX_Speed : -handlePosX_Speed;

        sequence_Speed?.Complete();
        sequence_Speed = DOTween.Sequence();
        sequence_Speed.Append(backgroundImage_Speed.DOColor(bgColor, duration))
            .Join(handle_Speed.DOAnchorPosX(handleDestX, duration / 2));
    }
    /////////////////////////////////////////////////////////////////////////////
}