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


public class tcpip_data
{
    public static string RED_data = null;
    public static string RED_data_cmd = null;
    public static int view_flag = 0;
    public static int RED_num_flag = 0;
    public static string ip_addr = null;
    public static int server_connect_command = 0;
    public static int broker_connect_command = 0;

    public static int receive_count = 0;
}


public class TCPIP_NEW : MonoBehaviour
{
    int check = 0;
    int first_access = 1;
    string send_broker_everyrob = null;

    int destroy_count = 0;

    //インスタンス化
    //EntireData EM = new EntireData();
    //RM rm = new RM();
    //EntireManagement EM = new EntireManagement();

    /// <RadiconModeのデータを送信する変数>
    //private string send_forward = null;
    private string send_Right = null;
    private string send_Left = null;
    private string send_Stop = null;
    private string send_TurnRight = null;
    private string send_TurnLeft = null;
    /// 

    //TCPIP
    private TcpClient m_tcpClient;
    private NetworkStream m_networkStream;
    private TcpClient m_tcpClient_RED_data;
    private NetworkStream m_networkStream_RED_data;
    //private NetworkStream m_networkStream_RED_data_1;
    private bool m_isConnection;//接続確認フラグ    public int accessedCounter = 0;
                                //InputFieldを格納するための変数
    InputField input_ipaddress;
    InputField input_brokeripaddress;
    int Port_CR = 40001;
    int Port_RED_data = 40000;

    public GameObject ipaddress_input;
    public GameObject brokeripaddress_input;
    public string receiveText;
    public string receiveMessage = null;
    string replystr_CR = null;
    public string receiveMessage_RED_data = null;
    private string m_message = string.Empty; // サーバから受信した文字列
    private string m_message0 = "0";
    private string m_message1 = "1"; // サーバに送信する文字列(Yes)
    private string m_message2 = "2"; // サーバに送信する文字列(No)
    private string m_message3 = "3"; // サーバから受信する文字列(Yes, No)
    private string m_message4 = "4";
    string send_RED_Num = null;

    string cmd_to_send = "tekitou";
    private string paramset_STOP_str = "tekitou";
    private string paramset_STOP = "tekitou";
    private string paramset_str = "tekitou";

    int i = 0;

    int send_count = 100;

    // Start is called before the first frame update
    void Start()
    {
        //InputFieldコンポーネントを取得
        input_ipaddress = ipaddress_input.GetComponent<InputField>();
        input_brokeripaddress = brokeripaddress_input.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {

//        Debug.Log("TCP update -" + ImageData.ImageBaseURL + "- tcp:" + tcpip_data.server_connect_command.ToString());

        ///////////////////////// ロボットのImageの指令を送信する部分　///////////////////////////////////
        if (ImageData.ImageGetBaseURL_Trigger == 1)
        {
            send_cmd_to_dataproc("IMGURLBASE\r\n");
            Debug.Log("tcpip: imgurlbase");
        }
        if (ImageData.ImageRobotList_Trigger == 1)
        {
            send_cmd_to_dataproc("IMGROBOTLIST\r\n");
            Debug.Log("tcpip: imgrobotlist");
            //            ImageData.ImageRobotList_Trigger = 0;
        }
        //kammide
        if (ImageData.ImageList_RobotIPSel_Trigger == 1)
        {
            send_cmd_to_dataproc("IMGLIST " + ImageData.ImageList_RobotIPSel_RobotIP + "\r\n");
            Debug.Log("tcpip: imglist " + ImageData.ImageList_RobotIPSel_RobotIP + "\r\n");
        }

        /////////////////////////  Histogramの指令を送信する部分 　///////////////////////////////////
        if (GroupData.Histgraph_trigger == 1)
        {
            if(GroupData.Graph_Vizualize_Robot == "ALL RED")
            {
                send_cmd_to_dataproc("HISTO RED 0 0.5\r\n");
                //Debug.Log("ここはDavidさんと相談してどう送るかを検討1: " + GroupData.Graph_Vizualize_Robot);
            }
            else
            {
                send_cmd_to_dataproc("HISTOROBOT "+ GroupData.Graph_Vizualize_Robot + " 0 0.5\r\n");
                //Debug.Log("ここはDavidさんと相談してどう送るかを検討2: " + GroupData.Graph_Vizualize_Robot);
            }
            GroupData.Histgraph_trigger = 0;
        }
        /////////////////////////////////////////////////////////////////////////////////////////
        
        /////////////////////////  Radiocon Modeからの指令を送信する部分  ///////////////////////////
        if (RM.Forward_trigger == true)
        {
            if(RM.one_times == 0)
            {
                if(RM.Radicon_RED_IP == "ALL RED")
                {
                    Debug.Log("ALLRED");
                    for (int i = 1; i < RED_Group.RED_Gr[RM.Radi_RED_Group_num].Count; i++)//Group名が最初だから1から回している
                    {
                        //stop
                        cmd_to_send = RED_Group.RED_Gr[RM.Radi_RED_Group_num][i] + " RADIO Forward\r\n";
                        cmd_to_send = tcpip_data.ip_addr + " RADIO Forward\r\n";
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    cmd_to_send = tcpip_data.ip_addr + " RADIO Forward\r\n";
                    send_cmd_to_relay(cmd_to_send);
                }
            }
            RM.one_times = 1;
        }
        if (RM.Right_trigger == true)
        {
            if (RM.one_times == 0)
            {
                if (RM.Radicon_RED_IP == "ALL RED")
                {
                    Debug.Log("ALLRED");
                    for (int i = 1; i < RED_Group.RED_Gr[RM.Radi_RED_Group_num].Count; i++)//Group名が最初だから1から回している
                    {
                        //stop
                        cmd_to_send = RED_Group.RED_Gr[RM.Radi_RED_Group_num][i] + " RADIO Right\r\n";
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    cmd_to_send = tcpip_data.ip_addr + " RADIO Right\r\n";
                    send_cmd_to_relay(cmd_to_send);
                }
                
            }
            RM.one_times = 1;
        }
        if (RM.Left_trigger == true)
        {
            if (RM.one_times == 0)
            {
                if (RM.Radicon_RED_IP == "ALL RED")
                {
                    Debug.Log("ALLRED");
                    for (int i = 1; i < RED_Group.RED_Gr[RM.Radi_RED_Group_num].Count; i++)//Group名が最初だから1から回している
                    {
                        //stop
                        cmd_to_send = RED_Group.RED_Gr[RM.Radi_RED_Group_num][i] + " RADIO Left\r\n";
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    cmd_to_send = tcpip_data.ip_addr + " RADIO Left\r\n";
                    send_cmd_to_relay(cmd_to_send);
                }

            }
            RM.one_times = 1;
        }
        if (RM.Stop_trigger == true)
        {
            if (RM.one_times == 0)
            {
                if (RM.Radicon_RED_IP == "ALL RED")
                {
                    Debug.Log("ALLRED");
                    for (int i = 1; i < RED_Group.RED_Gr[RM.Radi_RED_Group_num].Count; i++)//Group名が最初だから1から回している
                    {
                        //stop
                        cmd_to_send = RED_Group.RED_Gr[RM.Radi_RED_Group_num][i] + " RADIO Stop\r\n";
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    cmd_to_send = tcpip_data.ip_addr + " RADIO Stop\r\n";
                    send_cmd_to_relay(cmd_to_send);
                }

            }
            RM.one_times = 1;
        }
        if (RM.TurnRight_trigger == true)
        {
            if (RM.one_times == 0)
            {
                if (RM.Radicon_RED_IP == "ALL RED")
                {
                    Debug.Log("ALLRED");
                    for (int i = 1; i < RED_Group.RED_Gr[RM.Radi_RED_Group_num].Count; i++)//Group名が最初だから1から回している
                    {
                        //stop
                        cmd_to_send = RED_Group.RED_Gr[RM.Radi_RED_Group_num][i] + " RADIO PivotRight\r\n";
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    cmd_to_send = tcpip_data.ip_addr + " RADIO PivotRight\r\n";
                    send_cmd_to_relay(cmd_to_send);
                }

            }
            RM.one_times = 1;
        }
        if (RM.TurnLeft_trigger == true)
        {
            if (RM.one_times == 0)
            {
                if (RM.Radicon_RED_IP == "ALL RED")
                {
                    Debug.Log("ALLRED");
                    for (int i = 1; i < RED_Group.RED_Gr[RM.Radi_RED_Group_num].Count; i++)//Group名が最初だから1から回している
                    {
                        //stop
                        cmd_to_send = RED_Group.RED_Gr[RM.Radi_RED_Group_num][i] + " RADIO PivotLeft\r\n";
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    cmd_to_send = tcpip_data.ip_addr + " RADIO PivotLeft\r\n";
                    send_cmd_to_relay(cmd_to_send);
                }
               
            }
            RM.one_times = 1;
        }
        //////////////////////////////////////////////////////////////////////////////////


        /////  コマンドリレイにパラメータを送信する部分  /////////////
        if (CommandRelay_Trigger.stop_trigger == 1)
        {
            if (title_data.RED_IP_title == "ALL RED")
            {
                Debug.Log("ALLRED");
                for (int i = 1; i < RED_Group.RED_Gr[RED_Group.RED_Group_num].Count; i++)//Group名が最初だから1から回している
                {
                    //stop
                    cmd_to_send = RED_Group.RED_Gr[RED_Group.RED_Group_num][i] + " SETPARAMS " + send_data_Algo.send_json_param + "\r\n";
                    //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                    send_cmd_to_relay(cmd_to_send);
                    Thread.Sleep(50);
                }
            }
            else if (title_data.RED_IP_title == "EntireRED" && screen_position.EntireScreen == 1)
            {

                for (int j = 0; j < EntireData.GroupVisu_RED_Data.Count; j++)//Group名が最初だから1から回している
                {
                    //shutdown
                    //cmd_to_send = EntireData.Entire_RED_Data[j][0] + " SETPARAMS " + send_data_Algo.send_json_param + "\r\n";
                    cmd_to_send = EntireData.GroupVisu_RED_Data[j][0] + " SETPARAMS " + send_data_Algo.send_json_param + "\r\n";
                    //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                    send_cmd_to_relay(cmd_to_send);
                    Thread.Sleep(50);
                }
            }
            else if (title_data.RED_IP_title == "SelectRED")
            {
                if (screen_position.EntireScreen == 1) 
                { 
                    for (int j = 0; j < Clone_RED_IP.Select_RED_IP_Entire.Count; j++)//Group名が最初だから1から回している
                    {
                        //shutdown
                        cmd_to_send = Clone_RED_IP.Select_RED_IP_Entire[j] + " SETPARAMS " + send_data_Algo.send_json_param + "\r\n";
                        //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
                else if(screen_position.GroupScreen == 1)
                {
                    for (int j = 0; j < Clone_RED_IP.Select_RED_IP_Group.Count; j++)//Group名が最初だから1から回している
                    {
                        //shutdown
                        cmd_to_send = Clone_RED_IP.Select_RED_IP_Group[j] + " SETPARAMS " + send_data_Algo.send_json_param + "\r\n";
                        //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
            }
            else
            {
                Debug.Log("Solo");
                //shutdown
                cmd_to_send = tcpip_data.ip_addr + " SETPARAMS " + send_data_Algo.send_json_param + "\r\n";
                //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                send_cmd_to_relay(cmd_to_send);
            }
            CommandRelay_Trigger.stop_trigger = 0;
            Debug.Log("kokokokokokoko");
        }
        if (CommandRelay_Trigger.Explor_trigger == 1)
        {
            if (title_data.RED_IP_title == "ALL RED")
            {
                Debug.Log("ALLRED");
                for (int i = 1; i < RED_Group.RED_Gr[RED_Group.RED_Group_num].Count; i++)//Group名が最初だから1から回している
                {
                    cmd_to_send = RED_Group.RED_Gr[RED_Group.RED_Group_num][i] + " STARTEXPLORE\r\n";
                    send_cmd_to_relay(cmd_to_send);
                    Thread.Sleep(50);
                }
            }
            else if (title_data.RED_IP_title == "EntireRED" && screen_position.EntireScreen == 1)
            {

                for (int j = 0; j < EntireData.GroupVisu_RED_Data.Count; j++)//Group名が最初だから1から回している
                {
                    //shutdown
                    //cmd_to_send = EntireData.Entire_RED_Data[j][0] + " STARTEXPLORE\r\n";
                    cmd_to_send = EntireData.GroupVisu_RED_Data[j][0] + " STARTEXPLORE\r\n";
                    //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                    send_cmd_to_relay(cmd_to_send);
                    Thread.Sleep(50);
                }
            }
            else if (title_data.RED_IP_title == "SelectRED")
            {
                if (screen_position.EntireScreen == 1)
                {
                    for (int j = 0; j < Clone_RED_IP.Select_RED_IP_Entire.Count; j++)//Group名が最初だから1から回している
                    {
                        //shutdown
                        cmd_to_send = Clone_RED_IP.Select_RED_IP_Entire[j] + " STARTEXPLORE\r\n";
                        //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
                else if (screen_position.GroupScreen == 1)
                {
                    for (int j = 0; j < Clone_RED_IP.Select_RED_IP_Group.Count; j++)//Group名が最初だから1から回している
                    {
                        //shutdown
                        cmd_to_send = Clone_RED_IP.Select_RED_IP_Group[j] + " STARTEXPLORE\r\n";
                        //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
            }
            else
            {
                Debug.Log("Solo");
                //shutdown
                cmd_to_send = tcpip_data.ip_addr + " STARTEXPLORE\r\n";
                //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                send_cmd_to_relay(cmd_to_send);
            }

            CommandRelay_Trigger.Explor_trigger = 0;
        }
        if (CommandRelay_Trigger.Restart_trigger == 1)
        {
            if (title_data.RED_IP_title == "ALL RED")
            {
                Debug.Log("ALLRED");
                for (int i = 1; i < RED_Group.RED_Gr[RED_Group.RED_Group_num].Count; i++)//Group名が最初だから1から回している
                {
                    cmd_to_send = RED_Group.RED_Gr[RED_Group.RED_Group_num][i] + " RESTART\r\n";
                    send_cmd_to_relay(cmd_to_send);
                    Thread.Sleep(50);
                }
            }
            else if (title_data.RED_IP_title == "EntireRED" && screen_position.EntireScreen == 1)
            {

                for (int j = 0; j < EntireData.GroupVisu_RED_Data.Count; j++)//Group名が最初だから1から回している
                {
                    //shutdown
                    //cmd_to_send = EntireData.Entire_RED_Data[j][0] + " RESTART\r\n";
                    cmd_to_send = EntireData.GroupVisu_RED_Data[j][0] + " RESTART\r\n";
                    //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                    send_cmd_to_relay(cmd_to_send);
                    Thread.Sleep(50);
                }
            }
            else if (title_data.RED_IP_title == "SelectRED")
            {
                if (screen_position.EntireScreen == 1)
                {
                    for (int j = 0; j < Clone_RED_IP.Select_RED_IP_Entire.Count; j++)//Group名が最初だから1から回している
                    {
                        //shutdown
                        cmd_to_send = Clone_RED_IP.Select_RED_IP_Entire[j] + " RESTART\r\n";
                        //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
                else if (screen_position.GroupScreen == 1)
                {
                    for (int j = 0; j < Clone_RED_IP.Select_RED_IP_Group.Count; j++)//Group名が最初だから1から回している
                    {
                        //shutdown
                        cmd_to_send = Clone_RED_IP.Select_RED_IP_Group[j] + " RESTART\r\n";
                        //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
            }
            else
            {
                Debug.Log("Solo");
                //shutdown
                cmd_to_send = tcpip_data.ip_addr + " RESTART\r\n";
                //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                send_cmd_to_relay(cmd_to_send);
            }
            CommandRelay_Trigger.Restart_trigger = 0;
        }
        if (CommandRelay_Trigger.shutdown_trigger == 1)
        {
            if (title_data.RED_IP_title == "ALL RED")
            {
                Debug.Log("ALLRED");
                for (int i = 1; i < RED_Group.RED_Gr[RED_Group.RED_Group_num].Count; i++)//Group名が最初だから1から回している
                {
                    cmd_to_send = RED_Group.RED_Gr[RED_Group.RED_Group_num][i] + " SHUTDOWN\r\n";
                    send_cmd_to_relay(cmd_to_send);
                    Thread.Sleep(50);
                }
            }
            else if (title_data.RED_IP_title == "EntireRED" && screen_position.EntireScreen == 1)
            {

                for (int j = 0; j < EntireData.GroupVisu_RED_Data.Count; j++)//Group名が最初だから1から回している
                {
                    //shutdown
                    //cmd_to_send = EntireData.Entire_RED_Data[j][0] + " SHUTDOWN\r\n";
                    cmd_to_send = EntireData.GroupVisu_RED_Data[j][0] + " SHUTDOWN\r\n";
                    //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                    send_cmd_to_relay(cmd_to_send);
                    Thread.Sleep(50);
                }
            }
            else if(title_data.RED_IP_title == "SelectRED")
            {
                if (screen_position.EntireScreen == 1)
                {
                    for (int j = 0; j < Clone_RED_IP.Select_RED_IP_Entire.Count; j++)//Group名が最初だから1から回している
                    {
                        //shutdown
                        cmd_to_send = Clone_RED_IP.Select_RED_IP_Entire[j] + " SHUTDOWN\r\n";
                        //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
                else if (screen_position.GroupScreen == 1)
                {
                    for (int j = 0; j < Clone_RED_IP.Select_RED_IP_Group.Count; j++)//Group名が最初だから1から回している
                    {
                        //shutdown
                        cmd_to_send = Clone_RED_IP.Select_RED_IP_Group[j] + " SHUTDOWN\r\n";
                        //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                        send_cmd_to_relay(cmd_to_send);
                        Thread.Sleep(50);
                    }
                }
            }
             else
            {
                Debug.Log("Solo");
                //shutdown
                cmd_to_send = tcpip_data.ip_addr + " SHUTDOWN\r\n";
                //Debug.LogFormat(ip_addr, process_replystr(send_cmd_to_relay(cmd_to_send)));
                send_cmd_to_relay(cmd_to_send);
            }

            CommandRelay_Trigger.shutdown_trigger = 0;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////


        if(check == 1)
        {
            ReceiveData_Data_cmd();
            check = 0;
        }

        if (send_count == 1)
        {
            ReceiveData_Data_Processor();
            if (first_access == 1)
            {
                ReceiveData_Data_cmd();
                first_access = 2;
                tcpip_data.receive_count = 0;
            }

            send_count = 0;
        }
       

        if(GroupData.Get_Freq_Command)
        {
       
            if (RM.one_times == 0)
            {
                send_cmd_to_dataproc("FREQLIST RED\r\n");
            }
            RM.one_times = 1;
        }


        /////////////////////////////  Algorithmモードのパラメータを送信する部分　　/////////////////////////
        if (send_data_Algo.send_param_count == 1)
        {
            if (RED_Group.RED_IP == "ALL RED")
            {
                Debug.Log("ALLRED");
                for (int i = 1; i < RED_Group.RED_Gr[RED_Group.RED_Group_num].Count; i++)//Group名が最初だから1から回している
                {
                    send_data_Algo.send_all_param = RED_Group.RED_Gr[RED_Group.RED_Group_num][i] + " SETPARAMS " + send_data_Algo.send_json_param + "\r\n";
                    send_cmd_to_relay(send_data_Algo.send_all_param);
                    Thread.Sleep(50);
                }
            }
            else
            {
                Debug.Log("Solo");
                send_data_Algo.send_all_param = RED_Group.RED_IP + " SETPARAMS " + send_data_Algo.send_json_param + "\r\n";
                send_cmd_to_relay(send_data_Algo.send_all_param);
            }
            send_data_Algo.send_param_count = 0;
        }
        //////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////// Broker IP 接続部分  //////////////////////////////////////
        if (title_data.broker_ip_command == 1)
        {
            if (title_data.RED_IP_title == "ALL RED")
            {
                Debug.Log("ALLRED");
                for (int i = 1; i < RED_Group.RED_Gr[RED_Group.RED_Group_num].Count; i++)//Group名が最初だから1から回している
                {  // kide exploration tag
                    send_broker_everyrob = RED_Group.RED_Gr[RED_Group.RED_Group_num][i] + " SENDBROKERIP " + input_brokeripaddress.text + " " + title_data.Explore_Tag + "\r\n";
                    send_cmd_to_relay(send_broker_everyrob);
                    Thread.Sleep(50);
                }
            }
            else
            {
                Debug.Log("Solo");
                send_broker_everyrob = title_data.RED_IP_title + " SENDBROKERIP " + input_brokeripaddress.text + " " + title_data.Explore_Tag + "\r\n";
                send_cmd_to_relay(send_broker_everyrob);
            }
            title_data.broker_ip_command = 0;
        }
        //////////////////////////////////////////////////////////////////////////////////////////

    }


    //プルダウンで選択したREDの情報を獲得するために，データ受信側にデータを送る
    public void Receive_REDData_Button()
    {
        Debug.Log("koko3");
        try
        {
            send_RED_Num = EntireData.select_RED_Num;
            var buffer = Encoding.UTF8.GetBytes(send_RED_Num);
            //var buffer = Encoding.UTF8.GetBytes(m_message1);
            //string text = System.Text.Encoding.UTF8.GetString(buffer);
            m_networkStream.Write(buffer, 0, buffer.Length);
            //m_networkStream.Write(text, 0, text.Length);

            Debug.LogFormat("送信成功：{0}", send_RED_Num);
            buffer = null;

        }
        catch (Exception)
        {
            // サーバが起動しておらず送信に失敗した場合はここに来ます
            // SocketException 型だと例外のキャッチができないようなので
            // Exception 型で例外をキャッチしています
            Debug.LogError("送信失敗");
        }
        Debug.Log("koko4");
    }

    //サーバからの文字列を受信するための関数です
    private void OnProcess_CR()
    {
            var receivebuffer = new byte[65535];
            var count = m_networkStream.Read(receivebuffer, 0, receivebuffer.Length);

            // クライアントからの接続が切断された場合は
            if (count == 0)
            {
                Debug.Log("切断");

                // 通信に使用したインスタンスを破棄して
                OnDestroy();

                // 再度クライアントからの接続を待機します
                Task.Run(() => OnProcess_CR());

                //break;
            }

            // クライアントから文字列を受信した場合は
            // GUI とログに出力します
            receiveMessage = Encoding.UTF8.GetString(receivebuffer, 0, count);
     
    }

    /// <summary>
    /// 破棄する時に呼び出されます
    /// </summary>
    private void OnDestroy()
    {
        // 通信に使用したインスタンスを破棄します
        // Awake 関数でインスタンスの生成に失敗している可能性もあるので
        // null 条件演算子を使用しています
        m_tcpClient?.Dispose();
        m_networkStream?.Dispose();

        Debug.Log("切断");
    }

   /////////  リアルタイムで周波数ごとのデータを送信する部分　 ///////////////////////
    private void OnProcess_RED_data()
    {
        while (true)
        {
            ////////////  Group(周波数)ごとにロボットのパラメータを獲得するためにコマンドを送る所  /////////////////
            if (screen_position.EntireScreen == 1)
            {
                if (EntireData.Group_send_command)
                {
                    //for(int i = 0; i < RED_Group.RED_Gr.Count; i++)
                    //{
                    //    if (EntireData.Entire_Group_Command == RED_Group.RED_Gr[i][0])
                    //    {
                    //        send_cmd_to_dataproc("ROBOTLIST RED " + EntireData.Entire_Group_Command + "\r\n");
                    //        EntireData.Group_reddata_param_count = 1;
                    //        Thread.Sleep(3000);
                    //        //GroupData.Freq_Group_Command = null;
                    //        Debug.Log("Okkkkkkkkkkk");
                    //    }

                    //}
                    send_cmd_to_dataproc("ROBOTLIST RED 0\r\n");
                    EntireData.Group_reddata_param_count = 1;
                    Thread.Sleep(3000);
                    //GroupData.Freq_Group_Command = null;
                    Debug.Log("Okkkkkkkkkkk");

                }
            }


            if(destroy_count == 1)
            {
                // 通信に使用したインスタンスを破棄して
                OnDestroy();
                OnDestroy_RED_data();
                break;
            }
        }
    }

    public void ReceiveData_Data_cmd()
    {
        var receivebuffer = new byte[65535];
        var count = m_networkStream.Read(receivebuffer, 0, receivebuffer.Length);
        // クライアントから文字列を受信した場合は
        // GUI とログに出力します
        receiveMessage = Encoding.UTF8.GetString(receivebuffer, 0, count);
        tcpip_data.RED_data_cmd = receiveMessage;
        Debug.Log("Data: " + receiveMessage);
    }

    public void ReceiveData_Data_Processor()
    {
        var receivebuffer_RED_data = new byte[65535];
        var count = m_networkStream_RED_data.Read(receivebuffer_RED_data, 0, receivebuffer_RED_data.Length);
        // クライアントから文字列を受信した場合は
        // GUI とログに出力します
        receiveMessage_RED_data = Encoding.UTF8.GetString(receivebuffer_RED_data, 0, count);
        tcpip_data.RED_data = receiveMessage_RED_data;
        Debug.Log("Data: " + receiveMessage_RED_data);
        Debug.Log("NetworkStream.: " + count);

        tcpip_data.receive_count = 1;
    }

    /// <summary>
    /// 破棄する時に呼び出されます
    /// </summary>
    private void OnDestroy_RED_data()
    {
        // 通信に使用したインスタンスを破棄します
        // Awake 関数でインスタンスの生成に失敗している可能性もあるので
        // null 条件演算子を使用しています
        m_tcpClient_RED_data?.Dispose();
        m_networkStream_RED_data?.Dispose();

        Debug.Log("切断");
    }


    public void TCPIP_Click_Button()
    {
        try
        {
            //指定された IP アドレスとポートでサーバに接続します
            //m_tcpClient = new TcpClient(m_ipAddress, m_port);
            m_tcpClient_RED_data = new TcpClient(input_ipaddress.text, Port_RED_data);
            m_tcpClient_RED_data.ReceiveTimeout = 2000; // タイムアウトを5秒に設定
            m_networkStream_RED_data = m_tcpClient_RED_data.GetStream();
            //m_networkStream_RED_data_1 = m_tcpClient_RED_data.GetStream();
            //confirm_connect.GetComponent<Text>().text = "Connected !!";
            //Debug.LogFormat("接続成功");

            //なぜか必要
            Task.Run(() => OnProcess_RED_data());
        }
        catch (SocketException ex)
        {
            // サーバが起動しておらず接続に失敗した場合はここに来ます
            Debug.LogError("接続失敗");
            Debug.LogError("SocketException: " + ex.Message);
        }

        try
        {
            //指定された IP アドレスとポートでサーバに接続します
            //m_tcpClient = new TcpClient(m_ipAddress, m_port);
            m_tcpClient = new TcpClient(input_ipaddress.text, Port_CR);
            m_tcpClient.ReceiveTimeout = 2000; // タイムアウトを5秒に設定
            m_networkStream = m_tcpClient.GetStream();
            Debug.LogFormat("接続成功");

            //Task.Run(() => OnProcess_CR());

            ReceiveData_Data_Processor();
            ReceiveData_Data_cmd();
            // Password here - kide
//            send_cmd_to_dataproc(title_data.Explore_Tag + "\r\n");
//            send_cmd_to_relay(title_data.Explore_Tag + "\r\n");
            send_cmd_to_dataproc("NLABRED\r\n");
            send_cmd_to_relay("NLABRED\r\n");

            tcpip_data.server_connect_command = 1;

            Debug.Log("TCP -" + ImageData.ImageBaseURL + "- tcp:" + tcpip_data.server_connect_command.ToString());

        }
        catch (SocketException ex)
        {
            // サーバが起動しておらず接続に失敗した場合はここに来ます
            Debug.LogError("接続失敗");
            Debug.LogError("SocketException: " + ex.Message);
        }


        tcpip_data.receive_count = 0;


    }

    public void BrokerConnect_Click_Button()
    {
        //send_cmd_to_relay(input_brokeripaddress.text+" SENDBROKERIP\r\n");
        title_data.broker_ip_command = 1;
        tcpip_data.broker_connect_command = 1;
        //check = 1;
    }

    //stop用の文字列
    //private string paramset_STOP_str = "{"IsExploring": False, "TransitTime": 2.0,"Mu": 1.0, "Sigma": 1.0,"Outer_Rth": 3.0, "Inner_Rth": 0.0,"Height": 2.2, "BetweenMarkers": 0.0,"Height_Correction": False, "Reject": "A","MarkerColor": "", "ShutterSpeed": 100,"LeftPWM": 0, "RightPWM": 0,"Xcoord": 0, "Ycoord": 0}";

    //関数：cmd_to_sendをstring型にして一回送信，受信したもののsring型に変換
    private void send_cmd_to_relay(string cmdstring)
    {
        try
        {
            var buffer = Encoding.UTF8.GetBytes(cmdstring);
            //var buffer = Encoding.UTF8.GetBytes(m_message1);
            //string text = System.Text.Encoding.UTF8.GetString(buffer);
            m_networkStream.Write(buffer, 0, buffer.Length);
            //m_networkStream.Write(text, 0, text.Length);

            Debug.LogFormat("送信成功：{0}", cmdstring);
            buffer = null;

        }
        catch (Exception)
        {
            // サーバが起動しておらず送信に失敗した場合はここに来ます
            // SocketException 型だと例外のキャッチができないようなので
            // Exception 型で例外をキャッチしています
            Debug.LogError("送信失敗");
        }
        //if(first_access == 0)
        //{
        //    var receivebuffer = new byte[65535];
        //    var count = m_networkStream.Read(receivebuffer, 0, receivebuffer.Length);
        //    // クライアントから文字列を受信した場合は
        //    // GUI とログに出力します
        //    receiveMessage = Encoding.UTF8.GetString(receivebuffer, 0, count);
        //    Debug.Log("Data: " + receiveMessage_RED_data);
        //}
    }

    //関数：data_to_sendをstring型にして一回送信，受信したもののsring型に変換
    public void send_cmd_to_dataproc(string datastring)
    {
        try
        {
            var buffer = Encoding.UTF8.GetBytes(datastring);
            //var buffer = Encoding.UTF8.GetBytes(m_message1);
            //string text = System.Text.Encoding.UTF8.GetString(buffer);
            m_networkStream_RED_data.Write(buffer, 0, buffer.Length);
            //m_networkStream.Write(text, 0, text.Length);

            Debug.LogFormat("送信成功：{0}", datastring);
            buffer = null;
            Thread.Sleep(50);

        }
        catch (Exception)
        {
            // サーバが起動しておらず送信に失敗した場合はここに来ます
            // SocketException 型だと例外のキャッチができないようなので
            // Exception 型で例外をキャッチしています
            Debug.LogError("送信失敗");
        }
      

       send_count = 1;

    }

    //関数：replystrの中身で送信できたかを判断
    private void process_replystr(string replystring)
    {
        string replystr_proc;

        if (replystring == "ACK")
        {
            replystr_proc = "送信成功";
        }
        else if (replystring == "ERR")
        {
            replystr_proc = "送信失敗";
        }
        else
        {
            replystr_proc = "未知のコマンドです";
        }

        Debug.Log("送信状況：　" + replystr_proc);

    }

    //終了処理
    private void OnApplicationQuit()
    {
        Debug.Log("アプリ終了");
        destroy_count = 1;
    }

}
