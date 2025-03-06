using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using DG.Tweening;

public static class EntireData
{
    public static string select_RED_Num = null;
    //public static int GroupAll_reddata_param_count = 0;
    public static List<List<string>> Entire_RED_Data = new List<List<string>>();
    public static List<List<string>> GroupVisu_RED_Data = new List<List<string>>();
    public static bool Group_send_command;
    public static int Group_reddata_param_count = 0;
    public static string Entire_Group_Command = null;
}

public class CommandRelay_Trigger
{
    public static int stop_trigger = 0;
    public static int shutdown_trigger = 0;
    public static int Restart_trigger = 0;
    public static int Explor_trigger = 0;
}

 
public class EntireManagement : MonoBehaviour
{
    public GameObject Group_name_dropdown_List;
    Dropdown ddtmp_Group_name_List;

    int initial_gui_count_Entire = 0;
    int bbb = 0;

    ////////  IP Buttonを押したときにIPを獲得するもの  ////////////
    public GameObject IPButton_textObject;
    int push_count = 0;
    int push_forAll_count = 0;
    string IP_textValue;

    public Text text_selectRobot;

    Dropdown ddtmp; /*  */

    public GameObject RED_Dropdown;
    public GameObject Details_data;

    Transform[] children; // 子オブジェクト達を入れる配列

    //public Image image;
    private Sprite sprite;

    private List<GameObject> contentItems = new List<GameObject>(); // 生成されたコンテンツのリスト


    Transform[] children_Entire; // 子オブジェクト達を入れる配列

    //public Image image;
    private Sprite sprite_Entire;

    public GameObject contentPanel_Entire; // ScrollView内のコンテンツ用のパネル
    public GameObject contentPrefab_Entire; // 動的なコンテンツのプレハブ

    public GameObject EM_Entire; // Entire数を表示するテキスト
    public GameObject EM_Connect; //Connect数を表示するテキスト
    public GameObject EM_Disconnect; // Disconnect数を表示するテキスト
    public GameObject EM_CeilingImage; // CeilingImage数を表示するテキスト
    public GameObject EM_FloorImage; //FloorImage数を表示するテキスト
    public GameObject EM_FindCenter; //FindCenter率を表示するテキスト
    public GameObject EM_Accept; //Accept率を表示するテキスト
    public GameObject EM_Reject; //Reject率を表示するテキスト

    //Groupごとのデータを一括で受け取った時の受け取る変数
    List<string> Connection_Num_Entire_Data = new List<string>();
    List<string> Connection_Num_Group_Data = new List<string>();
    //String[] Connection_Num_Group_Data = new string[3];
    List<string> Freq_sum = new List<string>();
    int connect_count = 0;
    int first_check = 0;

    private GameObject item_Entire;

    private List<GameObject> contentItems_Entire = new List<GameObject>(); // 生成されたコンテンツのリスト

    [SerializeField] private Image Image_ALL_setting;
    [SerializeField] private RectTransform handle_ALL_setting;
    /// <summary>
    /// トグルの値
    /// </summary>
    [NonSerialized] public bool Value_ALL_setting;

    private float handlePosX_ALL_setting;
    private Sequence sequence_ALL_setting;

    private static readonly Color OFF_BG_COLOR = new Color(255.0f, 255.0f, 255.0f);
    private static readonly Color ON_BG_COLOR = new Color(0.2f, 0.84f, 0.3f);

    private const float SWITCH_DURATION = 0.36f;

    int kakuninn = 0;

    //EntireData EM_1 = new EntireData();
    // Start is called before the first frame update
    void Start()
    {
        ddtmp_Group_name_List = Group_name_dropdown_List.GetComponent<Dropdown>();

        handlePosX_ALL_setting = Mathf.Abs(handle_ALL_setting.anchoredPosition.x);
        UpdateToggle_ALL_setting(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Clone_RED_IP.IPButton_trigger == 1)
        {
            text_selectRobot.text = ""; //データの初期化
            for (int i = 0; i < Clone_RED_IP.Select_RED_IP_Entire.Count; i++)
            {
                text_selectRobot.text += Clone_RED_IP.Select_RED_IP_Entire[i];
                if(i != Clone_RED_IP.Select_RED_IP_Entire.Count - 1)
                {
                    text_selectRobot.text += Environment.NewLine;
                }
                
            }
            Clone_RED_IP.IPButton_trigger = 0;
        }
        //ALL選択時、表記が2重になる所改善
        //////////ここにFor Allの時のロボットを追加する//////////////////////
        if(push_forAll_count == 1 && title_data.RED_IP_title == "EntireRED")
        {
            Debug.Log("OKKKKKKK");
            for (int j = 0; j < EntireData.GroupVisu_RED_Data.Count; j++)//Group名が最初だから1から回している
            {
                text_selectRobot.text += EntireData.GroupVisu_RED_Data[j][0];
                if (j != EntireData.GroupVisu_RED_Data.Count - 1)
                {
                    text_selectRobot.text += Environment.NewLine;
                }
            }

            push_forAll_count = 0;
        }
        if(push_forAll_count  == 2 && title_data.RED_IP_title == "SelectRED")
        {
            text_selectRobot.text = "";
            push_forAll_count = 0;
        }

        //ここは確認のためのコードだから、通信するときはコメントアウト////////
        //if(screen_position.EntireScreen == 1 && kakuninn == 0)
        //{
        //    split_EntireRED_data("test");
        //    kakuninn = 1;
        //}
        ////////////////////////////////////////////////////////////

        if(tcpip_data.view_flag == 1)
        {
            DetailView_RED(tcpip_data.RED_data);
            tcpip_data.view_flag = 0;
        }
        if (tcpip_data.RED_num_flag == 1)
        {
            dropdown_button(tcpip_data.RED_data);
            tcpip_data.RED_num_flag = 0;
        }

        if (tcpip_data.receive_count == 1 && EntireData.Group_reddata_param_count == 1)
        {
            split_EntireRED_data(tcpip_data.RED_data);
            tcpip_data.receive_count = 0;
            EntireData.Group_reddata_param_count = 0;
        }

        if (RED_Group.RED_Group_Command.Equals(1))
        {
            //Debug.Log("//////////////////////////////////////////////");
            dropdown_Robotgroup(RED_Group.RED_Gr, RED_Group.RED_Gr.Count);

        }
    }

    // 新しいコンテンツを追加する関数(意味なしかも？？)
    public void AddContent(string contentText)
    {
        GameObject item = Instantiate(contentPrefab_Entire);
        item.transform.SetParent(contentPanel_Entire.transform, false);
        contentItems_Entire.Add(item);

        // 生成したアイテムのテキストや内容を設定
        Text itemText = item.GetComponentInChildren<Text>();
        itemText.text = contentText;
    }

    public void DetailView_RED(string data_all)
    {
        Details_data.GetComponent<Text>().text += null;

        string[] RED_data = data_all.Split(' ');
        string[] data_param = new string[RED_data.Length];

        Debug.Log("size: " + RED_data.Length);
        for (int i = 0; i < RED_data.Length; i++)
        {
            data_param[i] = RED_data[i];
            Debug.Log("aaa["+i+"]: " + data_param[i]);
        }

        Debug.Log("okkkkkkkkkk");
        Details_data.GetComponent<Text>().text += "Inner: " + data_param[0];
        Details_data.GetComponent<Text>().text += Environment.NewLine;
        Details_data.GetComponent<Text>().text += "Outer: " + data_param[1];
        Details_data.GetComponent<Text>().text += Environment.NewLine;
        Details_data.GetComponent<Text>().text += "Mu: " + data_param[2];
        Details_data.GetComponent<Text>().text += Environment.NewLine;
        Details_data.GetComponent<Text>().text += "Sigma: " + data_param[3];
        Details_data.GetComponent<Text>().text += Environment.NewLine;
        Details_data.GetComponent<Text>().text += "Freq: " + data_param[4];
        Details_data.GetComponent<Text>().text += Environment.NewLine;
        Details_data.GetComponent<Text>().text += "Step: " + data_param[5];
        Details_data.GetComponent<Text>().text += Environment.NewLine;
    }

    //////////////////  GUIにデータの可視化を行う部分　　////////////////
    public void EntireData_Visualize()
    {
        //エラーこのへん
        if (initial_gui_count_Entire > 0)
        {
            EM_Entire.GetComponent<Text>().text = "";
            EM_Connect.GetComponent<Text>().text = "";
            EM_Disconnect.GetComponent<Text>().text = "";
            EM_CeilingImage.GetComponent<Text>().text = "";
            EM_FloorImage.GetComponent<Text>().text = "";
            EM_FindCenter.GetComponent<Text>().text = "";
            EM_Accept.GetComponent<Text>().text = "";
            EM_Reject.GetComponent<Text>().text = "";
            foreach (var contentItem in contentItems)
            {
                Destroy(contentItem);
            }
            contentItems.Clear(); // リストもクリア
        }

        EM_Entire.GetComponent<Text>().text = Connection_Num_Group_Data[0];
        EM_Connect.GetComponent<Text>().text = Connection_Num_Group_Data[1];
        EM_Disconnect.GetComponent<Text>().text = Connection_Num_Group_Data[2];
        EM_CeilingImage.GetComponent<Text>().text = Connection_Num_Entire_Data[3];
        EM_FloorImage.GetComponent<Text>().text = Connection_Num_Entire_Data[4];
        EM_FindCenter.GetComponent<Text>().text = Connection_Num_Entire_Data[5];
        EM_Accept.GetComponent<Text>().text = Connection_Num_Entire_Data[6];
        EM_Reject.GetComponent<Text>().text = Connection_Num_Entire_Data[7];

        // 子オブジェクト達を入れる配列の初期化
        children = new Transform[contentPrefab_Entire.transform.childCount];
        for (int j = 0; j < contentPrefab_Entire.transform.childCount; j++)
        {
            children[j] = contentPrefab_Entire.transform.GetChild(j); // GetChild()で子オブジェクトを取得
        }
        // 生成したアイテムのテキストや内容を設定
        if (EntireData.GroupVisu_RED_Data[0][2] == "5")
        {
            sprite = Resources.Load<Sprite>("100");
            Image image_battery0 = children[0].GetComponent<Image>();
            image_battery0.sprite = sprite;
        }
        else if (EntireData.GroupVisu_RED_Data[0][2] == "warning")
        {
            sprite = Resources.Load<Sprite>("10");
            Image image_battery0 = children[0].GetComponent<Image>();
            image_battery0.sprite = sprite;
        }

        // 生成したアイテムのテキストや内容を設定
        Text itemText_IP0 = children[1].GetComponentInChildren<Text>();
        itemText_IP0.text = EntireData.GroupVisu_RED_Data[0][0];

        if (EntireData.GroupVisu_RED_Data[0][3] == "1")
        {
            sprite = Resources.Load<Sprite>("ceil_icon");
            Image image_ceil0 = children[2].GetComponent<Image>();
            image_ceil0.sprite = sprite;
        }
        else
        {
            sprite = Resources.Load<Sprite>("not_ceil_icon");
            Image image_ceil0 = children[2].GetComponent<Image>();
            image_ceil0.sprite = sprite;
        }

        if (EntireData.GroupVisu_RED_Data[0][4] == "1")
        {
            sprite = Resources.Load<Sprite>("floor_icon");
            Image image_floor0 = children[3].GetComponent<Image>();
            image_floor0.sprite = sprite;
        }
        else
        {
            sprite = Resources.Load<Sprite>("not_floor_icon");
            Image image_floor0 = children[3].GetComponent<Image>();
            image_floor0.sprite = sprite;
        }

        if (EntireData.GroupVisu_RED_Data[0][5] == "1")
        {
            sprite = Resources.Load<Sprite>("Find_icon");
            Image image_Find0 = children[4].GetComponent<Image>();
            image_Find0.sprite = sprite;
        }
        else
        {
            sprite = Resources.Load<Sprite>("Lost_icon");
            Image image_Find0 = children[4].GetComponent<Image>();
            image_Find0.sprite = sprite;
        }

        Text itemText_Inner0 = children[5].GetComponentInChildren<Text>();
        itemText_Inner0.text = EntireData.GroupVisu_RED_Data[0][7];

        Text itemText_Mu0 = children[6].GetComponentInChildren<Text>();
        itemText_Mu0.text = EntireData.GroupVisu_RED_Data[0][8];


        Text itemText_Sigma0 = children[7].GetComponentInChildren<Text>();
        itemText_Sigma0.text = EntireData.GroupVisu_RED_Data[0][9];

        Text itemText_Outer0 = children[8].GetComponentInChildren<Text>();
        itemText_Outer0.text = EntireData.GroupVisu_RED_Data[0][10];

        Text itemText_Freq0 = children[9].GetComponentInChildren<Text>();
        itemText_Freq0.text = EntireData.GroupVisu_RED_Data[0][11];

        Text itemText_Step0 = children[10].GetComponentInChildren<Text>();
        itemText_Step0.text = EntireData.GroupVisu_RED_Data[0][6];

        if (EntireData.GroupVisu_RED_Data[0][1] == "0")
        {
            sprite = Resources.Load<Sprite>("Disconnect");
            Image image_Connection0 = children[11].GetComponent<Image>();
            image_Connection0.sprite = sprite;
        }
        else if (EntireData.GroupVisu_RED_Data[0][1] == "1")
        {
            sprite = Resources.Load<Sprite>("Connect");
            Image image_Connection0 = children[11].GetComponent<Image>();
            image_Connection0.sprite = sprite;
        }
        else if (EntireData.GroupVisu_RED_Data[0][1] == "2")
        {
            sprite = Resources.Load<Sprite>("Wait");
            Image image_Connection0 = children[11].GetComponent<Image>();
            image_Connection0.sprite = sprite;
        }

        // ここで動的なコンテンツを生成し、ScrollView内に配置
        for (int i = 1; i < EntireData.GroupVisu_RED_Data.Count; i++) // 例として20個のアイテムを生成
        {
           // Debug.Log("[[[[[[[[[[[[[[[[[[[[[[[[[[[[: " + i);
            GameObject item = Instantiate(contentPrefab_Entire);
            item.transform.SetParent(contentPanel_Entire.transform, false);
            contentItems.Add(item);
            //Debug.Log("IP address[" + i + "]: " + Entire_RED_Data[i][0]);
            // 子オブジェクト達を入れる配列の初期化
            children = new Transform[item.transform.childCount];

            for (int j = 0; j < item.transform.childCount; j++)
            {
                children[j] = item.transform.GetChild(j); // GetChild()で子オブジェクトを取得
                //Debug.Log($"検索方法１： {j} 番目の子供は {children[j].name} です");
            }

            if (EntireData.GroupVisu_RED_Data[i][2].Equals("5"))
            {
                //Debug.Log("Acessssssssssssssss");
                sprite = Resources.Load<Sprite>("100");
                Image image_battery = children[0].GetComponent<Image>();
                image_battery.sprite = sprite;
            }
            else if (EntireData.GroupVisu_RED_Data[i][2].Equals("warning"))
            {
                //Debug.Log("kocchi Acesssssssssssssss");
                sprite = Resources.Load<Sprite>("10");
                Image image_battery = children[0].GetComponent<Image>();
                image_battery.sprite = sprite;
            }

            // 生成したアイテムのテキストや内容を設定
            Text itemText = children[1].GetComponentInChildren<Text>();
            itemText.text = EntireData.GroupVisu_RED_Data[i][0];

            if (EntireData.GroupVisu_RED_Data[i][3] == "1")
            {
                sprite = Resources.Load<Sprite>("ceil_icon");
                Image image_ceil = children[2].GetComponent<Image>();
                image_ceil.sprite = sprite;
            }
            else
            {
                sprite = Resources.Load<Sprite>("not_ceil_icon");
                Image image_ceil = children[2].GetComponent<Image>();
                image_ceil.sprite = sprite;
            }

            if (EntireData.GroupVisu_RED_Data[i][4] == "1")
            {
                sprite = Resources.Load<Sprite>("floor_icon");
                Image image_floor = children[3].GetComponent<Image>();
                image_floor.sprite = sprite;
            }
            else
            {
                sprite = Resources.Load<Sprite>("not_floor_icon");
                Image image_floor = children[3].GetComponent<Image>();
                image_floor.sprite = sprite;
            }

            if (EntireData.GroupVisu_RED_Data[i][5] == "1")
            {
                sprite = Resources.Load<Sprite>("Find_icon");
                Image image_Find = children[4].GetComponent<Image>();
                image_Find.sprite = sprite;
            }
            else
            {
                sprite = Resources.Load<Sprite>("Lost_icon");
                Image image_Find = children[4].GetComponent<Image>();
                image_Find.sprite = sprite;
            }

            Text itemText_Inner = children[5].GetComponentInChildren<Text>();
            itemText_Inner.text = EntireData.GroupVisu_RED_Data[i][7];

            Text itemText_Mu = children[6].GetComponentInChildren<Text>();
            itemText_Mu.text = EntireData.GroupVisu_RED_Data[i][8];


            Text itemText_Sigma = children[7].GetComponentInChildren<Text>();
            itemText_Sigma.text = EntireData.GroupVisu_RED_Data[i][9];

            Text itemText_Outer = children[8].GetComponentInChildren<Text>();
            itemText_Outer.text = EntireData.GroupVisu_RED_Data[i][10];

            Text itemText_Freq = children[9].GetComponentInChildren<Text>();
            itemText_Freq.text = EntireData.GroupVisu_RED_Data[i][11];

            Text itemText_Step = children[10].GetComponentInChildren<Text>();
            itemText_Step.text = EntireData.GroupVisu_RED_Data[i][6];

            if (EntireData.GroupVisu_RED_Data[i][1] == "0")
            {
                sprite = Resources.Load<Sprite>("Disconnect");
                Image image_Connection = children[11].GetComponent<Image>();
                image_Connection.sprite = sprite;
            }
            else if (EntireData.GroupVisu_RED_Data[i][1] == "1")
            {
                sprite = Resources.Load<Sprite>("Connect");
                Image image_Connection = children[11].GetComponent<Image>();
                image_Connection.sprite = sprite;
            }
            else if (EntireData.GroupVisu_RED_Data[i][1] == "2")
            {
                sprite = Resources.Load<Sprite>("Wait");
                Image image_Connection = children[11].GetComponent<Image>();
                image_Connection.sprite = sprite;
            }
        }

        //remember_Group_Num = Entire_RED_Data.Count;
        initial_gui_count_Entire += 1;
    }

    //多分いらない
    public void check_RED_UI()
    {
        // 子オブジェクト達を入れる配列の初期化
        children = new Transform[contentPrefab_Entire.transform.childCount];
        for (int j = 0; j < contentPrefab_Entire.transform.childCount; j++)
        {
            children[j] = contentPrefab_Entire.transform.GetChild(j); // GetChild()で子オブジェクトを取得
        }

        if (EntireData.GroupVisu_RED_Data[0][5] == "1")
        {
            sprite = Resources.Load<Sprite>("Find_icon");
            Image image_Find0 = children[4].GetComponent<Image>();
            image_Find0.sprite = sprite;
        }
        else
        {
            sprite = Resources.Load<Sprite>("Lost_icon");
            Image image_Find0 = children[4].GetComponent<Image>();
            image_Find0.sprite = sprite;
        }

        // ここで動的なコンテンツを生成し、ScrollView内に配置
        for (int i = 0; i < EntireData.GroupVisu_RED_Data.Count; i++) // 例として20個のアイテムを生成
        {
            // Debug.Log("[[[[[[[[[[[[[[[[[[[[[[[[[[[[: " + i);
            GameObject item = Instantiate(contentPrefab_Entire);
            item.transform.SetParent(contentPanel_Entire.transform, false);
            contentItems.Add(item);
            //Debug.Log("IP address[" + i + "]: " + Entire_RED_Data[i][0]);
            // 子オブジェクト達を入れる配列の初期化
            children = new Transform[item.transform.childCount];

            for (int j = 0; j < item.transform.childCount; j++)
            {
                children[j] = item.transform.GetChild(j); // GetChild()で子オブジェクトを取得
                //Debug.Log($"検索方法１： {j} 番目の子供は {children[j].name} です");
            }
        }

    }

    ////////////  Groupごとのデータを一括で受け取った時それを分割する関数  ///////////////////
    public void split_EntireRED_data(string data_processor)
    {
        if (bbb == 1)
        {
            connect_count = 0;
            //Debug.Log("Before Clear: " + Group_RED_Data[0][0]);
            Connection_Num_Entire_Data.Clear();
            EntireData.Entire_RED_Data.Clear();
            EntireData.GroupVisu_RED_Data.Clear();
            //Debug.Log("After Clear: " + Group_RED_Data[0][0]);
        }

        //data_processor = "123/12/312\r\n312,312,321,423,54,65,546,36\r\n312,312,321,423,54,65,546,333";
        //data_processorは確認のために以下で値を与えているから、通信するときコメントアウト
        //data_processor = "2/2/0/0/0/0/0/0\r\n192.168.1.198,1,5,1,1,0,0,0.0,0.0,0.0,0.0,7\r\n192.168.1.210,0,warning,1,1,0,1,0.0,0.0,0.0,0.0,7_11\r\n";
        string[] del = { "\r\n" };
        string[] data_size = data_processor.Split(del, StringSplitOptions.None);
        Debug.Log("-^-^-^-^-^-^-^-^-^-^-^-^-^" + data_size.Length);
        for (int i = 0; i < data_size.Length - 1; i++)
        {
            if (i == 0)
            {
                Debug.Log("kkkooo1");
                string[] split_details = data_size[i].Split('/');
                for (int y = 0; y < split_details.Length; y++)
                {
                    Connection_Num_Entire_Data.Add(split_details[y]);
                    Connection_Num_Group_Data.Add(split_details[y]);
                }
            }
            else
            {
                List<string> details = new List<string>();
                List<string> Group_details = new List<string>();
                string[] split_details = data_size[i].Split(',');
                for (int j = 0; j < RED_Group.RED_Gr[ddtmp_Group_name_List.value - 1].Count; j++)
                {
                    if (split_details[0] == RED_Group.RED_Gr[ddtmp_Group_name_List.value - 1][j])
                    {
                        Debug.Log("IP_before: " + split_details[0]);
                        for (int k = 0; k < split_details.Length; k++)
                        {
                            //Debug.Log("IP_after: " + split_details[0]);
                            Group_details.Add(split_details[k]);
                            /*
                            details[0]: IPアドレス
                            details[1]: Connected
                            details[2]: バッテリー状態
                            details[3]: Camera ceil on/off
                            details[4]: Camera floor on/off
                            details[5]: marker_seen
                            details[6]: Current Step Num
                            details[7]: innner
                            details[8]: mu
                            details[9]: sigma
                            details[10]: outer
                            details[11]: freq(5_11, 5 etc)
                             */
                        }
                        if (split_details[1] == "1" || split_details[1] == "2" && first_check == 0)
                        {

                            connect_count += 1;
                            //Debug.Log("check: " + connect_count + " IP: " + split_details[0]);
                            first_check = 1;
                        }
                        EntireData.GroupVisu_RED_Data.Add(Group_details);
                        first_check = 0;
                    }
                }
                for (int j = 0; j < split_details.Length; j++)
                {
                    details.Add(split_details[j]);
                    /*
                    details[0]: IPアドレスkoko
                    details[1]: Connected
                    details[2]: バッテリー状態
                    details[3]: Camera ceil on/off
                    details[4]: Camera floor on/off
                    details[5]: marker_seen
                    details[6]: Current Step Num
                    details[7]: innner
                    details[8]: mu
                    details[9]: sigma
                    details[10]: outer
                    details[11]: freq(5_11, 5 etc)
                     */
                }
                EntireData.Entire_RED_Data.Add(details);
            }

        }

        Connection_Num_Group_Data[0] = EntireData.GroupVisu_RED_Data.Count.ToString();
        Connection_Num_Group_Data[1] = connect_count.ToString();
        Connection_Num_Group_Data[2] = (EntireData.GroupVisu_RED_Data.Count - connect_count).ToString();
        bbb = 1;

        //Debug.Log("+-+-++-+=+=;=;=;=;=:  " + EntireData.Entire_RED_Data.Count);
        for(int i=0; i < EntireData.GroupVisu_RED_Data.Count; i++)
        {
            Debug.Log("Group Data: "+ EntireData.GroupVisu_RED_Data[i][0]);
        }
        EntireData_Visualize();

    }
    ////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////   ドロップダウンでロボットグループを獲得する所
    public void dropdown_Robotgroup(List<List<string>> data_redgroup, int data_group_size)
    {
        List<string> REDGrList = new List<string>();

        REDGrList.Add("Select RED Group");
        for (int j = 0; j < data_group_size; j++)
        {
            //Optionsに表示する文字列をリストに追加
            REDGrList.Add(data_redgroup[j][0]);
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp_Group_name_List = Group_name_dropdown_List.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_name_List.ClearOptions();

        //リストを追加
        ddtmp_Group_name_List.AddOptions(REDGrList);
    }

    public void dropdown_group_List_call(int num)
    {
        Debug.Log("++++" + ddtmp_Group_name_List.value);
        Debug.Log(ddtmp_Group_name_List.options[ddtmp_Group_name_List.value].text);

        //EntireData.Entire_Group_Command = ddtmp_Group_name_List.options[ddtmp_Group_name_List.value].text;
        EntireData.Group_reddata_param_count = 1;
        EntireData.Group_send_command = true;
        //dropdown_paramset(paramSets, ddtmp_Param.value);
        //GroupData.Freq_Group_Command = ddtmp_FreqList.options[ddtmp_FreqList.value].text;
        //GroupData.freq_reddata_param_count = 1;
        //GroupData.Freq_send_command = true;
        //dropdown_redgroupIP(RED_Group.RED_Gr, ddtmp_FreqList.value);

    }

    public void dropdown_button(string data_RED_num)
    {
        List<string> REDlist = new List<string>();
        string[] RED_num = data_RED_num.Split(' ');

        for (int j = 0; j < RED_num.Length; j++)
        {
            //Optionsに表示する文字列をリストに追加
            REDlist.Add(RED_num[j]);
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp = RED_Dropdown.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp.ClearOptions();

        //リストを追加
        ddtmp.AddOptions(REDlist);
    }

    public void check_button()
    {

        //DetailView_RED("213 231 23 213 213 234");
        //DropDownコンポーネントのoptionsから選択されてているvalueをindexで指定し、
        //選択されている文字を樹徳
        EntireData.select_RED_Num = ddtmp.options[ddtmp.value].text;

        //ログに出力
        Debug.Log(EntireData.select_RED_Num);
    }

    public void Stop_Button()
    {
        CommandRelay_Trigger.stop_trigger = 1;
        Debug.Log("Stop Button");
    }

    public void StartExploration_Button()
    {
        CommandRelay_Trigger.Explor_trigger = 1;
        Debug.Log("Start Exploration Button");
    }

    public void Restart_Button()
    {
        CommandRelay_Trigger.Restart_trigger = 1;
        Debug.Log("Restart Button");

    }

    public void Shutdown_Button()
    {
        CommandRelay_Trigger.shutdown_trigger = 1;
        Debug.Log("Shutdown Button");
    }

    /// <summary>
    /// トグルのボタンアクションに設定しておく
    /// </summary>
    public void SwitchToggle_ALL_setting()
    {
        if(push_count % 2 == 0)
        {
            title_data.RED_IP_title = "EntireRED";
            //for (int i = 0; i < RED_Group.RED_Gr.Count; i++)
            //{
            //    Debug.Log("Param Name: " + RED_Group.RED_Gr[i][0]);
            //    if (RED_Group.RED_Gr[i][0] == "ALL")
            //    {
            //        title_data.RED_IP_title = "ALL RED";
            //        RED_Group.RED_Group_num = i;
            //    }
            //}
            push_forAll_count = 1;
        }
        else
        {
            title_data.RED_IP_title = "SelectRED";
            Debug.Log("ppp");
            push_forAll_count = 2;
        }

        push_count += 1;

        Value_ALL_setting = !Value_ALL_setting;
        UpdateToggle_ALL_setting(SWITCH_DURATION);

    }
    /// <summary>
    /// 状態を反映させる
    /// </summary>
    /// 同じロボットが表示されるエラーの原因
    private void UpdateToggle_ALL_setting(float duration)
    {
        var bgColor = Value_ALL_setting ? ON_BG_COLOR : OFF_BG_COLOR;
        var handleDestX = Value_ALL_setting ? handlePosX_ALL_setting : -handlePosX_ALL_setting;

        sequence_ALL_setting?.Complete();
        sequence_ALL_setting = DOTween.Sequence();
        sequence_ALL_setting.Append(Image_ALL_setting.DOColor(bgColor, duration))
            .Join(handle_ALL_setting.DOAnchorPosX(handleDestX, duration / 2));
    }
    /////////////////////////////////////////////////////////////////////
}
