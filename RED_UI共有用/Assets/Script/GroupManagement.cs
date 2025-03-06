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
using System.Linq;
using XCharts;

public class GroupData
{
    public static string Freq_Group_Command = null;
    public static List<string> Group_Freq_Data = new List<string>();
    public static int freq_reddata_param_count = 0;
    public static int Histgraph_trigger = 0;
    public static bool Get_Freq_Command;
    public static bool Freq_send_command;
    public static List<List<string>> Group_RED_Data = new List<List<string>>();
    public static string Graph_Vizualize_Robot = null;
}

////////////// 当初はGroupManagementだったが，現在はグラフを可視化する部分のコード  //////////////////
public class GroupManagement : MonoBehaviour {

    public GameObject Graph_name_dropdown_List;
    Dropdown ddtmp_Graph_name_List;
    public GameObject Group_IP_dropdown_Graph;
    Dropdown ddtmp_Group_Ip_Graph;

    ///////////////////// XChart グラフ描画  ////////////////////////////
    [Header("BarChart_")]
    public GameObject BarChart;       //BarChart本体
    private BarChart barchart;       //BarChartコンポーネント
    private Serie serie;
    int histgram_count = 0;

    [Header("LineChart_")]
    public GameObject LineChart;       //LineChart
                                       
    private LineChart linechart;       //BarChart
    private Serie serie_LineChart;
    int GaussGraph_count = 0;
    /////////////////////////////////////////////////////////////////////

    int initial_gui_count = 0;
    int remember_Group_Num;
    //int aaa = 0;
    int push_count_gr = 0;

    public GameObject Details_data;

    
    public int freqList_count = 0;

    public GameObject FreqList_Dropdown;
    Dropdown ddtmp_FreqList; /*  */
    public GameObject Group_IP_dropdown;
    Dropdown ddtmp_Group_Ip;

    Transform[] children; // 子オブジェクト達を入れる配列

    //public Image image;
    private Sprite sprite;

    public GameObject contentPanel; // ScrollView内のコンテンツ用のパネル
    public GameObject contentPrefab; // 動的なコンテンツのプレハブ

    private GameObject item;

    private List<GameObject> contentItems = new List<GameObject>(); // 生成されたコンテンツのリスト


    void Start()
    {
        ddtmp_Graph_name_List = Graph_name_dropdown_List.GetComponent<Dropdown>();
        ddtmp_Group_Ip_Graph = Group_IP_dropdown_Graph.GetComponent<Dropdown>();

        barchart = BarChart.GetComponent<BarChart>();
        barchart.RemoveData();
        serie = barchart.AddSerie(SerieType.Bar, "data_name"); //Set graph type
        barchart.AnimationEnable(false); //graph animation 
        Set_Size(); //set graph size
        Set_Title();

        /*set graph range*/
        Set_Y_Axis();
        Set_X_Axis();

        linechart = LineChart.GetComponent<LineChart>();
        linechart.RemoveData();
        serie_LineChart = linechart.AddSerie(SerieType.Line, "data_name"); //Set graph type
        linechart.AnimationEnable(false);
        serie_LineChart.lineType = LineType.Smooth;
        serie_LineChart.symbol.type = SerieSymbolType.None;
        linechart.RefreshChart();
        Set_Size_LineChart();
        Set_Title_LineChart();
        Set_Y_Axis_LineChart();
        Set_X_Axis_LineChart();
        serie_LineChart.lineStyle.color = new Color(0.5f, 1.0f, 0.0f, 255);

        Debug.Log("launch");

        ddtmp_FreqList = FreqList_Dropdown.GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tcpip_data.view_flag == 1)
        {
            DetailView_RED(tcpip_data.RED_data);
            tcpip_data.view_flag = 0;
        }
        if (tcpip_data.RED_num_flag == 1)
        {
            //dropdown_button(tcpip_data.RED_data);
            tcpip_data.RED_num_flag = 0;
        }
        //if (RED_Group.RED_Group_Command == 1)
        //{
        //    //dropdown_redgroup(RED_Group.RED_Gr, RED_Group.RED_Gr.Count);
        //    //dropdown_redgroupIP(RED_Group.RED_Gr, RED_Group.RED_Gr[0].Count);
        //    RED_Group.RED_Group_Command = 0;
        //}
        if (tcpip_data.receive_count == 1 && freqList_count == 1)
        {
            Debug.Log("OKkkkkkkkkkkkkkkkkkkkkkkkkkk");
            dropdown_Freqgroup();
            tcpip_data.receive_count = 0;
            freqList_count = 0;
            Debug.Log("OKkkkkkkkkkkkkwwwwwwwwwwwwwww");
        }

        //if (tcpip_data.receive_count == 1 && GroupData.freq_reddata_param_count == 1)
        //{
        //    split_Group_data(tcpip_data.RED_data);
        //    tcpip_data.receive_count = 0;
        //    GroupData.freq_reddata_param_count = 0;
        //}

        if(histgram_count == 1 && tcpip_data.receive_count == 1)
        {
            Histgram_Visualization(tcpip_data.RED_data);
            GaussianGraph_Vizualization(tcpip_data.RED_data);
            tcpip_data.receive_count = 0;
            histgram_count = 0;
        }

        if (RED_Group.RED_Group_Command.Equals(1))
        {
            //Debug.Log("//////////////////////////////////////////////");
            dropdown_Robotgroup_Graph(RED_Group.RED_Gr, RED_Group.RED_Gr.Count);
            dropdown_redgroupIP_nothing_Graph();

        }

    }

    //////////////////////////////////  Histgram関係のXChartの関数  /////////////////////////////////////////////////////////////////////////////////////
    private void Set_Size()
    {
        barchart.SetSize(1200, 380);
        //barchart.SetPosition(0, 75, 0);
    }

    private void Set_Title()
    {
        barchart.title.show = true;
        barchart.title.textStyle.fontSize = 30;
        barchart.title.textStyle.color = new Color(0f, 0f, 0f, 255f);
        barchart.title.text = "Distance Histgram";
    }

    private void Set_Y_Axis()
    {
        barchart.yAxes[0].show = true;
        barchart.yAxes[0].type = Axis.AxisType.Value;
        barchart.yAxes[0].minMaxType = Axis.AxisMinMaxType.Custom;
        barchart.yAxes[0].min = 0;  //min value 
        barchart.yAxes[0].max = 10; //max value
        barchart.yAxes[0].splitNumber = 10; //num of scales
        barchart.yAxes[0].axisLabel.interval = 4;//num of scales between desired labels to be displayed."
    }

    private void Set_X_Axis()
    {
        barchart.xAxes[0].show = true;
        barchart.xAxes[0].type = Axis.AxisType.Value;
        barchart.xAxes[0].minMaxType = Axis.AxisMinMaxType.Custom;
        barchart.xAxes[0].min = 0;  //min value
        barchart.xAxes[0].max = 10; //max value
        barchart.xAxes[0].splitNumber = 10 * 4; //num of scales
        barchart.xAxes[0].axisLabel.interval = 3;//num of scales between desired labels to be displayed."
        serie.barWidth = 0.2f; //width of the bar
        serie.barGap = 0.5f;

    }
    public void Histgram_Visualization(string inputString)
    {
        barchart.ClearData();//clear graph 

        //string inputString = "0.00:3,3.00:1,13.25:1,3.25:1,0.00:5,8.25:10, 0.25:1";
        //string inputString = "0.00:3,0.25:3,0.50:3,0.75:3,1.00:3,1.25:3,1.50:3,1.75:3,2.00:3,2.25:3,2.50:3,2.75:3,3.00:3,3.25:3,3.50:3,3.75:3,4.00:3,4.25:3,4.50:3,4.75:3,5.00:3,5.25:3,5.50:3,5.75:3,6.00:3,6.25:3,6.50:3,6.75:3,7.00:3,7.25:3,7.50:3,7.75:3,8.00:3,8.25:3,8.50:3,8.75:3,9.00:3,9.25:3,9.50:3,9.75:3,10.00:3,10.25:3,10.50:3,10.75:3,11.00:3,11.25:3,11.50:3,11.75:3,12.00:3,12.25:3,12.50:3,12.75:3";

        char[] outerSeparators = { ',' };
        char[] innerSeparators = { ':' };

        string[] del_newline = { "\r\n" };

        // 最初に","で区切って配列に格納
        string[] split_newline = inputString.Split(del_newline, StringSplitOptions.None);
        string[] outerArray = split_newline[0].Split(outerSeparators, StringSplitOptions.None);

        // 各要素を":"で区切ってそれぞれの値を異なる配列に格納
        List<float> xValues = new List<float>();
        List<int> yValues = new List<int>();

        foreach (string item in outerArray)
        {
            // ":"で区切って配列に格納
            string[] innerArray = item.Split(innerSeparators, StringSplitOptions.None);

            // xValuesにfloat型の値を追加
            xValues.Add(float.Parse(innerArray[0]));
            // yValuesにint型の値を追加
            yValues.Add(int.Parse(innerArray[1]));
        }

        // xValuesを基準にしてリストをソート
        ListSort(xValues, yValues);

        // 同じxの値を持つ場合に、yの値を足し合わせる
        CombineSameXValues(xValues, yValues);

        // ソートおよび合計結果をログに表示
        for (int i = 0; i < xValues.Count; i++)
        {
            Debug.Log($"x: {xValues[i]}, y: {yValues[i]}");
        }

        // 最小値と最大値を取得

        /*
        float minX = xValues[0];
        float maxX = xValues[xValues.Count - 1];
        */

        float minX = xValues.Min();
        float maxX = xValues.Max();
        int x_axis_max = (int)Math.Ceiling(maxX)+1;
        int x_axis_min = (int)Math.Floor(minX);
        int y_axis_max = (int)Mathf.Ceil(yValues.Max() / 5.0f) * 5;
        //int minY = yValues.Min();

        //グラフのリサイズ
        resize_axis(x_axis_max, x_axis_min, y_axis_max);

        /*グラフ化用にデータ整理*/
        List<float> newXValues = new List<float>();
        for (float x = x_axis_min; x <= x_axis_max; x += 0.25f)
        {
            newXValues.Add(x);
        }

        // xValuesに存在するxの値に対応するyの値を新しいyValuesに代入
        List<int> newYValues = new List<int>();
        foreach (float x in newXValues)
        {
            int index = xValues.IndexOf(x);
            newYValues.Add(index != -1 ? yValues[index] : 0);
        }

        Debug.Log($"Max X: {newXValues.Count}"); //うまくいっているか確認

        /*グラフ用データプロット*/
        Plot_Data(newXValues, newYValues, x_axis_min);


        // 変数の中身確認
        Debug.Log($"Min X: {minX}, Max X: {maxX} {y_axis_max}");
    }

    private void resize_axis(int x_axis_max, int x_axis_min, int y_axis_max)
    {
        barchart.xAxes[0].min = x_axis_min;  //min value
        barchart.xAxes[0].max = x_axis_max; //max value
        barchart.xAxes[0].splitNumber = (x_axis_max - x_axis_min) * 2;
        barchart.xAxes[0].axisLabel.interval = 3;
        barchart.yAxes[0].min = 0;  //min value
        barchart.yAxes[0].max = y_axis_max; //max value
        barchart.yAxes[0].splitNumber = 10; //軸を分割するメモリ数
        barchart.yAxes[0].axisLabel.interval = 2;//表示したいラベルの間のメモリの数

    }


    private void Plot_Data(List<float> newXValues, List<int> newYValues, int x_axis_min)
    {
        double _f;
        float fl;
        for (int i = 0; i < newXValues.Count-1; i++)
        {
            _f = i * 0.25 + x_axis_min;
            fl = (float)_f;
            barchart.AddData(0, fl, newYValues[i]);
        }
    }

    // リストをソート
    void ListSort(List<float> xValues, List<int> yValues)
    {
        // xValuesを基準でソート
        List<Tuple<float, int>> listToSort = new List<Tuple<float, int>>();
        for (int i = 0; i < xValues.Count; i++)
        {
            listToSort.Add(new Tuple<float, int>(xValues[i], yValues[i]));
        }

        listToSort.Sort((x, y) => x.Item1.CompareTo(y.Item1));

        for (int i = 0; i < xValues.Count; i++)
        {
            xValues[i] = listToSort[i].Item1;
            yValues[i] = listToSort[i].Item2;
        }
    }

    // 同じxの値を持つ場合に、yの値を足し合わせる関数
    void CombineSameXValues(List<float> xValues, List<int> yValues)
    {
        for (int i = 1; i < xValues.Count; i++)
        {
            if (xValues[i] == xValues[i - 1])
            {
                // 同じxの値を持つ場合、yの値を足し合わせる
                yValues[i] += yValues[i - 1];
                // 前のyの値を0に設定（重複を避けるため）
                yValues[i - 1] = 0;
            }
        }

        // yの値が0の要素だけ消す
        List<int> newYValues = new List<int>();
        List<float> newXValues = new List<float>();

        for (int i = 0; i < yValues.Count; i++)
        {
            if (yValues[i] != 0)
            {
                newYValues.Add(yValues[i]);
                newXValues.Add(xValues[i]);
            }
        }

        xValues.Clear();
        xValues.AddRange(newXValues);
        yValues.Clear();
        yValues.AddRange(newYValues);
    }

    ////////////  HistogramやGaussグラフを更新する所  ////////////////////////////////////
    public void Graph_Update_Button()
    {
        GaussianGraph_Vizualization("0.00:3,3.00:1,13.25:1,3.25:1,0.00:5,8.25:10, 0.25:1");
        GroupData.Histgraph_trigger = 1;
        histgram_count = 1;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////  Gauss関係のXcahrt  /////////////////////////////////////////
    private void Set_Size_LineChart()
    {
        linechart.SetSize(1200, 380);
    }

    private void Set_Title_LineChart()
    {
        linechart.title.show = true;
        linechart.title.textStyle.fontSize = 30;
        linechart.title.textStyle.color = new Color(0f, 0f, 0f, 255f);
        linechart.title.text = "Defined/Actual Gauss Distribution";
    }

    private void Set_Y_Axis_LineChart()
    {
        linechart.yAxes[0].show = true;
        linechart.yAxes[0].type = Axis.AxisType.Value;
        linechart.yAxes[0].minMaxType = Axis.AxisMinMaxType.Custom;
        linechart.yAxes[0].min = 0;  //���̍ŏ��l
        linechart.yAxes[0].max = 1.0f; //���̍ő�l
        linechart.yAxes[0].splitNumber = 10; //���𕪊����郁������
        linechart.yAxes[0].axisLabel.interval = 4;//�\�����������x���̊Ԃ̃������̐�
    }

    private void Set_X_Axis_LineChart()
    {
        linechart.xAxes[0].show = true;
        linechart.xAxes[0].type = Axis.AxisType.Value;
        linechart.xAxes[0].minMaxType = Axis.AxisMinMaxType.Custom;
        linechart.xAxes[0].min = 0;  //���̍ŏ��l
        linechart.xAxes[0].max = 10; //���̍ő�l
        linechart.xAxes[0].splitNumber = 10; //���𕪊����郁������
        linechart.xAxes[0].axisLabel.interval = 9;//�\�����������x���̊Ԃ̃������̐�
    }

    private void Test_Data_LineChart()
    {
        double _f;
        float fl;
        for (int i = 0; i <= 200; i++)
        {
            _f = i * 0.05;
            fl = (float)_f;
            linechart.AddData(0, fl, 0); //AddData(index�ԍ�, x , Y)
        }

    }

    public void GaussianGraph_Vizualization(string inputString)
    {
        //linechart.RefreshChart(); //refresh graph data
        linechart.ClearData();//clear graph 

        double _average = 0, _sigma2 = 0;
        int _data_num = 0;
        double _napier_num = Math.Exp(1);
        List<double> _x_list = new List<double>();
        List<int> _y_list = new List<int>();
        List<double> _plot_point = new List<double>();

        /* Organizing received data */
       // string inputString = "0.00:3,3.00:1,13.25:1,3.25:1,0.00:5,8.25:10,8.00:30, 0.25:1"; // Received data type is assumed as "String Type"

        char[] outerSeparators = { ',' };
        char[] innerSeparators = { ':' };

        // �ŏ���","�ŋ�؂��Ĕz��Ɋi�[
        string[] outerArray = inputString.Split(outerSeparators, StringSplitOptions.None);

        // �e�v�f��":"�ŋ�؂��Ă��ꂼ��̒l���قȂ�z��Ɋi�[
        List<float> xValues_LineChart = new List<float>();
        List<int> yValues_LineChart = new List<int>();

        foreach (string item in outerArray)
        {
            // ":"�ŋ�؂��Ĕz��Ɋi�[
            string[] innerArray = item.Split(innerSeparators, StringSplitOptions.None);

            // xValues��float�^�̒l��ǉ�
            xValues_LineChart.Add(float.Parse(innerArray[0]));
            // yValues��int�^�̒l��ǉ�
            yValues_LineChart.Add(int.Parse(innerArray[1]));
        }

        // xValues����ɂ��ă��X�g���\�[�g
        ListSort(xValues_LineChart, yValues_LineChart);

        // ����x�̒l�����ꍇ�ɁAy�̒l�𑫂����킹��
        CombineSameXValues(xValues_LineChart, yValues_LineChart);


        /* Calculating average */
        for (int i = 0; i < xValues_LineChart.Count; i++)
        {
            _average += xValues_LineChart[i] * yValues_LineChart[i];
            _data_num += yValues_LineChart[i];
        }
        _average = _average / _data_num;

        /*Calculating description*/
        for (int i = 0; i < xValues_LineChart.Count; i++)
        {
            _sigma2 += Math.Pow((xValues_LineChart[i] - _average), 2) * yValues_LineChart[i];
        }
        _sigma2 = _sigma2 / _data_num;
        //_sigma2 = 0.5f;


        //int plotPointSize = _plot_point.Count;

        Debug.Log(_average);
        Debug.Log(_sigma2);



        /*set x_axis scale  ***width �}3 sigma*** */
        int _max_xscale = (int)Math.Ceiling(_average + 3 * Math.Sqrt(_sigma2));
        int _min_xscale = (int)Math.Floor(_average - 3 * Math.Sqrt(_sigma2));
        if (_min_xscale < 0)
        {
            _min_xscale = 0;
        }
        // _min_xscale = 2;
        int _x_range = _max_xscale - _min_xscale;

        double Z, _x_plot;



        for (int i = 0; i <= 200; i++)
        {
            _x_plot = _min_xscale + _x_range * 0.005f * i;
            Z = 1 / (Math.Sqrt(2 * Math.PI) * Math.Sqrt(_sigma2)) * Math.Pow(_napier_num, -(Math.Pow((_x_plot - _average), 2) / (2 * _sigma2)));
            _plot_point.Add(Z);
        }
        Debug.Log(_plot_point);

        double _Max_Point = (int)Mathf.Ceil((float)_plot_point.Max() * 10.0f) * 0.1;

        resize_axis_LineChart(_max_xscale, _min_xscale, (float)_Max_Point);


        /* float _y;
         double y;
         *//*plot*//*
         for (int i = 0; i <= 200 ; i++)
         {   
             y = _plot_point[i];
             _y = (float)y;
             linechart.UpdateData(0,i, _y);
         }*/
        double _f;
        float fl, _xplt;
        for (int i = 0; i <= 200; i++)
        {
            _xplt = (float)(_min_xscale + _x_range * 0.005f * i);
            _f = _plot_point[i];
            fl = (float)_f;
            linechart.AddData(0, _xplt, fl); //AddData(index�ԍ�, x , y)***x,y��float�^��***
        }
    }
    // ���X�g���\�[�g
    void ListSort_LineChart(List<float> xValues_LineChart, List<int> yValues_LineChart)
    {
        // xValues����Ń\�[�g
        List<Tuple<float, int>> listToSort = new List<Tuple<float, int>>();
        for (int i = 0; i < xValues_LineChart.Count; i++)
        {
            listToSort.Add(new Tuple<float, int>(xValues_LineChart[i], yValues_LineChart[i]));
        }

        listToSort.Sort((x, y) => x.Item1.CompareTo(y.Item1));

        for (int i = 0; i < xValues_LineChart.Count; i++)
        {
            xValues_LineChart[i] = listToSort[i].Item1;
            yValues_LineChart[i] = listToSort[i].Item2;
        }
    }

    // ����x�̒l�����ꍇ�ɁAy�̒l�𑫂����킹��֐�
    void CombineSameXValues_LineChart(List<float> xValues_LineChart, List<int> yValues_LineChart)
    {
        for (int i = 1; i < xValues_LineChart.Count; i++)
        {
            if (xValues_LineChart[i] == xValues_LineChart[i - 1])
            {
                // ����x�̒l�����ꍇ�Ay�̒l�𑫂����킹��
                yValues_LineChart[i] += yValues_LineChart[i - 1];
                // �O��y�̒l��0�ɐݒ�i�d��������邽�߁j
                yValues_LineChart[i - 1] = 0;
            }
        }

        // y�̒l��0�̗v�f��������
        List<int> newYValues = new List<int>();
        List<float> newXValues = new List<float>();

        for (int i = 0; i < yValues_LineChart.Count; i++)
        {
            if (yValues_LineChart[i] != 0)
            {
                newYValues.Add(yValues_LineChart[i]);
                newXValues.Add(xValues_LineChart[i]);
            }
        }

        xValues_LineChart.Clear();
        xValues_LineChart.AddRange(newXValues);
        yValues_LineChart.Clear();
        yValues_LineChart.AddRange(newYValues);
    }

    private void resize_axis_LineChart(int x_axis_max, int x_axis_min, float y_axis_max)
    {
        linechart.yAxes[0].show = true;
        linechart.yAxes[0].type = Axis.AxisType.Value;
        linechart.yAxes[0].minMaxType = Axis.AxisMinMaxType.Custom;
        linechart.yAxes[0].min = 0;  //���̍ŏ��l
        linechart.yAxes[0].max = y_axis_max; //���̍ő�l
        linechart.yAxes[0].splitNumber = 10; //���𕪊����郁������
        linechart.yAxes[0].axisLabel.interval = 4;//�\�����������x���̊Ԃ̃������̐�
        linechart.xAxes[0].show = true;
        linechart.xAxes[0].type = Axis.AxisType.Value;
        linechart.xAxes[0].minMaxType = Axis.AxisMinMaxType.Custom;
        linechart.xAxes[0].min = x_axis_min;  //min value
        linechart.xAxes[0].max = x_axis_max; //max value
        linechart.xAxes[0].splitNumber = (x_axis_max - x_axis_min) * 2;
        linechart.xAxes[0].axisLabel.interval = 3;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void dropdown_Freqgroup()
    {
        GroupData.Group_Freq_Data.Clear();
        List<string> FreqGrList = new List<string>();
        string[] del = { "\r\n" };
        string[] data_size_freq = tcpip_data.RED_data.Split(del, StringSplitOptions.None);
        Debug.Log("-^-^-^-^-^-^-^-^-^-^-^-^-^" + data_size_freq.Length);
        for (int i = 0; i < data_size_freq.Length; i++)
        {
            if (i == 0)
            {
                GroupData.Group_Freq_Data.Add("Select Freqency");
            }
            else
            {
                Debug.Log("kkkooo22222");
                string[] split_freq_details = data_size_freq[i].Split(',');
                for (int j = 0; j < split_freq_details.Length; j++)
                {
                    GroupData.Group_Freq_Data.Add(split_freq_details[j]);
                }
            }

        }

        for (int j = 0; j < GroupData.Group_Freq_Data.Count; j++)
        {
            Debug.Log("Dataaaaaa: " + GroupData.Group_Freq_Data[j]);
        }
        

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp_FreqList = FreqList_Dropdown.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_FreqList.ClearOptions();

        //リストを追加
        ddtmp_FreqList.AddOptions(GroupData.Group_Freq_Data);
    }

    /////////////////////////////   ドロップダウンでロボットグループを獲得する所
    public void dropdown_Robotgroup_Graph(List<List<string>> data_redgroup, int data_group_size)
    {
        List<string> REDGrList = new List<string>();

        REDGrList.Add("Select RED Group");
        for (int j = 0; j < data_group_size; j++)
        {
            //Optionsに表示する文字列をリストに追加
            REDGrList.Add(data_redgroup[j][0]);
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp_Graph_name_List = Graph_name_dropdown_List.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Graph_name_List.ClearOptions();

        //リストを追加
        ddtmp_Graph_name_List.AddOptions(REDGrList);
    }

    public void dropdown_redgroupIP_nothing_Graph()
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
        ddtmp_Group_Ip_Graph = Group_IP_dropdown_Graph.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_Ip_Graph.ClearOptions();

        //リストを追加
        ddtmp_Group_Ip_Graph.AddOptions(REDGrIP_List);
    }

    public void dropdown_group_List_Graph_call(int num)
    {
        if (ddtmp_Graph_name_List.value == 0)
        {
            Debug.Log("Skip");
            dropdown_redgroupIP_nothing_Graph();
        }
        else
        {
            Debug.Log("koko11111");
            Debug.Log("++++" + ddtmp_Graph_name_List.value);
            Debug.Log(ddtmp_Graph_name_List.options[ddtmp_Graph_name_List.value].text);
            dropdown_redgroupIP_Graph(RED_Group.RED_Gr, ddtmp_Graph_name_List.value - 1);
        }

        //dropdown_paramset(paramSets, ddtmp_Param.value);
        GroupData.Freq_Group_Command = ddtmp_FreqList.options[ddtmp_FreqList.value].text;
        GroupData.freq_reddata_param_count = 1;
        GroupData.Freq_send_command = true;
        //dropdown_redgroupIP(RED_Group.RED_Gr, ddtmp_FreqList.value);
    }

    public void dropdown_redgroupIP_Graph(List<List<string>> data_redgroupIP, int data_group_num)
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
        ddtmp_Group_Ip_Graph = Group_IP_dropdown_Graph.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Group_Ip_Graph.ClearOptions();

        //リストを追加
        ddtmp_Group_Ip_Graph.AddOptions(REDGrIP_List);
    }

    public void dropdown_RobotIP_call_Graph(int num)
    {
        Debug.Log(ddtmp_Group_Ip_Graph.options[ddtmp_Group_Ip_Graph.value].text);
        //tcpip_data.ip_addr = ddtmp_Group_Ip_Radi.options[ddtmp_Group_Ip_Radi.value].text;
        GroupData.Graph_Vizualize_Robot = ddtmp_Group_Ip_Graph.options[ddtmp_Group_Ip_Graph.value].text;
    }

    // 新しいコンテンツを追加する関数
    public void AddContent(string contentText)
    {
        GameObject item = Instantiate(contentPrefab);
        item.transform.SetParent(contentPanel.transform, false);
        contentItems.Add(item);

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
            Debug.Log("aaa[" + i + "]: " + data_param[i]);
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

    //public void FreqList_dropdown_call(int Freq_num)
    //{
    //    Debug.Log("++++" + ddtmp_FreqList.value);
    //    Debug.Log(ddtmp_FreqList.options[Freq_num].text);
    //}

    public void check_button()
    {

        //DetailView_RED("213 231 23 213 213 234");
        //DropDownコンポーネントのoptionsから選択されてているvalueをindexで指定し、
        //選択されている文字を樹徳
        EntireData.select_RED_Num = ddtmp_FreqList.options[ddtmp_FreqList.value].text;

        //ログに出力
        Debug.Log(EntireData.select_RED_Num);
    }


    public void Data_Record_Button()
    {
        Debug.Log("Data Record");
    }

    public void Get_FreqList_Button_Push()
    {
        GroupData.Get_Freq_Command = true;
    }
    public void Get_FreqList_Button_Leave()
    {
        Debug.Log("Get Freq List Leave");
        freqList_count = 1;
        GroupData.Get_Freq_Command = false;
        RM.one_times = 0;
    }
    ////////////////////////////////////////////////////////////////////////////////////

   
}
