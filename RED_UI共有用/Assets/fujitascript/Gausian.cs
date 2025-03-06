using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts;
using System;
using System.Threading;

public class Gausian : MonoBehaviour
{
    [Header("LineChart_")]
    public GameObject LineChart;       //LineChart�{��
                                       //�v���C�x�[�g
    private LineChart linechart;       //BarChart�R���|�[�l���g
    private Serie serie;
    // Start is called before the first frame update
    void Start()
    {
        linechart = LineChart.GetComponent<LineChart>();
        linechart.RemoveData();
        serie = linechart.AddSerie(SerieType.Line, "data_name"); //Set graph type
        linechart.AnimationEnable(false);
        serie.lineType = LineType.Smooth;
        serie.symbol.type = SerieSymbolType.None;
        linechart.RefreshChart();
        Set_Size();
        Set_Title();
        Set_Y_Axis();
        Set_X_Axis();
        Test_Data();

    }

    private void Set_Size()
    {
        linechart.SetSize(1200, 380);
    }

    private void Set_Title()
    {
        linechart.title.show = true;
        linechart.title.textStyle.fontSize = 20;
        linechart.title.textStyle.color = new Color(0f, 0.2f, 1f, 1f);
        linechart.title.text = "�ށ[�񂵂���Ɨp";
    }

    private void Set_Y_Axis()
    {
        linechart.yAxes[0].show = true;
        linechart.yAxes[0].type = Axis.AxisType.Value;
        linechart.yAxes[0].minMaxType = Axis.AxisMinMaxType.Custom;
        linechart.yAxes[0].min = 0;  //���̍ŏ��l
        linechart.yAxes[0].max = 1.0f; //���̍ő�l
        linechart.yAxes[0].splitNumber = 10; //���𕪊����郁������
        linechart.yAxes[0].axisLabel.interval = 4;//�\�����������x���̊Ԃ̃������̐�
    }

    private void Set_X_Axis()
    {
        linechart.xAxes[0].show = true;
        linechart.xAxes[0].type = Axis.AxisType.Value;
        linechart.xAxes[0].minMaxType = Axis.AxisMinMaxType.Custom;
        linechart.xAxes[0].min = 0;  //���̍ŏ��l
        linechart.xAxes[0].max = 10; //���̍ő�l
        linechart.xAxes[0].splitNumber = 10; //���𕪊����郁������
        linechart.xAxes[0].axisLabel.interval = 9;//�\�����������x���̊Ԃ̃������̐�
        //serie.barWidth = 0.2f; //�_�O���t�̕�
    }

    private void Test_Data()
    {
        double _f;
        float fl;
        for (int i = 0; i < 100; i++)
        {
            _f = i * 0.1;
            fl = (float)_f;
            linechart.AddData(0,fl, 0); //AddData(index�ԍ�, x , Y)
        }

    }


    // Update is called once per frame
    void Update()
    {
        linechart.RefreshChart(); //refresh graph data

        double _average=0, _sigma2=0;
        int _data_num = 0;
        double _napier_num = Math.Exp(1);
        List<double> _x_list = new List<double>();
        List<int> _y_list = new List<int>();
        List<double> _plot_point = new List<double>();

        /* Organizing received data */
        string inputString = "0.00:3,3.00:1,13.25:1,3.25:1,0.00:5,8.25:10, 0.25:1"; // Received data type is assumed as "String Type"

        char[] outerSeparators = { ',' };
        char[] innerSeparators = { ':' };

        // �ŏ���","�ŋ�؂��Ĕz��Ɋi�[
        string[] outerArray = inputString.Split(outerSeparators, StringSplitOptions.None);

        // �e�v�f��":"�ŋ�؂��Ă��ꂼ��̒l���قȂ�z��Ɋi�[
        List<float> xValues = new List<float>();
        List<int> yValues = new List<int>();

        foreach (string item in outerArray)
        {
            // ":"�ŋ�؂��Ĕz��Ɋi�[
            string[] innerArray = item.Split(innerSeparators, StringSplitOptions.None);

            // xValues��float�^�̒l��ǉ�
            xValues.Add(float.Parse(innerArray[0]));
            // yValues��int�^�̒l��ǉ�
            yValues.Add(int.Parse(innerArray[1]));
        }

        // xValues����ɂ��ă��X�g���\�[�g
        ListSort(xValues, yValues);

        // ����x�̒l�����ꍇ�ɁAy�̒l�𑫂����킹��
        CombineSameXValues(xValues, yValues);


        /* Calculate Average */
        for (int i = 0;i < xValues.Count; i++) {
            _average += xValues[i] * yValues[i];
            _data_num += yValues[i];
        }
        _average = _average / _data_num;

        /*calculate discription*/
        for (int i = 0; i < _data_num; i++){
            _sigma2 += Math.Pow((_y_list[i] - _average),2);
        }
        _sigma2 = _sigma2 / _data_num;



        Debug.Log(_average);
        Debug.Log(_sigma2);
        //_sigma2 = 0.5;
        double Z;

        for (int i = 0; i < 100; i++)
        {
            Z = 1/ (Math.Sqrt(2 * Math.PI) * Math.Sqrt(_sigma2)) * Math.Pow(_napier_num, -(Math.Pow((_x_list[i] - _average),2) / (2 * _sigma2)));
            _plot_point.Add(Z);
        }
        Debug.Log(_plot_point);


        float _y;
        double y;

        for (int i = 0; i < 100 ; i++)
        {
            y = _plot_point[i];
            _y = (float)y;
            linechart.UpdateData(0,i, _y);
        }

    }

    // ���X�g���\�[�g
    void ListSort(List<float> xValues, List<int> yValues)
    {
        // xValues����Ń\�[�g
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

    // ����x�̒l�����ꍇ�ɁAy�̒l�𑫂����킹��֐�
    void CombineSameXValues(List<float> xValues, List<int> yValues)
    {
        for (int i = 1; i < xValues.Count; i++)
        {
            if (xValues[i] == xValues[i - 1])
            {
                // ����x�̒l�����ꍇ�Ay�̒l�𑫂����킹��
                yValues[i] += yValues[i - 1];
                // �O��y�̒l��0�ɐݒ�i�d��������邽�߁j
                yValues[i - 1] = 0;
            }
        }

        // y�̒l��0�̗v�f��������
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
