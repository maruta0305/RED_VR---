using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RM
{
    public static bool Forward_trigger = false;
    public static bool Right_trigger = false;
    public static bool Left_trigger = false;
    public static bool Stop_trigger = false;
    public static bool TurnRight_trigger = false;
    public static bool TurnLeft_trigger = false;
    public static int one_times = 0;
    public static string Radicon_RED_IP = null;
    public static int Radi_RED_Group_num;
}

public class RadiconMode : MonoBehaviour
{

    public GameObject Group_name_dropdown_Radi;
    public GameObject Group_IP_dropdown_Radi;
    Dropdown ddtmp_Group_name_Radi;
    Dropdown ddtmp_Group_Ip_Radi;

    // Start is called before the first frame update
    void Start()
    {
        ddtmp_Group_name_Radi = Group_name_dropdown_Radi.GetComponent<Dropdown>();
        ddtmp_Group_Ip_Radi = Group_IP_dropdown_Radi.GetComponent<Dropdown>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("RED_Group.RED_Group_Command: " + RED_Group.RED_Group_Command);

        if (RED_Group.RED_Group_Command.Equals(1))
        {
            //Debug.Log("//////////////////////////////////////////////");
            dropdown_redgroup_Radi(RED_Group.RED_Gr, RED_Group.RED_Gr.Count);
            dropdown_redgroupIP_nothing_Radi();
            RED_Group.RED_Group_Command = 0;
        }
    }

    public void dropdown_redgroup_Radi(List<List<string>> data_redgroup, int data_group_size)
    {
        List<string> REDGrList = new List<string>();

        REDGrList.Add("Select RED Group");
        for (int j = 0; j < data_group_size; j++)
        {
            //Optionsに表示する文字列をリストに追加
            REDGrList.Add(data_redgroup[j][0]);
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp_Group_name_Radi = Group_name_dropdown_Radi.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_name_Radi.ClearOptions();

        //リストを追加
        ddtmp_Group_name_Radi.AddOptions(REDGrList);
    }

    public void dropdown_Group_call_Radi(int num)
    {
        if (ddtmp_Group_name_Radi.value == 0)
        {
            Debug.Log("Skip");
            dropdown_redgroupIP_nothing_Radi();
        }
        else
        {
            Debug.Log("koko11111");
            Debug.Log("++++" + ddtmp_Group_name_Radi.value);
            Debug.Log(ddtmp_Group_name_Radi.options[ddtmp_Group_name_Radi.value].text);
            dropdown_redgroupIP_Radi(RED_Group.RED_Gr, ddtmp_Group_name_Radi.value - 1);
            RM.Radi_RED_Group_num = ddtmp_Group_name_Radi.value - 1;
        }

    }
    public void dropdown_redgroupIP_Radi(List<List<string>> data_redgroupIP, int data_group_num)
    {
        List<string> REDGrIP_List = new List<string>();

        for (int j = 0; j < data_redgroupIP[data_group_num].Count + 1; j++)
        {
            if (j == 0)
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
        ddtmp_Group_Ip_Radi = Group_IP_dropdown_Radi.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_Ip_Radi.ClearOptions();

        //リストを追加
        ddtmp_Group_Ip_Radi.AddOptions(REDGrIP_List);
    }

    public void dropdown_redgroupIP_nothing_Radi()
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
        ddtmp_Group_Ip_Radi = Group_IP_dropdown_Radi.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_Ip_Radi.ClearOptions();

        //リストを追加
        ddtmp_Group_Ip_Radi.AddOptions(REDGrIP_List);
    }

    public void dropdown_RobotIP_call_Radi(int num)
    {
        Debug.Log(ddtmp_Group_Ip_Radi.options[ddtmp_Group_Ip_Radi.value].text);
        RM.Radicon_RED_IP = ddtmp_Group_Ip_Radi.options[ddtmp_Group_Ip_Radi.value].text;
        tcpip_data.ip_addr = ddtmp_Group_Ip_Radi.options[ddtmp_Group_Ip_Radi.value].text;
    }

    public void Forward_button_push()
    {
        RM.Forward_trigger = true;
    }

    public void Forward_button_leave()
    {
        RM.Forward_trigger = false;
        RM.one_times = 0;
    }

    public void Right_button_push()
    {
        RM.Right_trigger = true;
    }

    public void Right_button_leave()
    {
        RM.Right_trigger = false;
        RM.one_times = 0;
    }

    public void Left_button_push()
    {
        RM.Left_trigger = true;
    }

    public void Left_button_leave()
    {
        RM.Left_trigger = false;
        RM.one_times = 0;
    }

    public void Stop_button_push()
    {
        RM.Stop_trigger = true;
    }

    public void Stop_button_leave()
    {
        RM.Stop_trigger = false;
        RM.one_times = 0;
    }

    public void TurnRight_button_push()
    {
        RM.TurnRight_trigger = true;
    }

    public void TurnRight_button_leave()
    {
        RM.TurnRight_trigger = false;
        RM.one_times = 0;
    }

    public void TurnLeft_button_push()
    {
        RM.TurnLeft_trigger = true;
    }

    public void TurnLeft_button_leave()
    {
        RM.TurnLeft_trigger = false;
        RM.one_times = 0;
    }
}
