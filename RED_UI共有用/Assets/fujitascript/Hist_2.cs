using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using XCharts;
//using XCharts.Runtime;
public class Hist_2 : MonoBehaviour
{
    [Header("BarChart_")]
    public GameObject BarChart;       //BarChart本体
    private BarChart barchart;       //BarChartコンポーネント
    private Serie serie;


    // Start is called before the first frame update
    void Start()
    {
        barchart = BarChart.GetComponent<BarChart>();
        barchart.RemoveData();
        serie = barchart.AddSerie(SerieType.Bar, "data_name"); //Set graph type
        barchart.AnimationEnable(false); //graph animation 
        Set_Size(); //set graph size
        Set_Title(); 

        /*set graph range*/
        Set_Y_Axis(); 
        Set_X_Axis(); 

        Debug.Log("launch");

    }

    private void Set_Size()
    {
        barchart.SetSize(1200, 380);
        //barchart.SetPosition(0, 75, 0);
    }

    private void Set_Title()
    {
        barchart.title.show = true;
        barchart.title.textStyle.fontSize = 20;
        barchart.title.textStyle.color = new Color(0f, 0.2f, 1f, 1f);
        barchart.title.text = "月撃ち";
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
        barchart.xAxes[0].splitNumber = 10*4; //num of scales
        barchart.xAxes[0].axisLabel.interval = 3;//num of scales between desired labels to be displayed."
        serie.barWidth = 0.2f; //width of the bar
        serie.barGap = 0.5f;
    }


    // Update is called once per frame
    void Update()
    {
        barchart.ClearData();//clear graph 

        string inputString = "0.00:3,3.00:1,13.25:1,3.25:1,0.00:5,8.25:10, 0.25:1";
        //string inputString = "0.00:3,0.25:3,0.50:3,0.75:3,1.00:3,1.25:3,1.50:3,1.75:3,2.00:3,2.25:3,2.50:3,2.75:3,3.00:3,3.25:3,3.50:3,3.75:3,4.00:3,4.25:3,4.50:3,4.75:3,5.00:3,5.25:3,5.50:3,5.75:3,6.00:3,6.25:3,6.50:3,6.75:3,7.00:3,7.25:3,7.50:3,7.75:3,8.00:3,8.25:3,8.50:3,8.75:3,9.00:3,9.25:3,9.50:3,9.75:3,10.00:3,10.25:3,10.50:3,10.75:3,11.00:3,11.25:3,11.50:3,11.75:3,12.00:3,12.25:3,12.50:3,12.75:3";

        char[] outerSeparators = { ',' };
        char[] innerSeparators = { ':' };

        // 最初に","で区切って配列に格納
        string[] outerArray = inputString.Split(outerSeparators, StringSplitOptions.None);

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
        int x_axis_max = (int)Math.Ceiling(maxX);
        int x_axis_min = (int)Math.Floor(minX);
        int y_axis_max = (int)Mathf.Ceil(yValues.Max() / 10.0f) * 10;
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
        barchart.xAxes[0].splitNumber = (x_axis_max -  x_axis_min) * 2;
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
        for (int i = 0; i < newXValues.Count; i++)
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
}



 
