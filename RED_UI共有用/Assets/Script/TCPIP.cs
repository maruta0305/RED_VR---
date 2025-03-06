//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine.SceneManagement;
//using System.IO;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Threading;

//public class tcpip_data
//{
//    public static string RED_data = null;
//    public static int view_flag = 0;
//    public static int RED_num_flag = 0;
//}

//public class TCPIP : MonoBehaviour
//{

//    //インスタンス化
//    //EntireData EM = new EntireData();
//    //RM rm = new RM();
//    //EntireManagement EM = new EntireManagement();

//    /// <RadiconModeのデータを送信する変数>
//    private string send_forward = null;
//    private string send_Right = null;
//    private string send_Left = null;
//    private string send_Stop = null;
//    private string send_TurnRight = null;
//    private string send_TurnLeft = null;
//    /// 

//    //TCPIP
//    private TcpClient m_tcpClient;
//    private NetworkStream m_networkStream;
//    private TcpClient m_tcpClient_RED_data;
//    private NetworkStream m_networkStream_RED_data;
//    private bool m_isConnection;//接続確認フラグ    public int accessedCounter = 0;
//                                //InputFieldを格納するための変数
//    InputField input_ipaddress;
//    int Port_CR = 40000;
//    int Port_RED_data = 40001;
//    public GameObject confirm_connect;
//    public GameObject ipaddress_input;
//    public string receiveText;
//    public string receiveMessage = null;
//    public string receiveMessage_RED_data = null;
//    private string m_message = string.Empty; // サーバから受信した文字列
//    private string m_message0 = "0";
//    private string m_message1 = "1"; // サーバに送信する文字列(Yes)
//    private string m_message2 = "2"; // サーバに送信する文字列(No)
//    private string m_message3 = "3"; // サーバから受信する文字列(Yes, No)
//    private string m_message4 = "4";
//    string send_RED_Num = null;

//    int i = 0;

//    // Start is called before the first frame update
//    void Start()
//    {
//        //InputFieldコンポーネントを取得
//        input_ipaddress = ipaddress_input.GetComponent<InputField>();
//    }

//    // Update is called once per frame
//    void Update()
//    {

//        /////////////////////////  Radicon Modeからの指令を送信する部分  ///////////////////////////
//        if (RM.Forward_trigger == true)
//        {
//            try
//            {
//                send_forward = "tahara";
//                var buffer = Encoding.UTF8.GetBytes(send_forward);
//                //var buffer = Encoding.UTF8.GetBytes(m_message1);
//                //string text = System.Text.Encoding.UTF8.GetString(buffer);
//                m_networkStream.Write(buffer, 0, buffer.Length);
//                //m_networkStream.Write(text, 0, text.Length);

//                Debug.LogFormat("送信成功：{0}", send_forward);
//                buffer = null;

//            }
//            catch (Exception)
//            {
//                // サーバが起動しておらず送信に失敗した場合はここに来ます
//                // SocketException 型だと例外のキャッチができないようなので
//                // Exception 型で例外をキャッチしています
//                Debug.LogError("送信失敗");
//            }
//        }
//        if (RM.Right_trigger == true)
//        {
//            try
//            {
//                send_Right = "Right";
//                var buffer = Encoding.UTF8.GetBytes(send_Right);
//                //var buffer = Encoding.UTF8.GetBytes(m_message1);
//                //string text = System.Text.Encoding.UTF8.GetString(buffer);
//                m_networkStream.Write(buffer, 0, buffer.Length);
//                //m_networkStream.Write(text, 0, text.Length);

//                Debug.LogFormat("送信成功：{0}", send_Right);
//                buffer = null;

//            }
//            catch (Exception)
//            {
//                // サーバが起動しておらず送信に失敗した場合はここに来ます
//                // SocketException 型だと例外のキャッチができないようなので
//                // Exception 型で例外をキャッチしています
//                Debug.LogError("送信失敗");
//            }
//        }
//        if (RM.Left_trigger == true)
//        {
//            try
//            {
//                send_Left = "Left";
//                var buffer = Encoding.UTF8.GetBytes(send_Left);
//                //var buffer = Encoding.UTF8.GetBytes(m_message1);
//                //string text = System.Text.Encoding.UTF8.GetString(buffer);
//                m_networkStream.Write(buffer, 0, buffer.Length);
//                //m_networkStream.Write(text, 0, text.Length);

//                Debug.LogFormat("送信成功：{0}", send_Left);
//                buffer = null;

//            }
//            catch (Exception)
//            {
//                // サーバが起動しておらず送信に失敗した場合はここに来ます
//                // SocketException 型だと例外のキャッチができないようなので
//                // Exception 型で例外をキャッチしています
//                Debug.LogError("送信失敗");
//            }
//        }
//        if (RM.Stop_trigger == true)
//        {
//            try
//            {
//                send_Stop = "Stop";
//                var buffer = Encoding.UTF8.GetBytes(send_Stop);
//                //var buffer = Encoding.UTF8.GetBytes(m_message1);
//                //string text = System.Text.Encoding.UTF8.GetString(buffer);
//                m_networkStream.Write(buffer, 0, buffer.Length);
//                //m_networkStream.Write(text, 0, text.Length);

//                Debug.LogFormat("送信成功：{0}", send_Stop);
//                buffer = null;

//            }
//            catch (Exception)
//            {
//                // サーバが起動しておらず送信に失敗した場合はここに来ます
//                // SocketException 型だと例外のキャッチができないようなので
//                // Exception 型で例外をキャッチしています
//                Debug.LogError("送信失敗");
//            }
//        }
//        if (RM.TurnRight_trigger == true)
//        {
//            try
//            {
//                send_TurnRight = "TurnRight";
//                var buffer = Encoding.UTF8.GetBytes(send_TurnRight);
//                //var buffer = Encoding.UTF8.GetBytes(m_message1);
//                //string text = System.Text.Encoding.UTF8.GetString(buffer);
//                m_networkStream.Write(buffer, 0, buffer.Length);
//                //m_networkStream.Write(text, 0, text.Length);

//                Debug.LogFormat("送信成功：{0}", send_TurnRight);
//                buffer = null;

//            }
//            catch (Exception)
//            {
//                // サーバが起動しておらず送信に失敗した場合はここに来ます
//                // SocketException 型だと例外のキャッチができないようなので
//                // Exception 型で例外をキャッチしています
//                Debug.LogError("送信失敗");
//            }
//        }
//        if (RM.TurnLeft_trigger == true)
//        {
//            try
//            {
//                send_TurnLeft = "TurnLeft";
//                var buffer = Encoding.UTF8.GetBytes(send_TurnLeft);
//                //var buffer = Encoding.UTF8.GetBytes(m_message1);
//                //string text = System.Text.Encoding.UTF8.GetString(buffer);
//                m_networkStream.Write(buffer, 0, buffer.Length);
//                //m_networkStream.Write(text, 0, text.Length);

//                Debug.LogFormat("送信成功：{0}", send_TurnLeft);
//                buffer = null;

//            }
//            catch (Exception)
//            {
//                // サーバが起動しておらず送信に失敗した場合はここに来ます
//                // SocketException 型だと例外のキャッチができないようなので
//                // Exception 型で例外をキャッチしています
//                Debug.LogError("送信失敗");
//            }
//        }
//        //////////////////////////////////////////////////////////////////////////////////
        
//        if(send_data_Algo.send_param_count == 1)
//        {
//            try
//            {
//                var buffer = Encoding.UTF8.GetBytes(send_data_Algo.send_all_param);
//                //var buffer = Encoding.UTF8.GetBytes(m_message1);
//                //string text = System.Text.Encoding.UTF8.GetString(buffer);
//                m_networkStream.Write(buffer, 0, buffer.Length);
//                //m_networkStream.Write(text, 0, text.Length);

//                Debug.LogFormat("送信成功：{0}", send_data_Algo.send_all_param);
//                buffer = null;

//            }
//            catch (Exception)
//            {
//                // サーバが起動しておらず送信に失敗した場合はここに来ます
//                // SocketException 型だと例外のキャッチができないようなので
//                // Exception 型で例外をキャッチしています
//                Debug.LogError("送信失敗");
//            }

//            send_data_Algo.send_param_count = 2;
//        }

//    }


//    //プルダウンで選択したREDの情報を獲得するために，データ受信側にデータを送る
//    public void Receive_REDData_Button()
//    {
//        Debug.Log("koko3");
//        try
//        {
//            send_RED_Num = EntireData.select_RED_Num;
//            var buffer = Encoding.UTF8.GetBytes(send_RED_Num);
//            //var buffer = Encoding.UTF8.GetBytes(m_message1);
//            //string text = System.Text.Encoding.UTF8.GetString(buffer);
//            m_networkStream.Write(buffer, 0, buffer.Length);
//            //m_networkStream.Write(text, 0, text.Length);

//            Debug.LogFormat("送信成功：{0}", send_RED_Num);
//            buffer = null;

//        }
//        catch (Exception)
//        {
//            // サーバが起動しておらず送信に失敗した場合はここに来ます
//            // SocketException 型だと例外のキャッチができないようなので
//            // Exception 型で例外をキャッチしています
//            Debug.LogError("送信失敗");
//        }
//        Debug.Log("koko4");
//    }

//    //サーバからの文字列を受信するための関数です
//    private void OnProcess_CR()
//    {
//        while (true)
//        {
//            var receivebuffer = new byte[256];
//            var count = m_networkStream.Read(receivebuffer, 0, receivebuffer.Length);

//            // クライアントからの接続が切断された場合は
//            if (count == 0)
//            {
//                Debug.Log("切断");

//                // 通信に使用したインスタンスを破棄して
//                OnDestroy();

//                // 再度クライアントからの接続を待機します
//                Task.Run(() => OnProcess_CR());

//                break;
//            }

//            // クライアントから文字列を受信した場合は
//            // GUI とログに出力します
//            receiveMessage = Encoding.UTF8.GetString(receivebuffer, 0, count);
//        }
//    }

//    /// <summary>
//    /// 破棄する時に呼び出されます
//    /// </summary>
//    private void OnDestroy()
//    {
//        // 通信に使用したインスタンスを破棄します
//        // Awake 関数でインスタンスの生成に失敗している可能性もあるので
//        // null 条件演算子を使用しています
//        m_tcpClient?.Dispose();
//        m_networkStream?.Dispose();

//        Debug.Log("切断");
//    }

//    //サーバからの文字列を受信するための関数です
//    private void OnProcess_RED_data()
//    {
//        while (true)
//        {
//            var receivebuffer_RED_data = new byte[256];
//            var count = m_networkStream_RED_data.Read(receivebuffer_RED_data, 0, receivebuffer_RED_data.Length);

//            // クライアントからの接続が切断された場合は
//            if (count == 0)
//            {
//                Debug.Log("切断");

//                // 通信に使用したインスタンスを破棄して
//                OnDestroy_RED_data();

//                // 再度クライアントからの接続を待機します
//                Task.Run(() => OnProcess_RED_data());

//                break;
//            }

//            // クライアントから文字列を受信した場合は
//            // GUI とログに出力します
//            receiveMessage_RED_data = Encoding.UTF8.GetString(receivebuffer_RED_data, 0, count); 
//            tcpip_data.RED_data = receiveMessage_RED_data;
//            string[] data_size = receiveMessage_RED_data.Split(' ');
//            Debug.Log("Data: " + receiveMessage_RED_data);
//            Debug.Log("Size: " + data_size.Length);
//            if (data_size.Length < 5)//ここの条件式は適当(順次変更していく)
//            {
//                tcpip_data.RED_num_flag = 1;
//            }
//            else if (data_size.Length > 5)
//            {
//                tcpip_data.view_flag = 1;
//            }

//            Debug.Log("完了");

//        }
//    }

//    /// <summary>
//    /// 破棄する時に呼び出されます
//    /// </summary>
//    private void OnDestroy_RED_data()
//    {
//        // 通信に使用したインスタンスを破棄します
//        // Awake 関数でインスタンスの生成に失敗している可能性もあるので
//        // null 条件演算子を使用しています
//        m_tcpClient_RED_data?.Dispose();
//        m_networkStream_RED_data?.Dispose();

//        Debug.Log("切断");
//    }

//    public void TCPIP_Click_Button()
//    {
//        try
//        {
//            //指定された IP アドレスとポートでサーバに接続します
//            //m_tcpClient = new TcpClient(m_ipAddress, m_port);
//            m_tcpClient = new TcpClient(input_ipaddress.text, Port_CR);
//            m_networkStream = m_tcpClient.GetStream();
//            //confirm_connect.GetComponent<Text>().text = "Connected !!";
//            //Debug.LogFormat("接続成功");

//            //接続確認テキストに接続出来たことをを記入します
//            //Text LabelConnect = textConnection.GetComponent<Text>();
//            //LabelConnect.text = "接続OK";

//            //なぜか必要
//            Task.Run(() => OnProcess_CR());
//        }
//        catch (SocketException)
//        {
//            // サーバが起動しておらず接続に失敗した場合はここに来ます
//            Debug.LogError("接続失敗");
//        }

//        try
//        {
//            //指定された IP アドレスとポートでサーバに接続します
//            //m_tcpClient = new TcpClient(m_ipAddress, m_port);
//            m_tcpClient_RED_data = new TcpClient(input_ipaddress.text, Port_RED_data);
//            m_networkStream_RED_data = m_tcpClient_RED_data.GetStream();
//            confirm_connect.GetComponent<Text>().text = "Connected !!";
//            Debug.LogFormat("接続成功");

//            //接続確認テキストに接続出来たことをを記入します
//            //Text LabelConnect = textConnection.GetComponent<Text>();
//            //LabelConnect.text = "接続OK";

//            //なぜか必要
//            Task.Run(() => OnProcess_RED_data());
//        }
//        catch (SocketException)
//        {
//            // サーバが起動しておらず接続に失敗した場合はここに来ます
//            Debug.LogError("接続失敗");
//        }
//    }

//}
