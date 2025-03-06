using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class title_data
{
    public static string RED_IP_title = null;
    public static int broker_ip_command = 0;
    public static string Explore_Tag = null;
}

public class Title : MonoBehaviour
{
    public GameObject Group_name_dropdown_title;
    public GameObject Select_RobotGroup_Dropdown_title;
    public GameObject Group_IP_dropdown_title;
    public GameObject Explore_Tag_Obj;
    InputField input_Explore_Tag;
    Dropdown ddtmp_Group_name_title;
    Dropdown ddtmp_Group_Ip_title;

    [SerializeField] private Image Server_Connect;
    [SerializeField] private RectTransform handle_Server;
    [SerializeField] private Image Broker_Connect;
    [SerializeField] private RectTransform handle_Broker;
    /// <summary>
    /// トグルの値
    /// </summary>
    [NonSerialized] public bool Value_Server;
    [NonSerialized] public bool Value_Broker;

    private float handlePosX_Server;
    private Sequence sequence_Server;
    private float handlePosX_Broker;
    private Sequence sequence_Broker;

    private static readonly Color OFF_BG_COLOR = new Color(255.0f, 255.0f, 255.0f);
    private static readonly Color ON_BG_COLOR = new Color(0.2f, 0.84f, 0.3f);

    private const float SWITCH_DURATION = 0.36f;

    // Start is called before the first frame update
    void Start()
    {
        input_Explore_Tag = Explore_Tag_Obj.GetComponent<InputField>();
        title_data.Explore_Tag = input_Explore_Tag.text;
        Debug.Log("Tag: " + title_data.Explore_Tag);
        handlePosX_Server = Mathf.Abs(handle_Server.anchoredPosition.x);
        UpdateToggle_Server(0);
        handlePosX_Broker = Mathf.Abs(handle_Broker.anchoredPosition.x);
        UpdateToggle_Broker(0);
    }

    // Update is called once per frame65
    //
    void Update()
    {
        if(input_Explore_Tag.text != title_data.Explore_Tag)
        {
            title_data.Explore_Tag = input_Explore_Tag.text;
            Debug.Log("Tag: " + title_data.Explore_Tag);
        }
        
        if (tcpip_data.server_connect_command == 1)
        {
            SwitchToggle_Server();
            tcpip_data.server_connect_command = 0;
        }
        if(tcpip_data.broker_connect_command == 1)
        {
            Debug.Log("togglebroker" );

            SwitchToggle_Broker();
            tcpip_data.broker_connect_command = 0;
        }

        if (RED_Group.RED_Group_Command.Equals(1))
        {
            //Debug.Log("//////////////////////////////////////////////");
            dropdown_redgroup_title(RED_Group.RED_Gr, RED_Group.RED_Gr.Count);
            dropdown_redgroupIP_nothing_title();
            //RED_Group.RED_Group_Command = 0;
        }
    }

    public void dropdown_redgroup_title(List<List<string>> data_redgroup, int data_group_size)
    {
        List<string> REDGrList = new List<string>();

        REDGrList.Add("Select RED Group");
        for (int j = 0; j < data_group_size; j++)
        {
            //Optionsに表示する文字列をリストに追加
            REDGrList.Add(data_redgroup[j][0]);
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
         ddtmp_Group_name_title = Group_name_dropdown_title.GetComponent<Dropdown>();
        //        ddtmp_Group_name_title = Select_RobotGroup_Dropdown_title.GetComponent<Dropdown>();

        Debug.Log("dropdown select");

//        ddtmp_Group_name_title = GameObject.Find("Select_RobotGroup_Dropdown_title").GetComponent<Dropdown>();

        //Select_RobotGroup_Dropdown_title

        //一度すべてのOptionsをクリア
        ddtmp_Group_name_title.ClearOptions();

        //リストを追加
        ddtmp_Group_name_title.AddOptions(REDGrList);
    }

    public void dropdown_redgroupIP_title(List<List<string>> data_redgroupIP, int data_group_num)
    {
        List<string> REDGrIP_List = new List<string>();

        for (int j = 0; j < data_redgroupIP[data_group_num].Count + 1; j++)
        {
            if (j == 0)
            {
                //Optionsに表示する文字列をリストに追加
                REDGrIP_List.Add("Select RED Robots");
            }else if (j == data_redgroupIP[data_group_num].Count)
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
        ddtmp_Group_Ip_title = Group_IP_dropdown_title.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_Ip_title.ClearOptions();

        //リストを追加
        ddtmp_Group_Ip_title.AddOptions(REDGrIP_List);
    }

    public void dropdown_redgroupIP_nothing_title()
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
        ddtmp_Group_Ip_title = Group_IP_dropdown_title.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_Ip_title.ClearOptions();

        //リストを追加
        ddtmp_Group_Ip_title.AddOptions(REDGrIP_List);
    }

    public void dropdown_RobotIP_call(int num)
    {
        Debug.Log(ddtmp_Group_Ip_title.options[ddtmp_Group_Ip_title.value].text);
        title_data.RED_IP_title = ddtmp_Group_Ip_title.options[ddtmp_Group_Ip_title.value].text;
        tcpip_data.ip_addr = ddtmp_Group_Ip_title.options[ddtmp_Group_Ip_title.value].text;
    }

    public void dropdown_Group_call(int num)
    {
        
        if (ddtmp_Group_name_title.value == 0)
        {
            Debug.Log("Skip");
            dropdown_redgroupIP_nothing_title();
        }
        else
        {
            RED_Group.RED_Group_num = ddtmp_Group_name_title.value - 1;
            Debug.Log("++++" + RED_Group.RED_Group_num);
            Debug.Log(ddtmp_Group_name_title.options[ddtmp_Group_name_title.value].text);
            dropdown_redgroupIP_title(RED_Group.RED_Gr, ddtmp_Group_name_title.value - 1);
        }

    }

    ///////////////  Algorithm Onの方の処理  /////////////////////////
    /// <summary>
    /// トグルのボタンアクションに設定しておく
    /// </summary>
    public void SwitchToggle_Server()
    {
        Value_Server = !Value_Server;
        UpdateToggle_Server(SWITCH_DURATION);

    }
    /// <summary>
    /// 状態を反映させる
    /// </summary>
    private void UpdateToggle_Server(float duration)
    {
        var bgColor = Value_Server ? ON_BG_COLOR : OFF_BG_COLOR;
        var handleDestX = Value_Server ? handlePosX_Server : -handlePosX_Server;

        sequence_Server?.Complete();
        sequence_Server = DOTween.Sequence();
        sequence_Server.Append(Server_Connect.DOColor(bgColor, duration))
            .Join(handle_Server.DOAnchorPosX(handleDestX, duration / 2));
    }
    /////////////////////////////////////////////////////////////////////


    ///////////////  Algorithm Onの方の処理  //////////////////////////////
    public void SwitchToggle_Broker()
    {
        Value_Broker = !Value_Broker;
        UpdateToggle_Broker(SWITCH_DURATION);
        Debug.Log("koko2");
    }
    private void UpdateToggle_Broker(float duration)
    {
        var bgColor = Value_Broker ? ON_BG_COLOR : OFF_BG_COLOR;
        var handleDestX = Value_Broker ? handlePosX_Broker : -handlePosX_Broker;

        sequence_Broker?.Complete();
        sequence_Broker = DOTween.Sequence();
        sequence_Broker.Append(Broker_Connect.DOColor(bgColor, duration))
            .Join(handle_Broker.DOAnchorPosX(handleDestX, duration / 2));
    }
    /////////////////////////////////////////////////////////////////////
}
