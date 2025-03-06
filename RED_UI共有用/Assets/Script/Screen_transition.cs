using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screen_position
{
    public static int title = 1;
    public static int AlgoScreen = 0;
    public static int RadiconScreen = 0;
    public static int EntireScreen = 0;
    public static int GroupScreen = 0;
    public static int ImageScreen = 0;
    public static int TestScreen = 0;
    public static int makeREDGroupScreen = 0;
}

public class Screen_transition : MonoBehaviour
{

    public GameObject PanelWalls;

    // Start is called before the first frame update
    void Start()
    {
       // PanelWalls.transform.localPosition = new Vector2(-3000.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //画面移動
    public void Tittle_Click_Button()
    {
        //Debug.Log("Payment");
        PanelWalls.transform.localPosition = new Vector2(0.0f, 0.0f);
        screen_position.title = 1;
        screen_position.AlgoScreen = 0;
        screen_position.RadiconScreen = 0;
        screen_position.EntireScreen = 0;
        screen_position.GroupScreen = 0;
        screen_position.ImageScreen = 0;
        screen_position.TestScreen = 0;
        screen_position.makeREDGroupScreen = 0;
        //EntireData.GroupAll_reddata_param_count = 0;
    }
    //画面移動
    public void Algorithm_Click_Button()
    {
        //Debug.Log("Payment");
        PanelWalls.transform.localPosition = new Vector2(-3000.0f, 0.0f);
        //Debug.Log("Payment!!!");
        screen_position.title = 0;
        screen_position.AlgoScreen = 1;
        screen_position.RadiconScreen = 0;
        screen_position.EntireScreen = 0;
        screen_position.GroupScreen = 0;
        screen_position.ImageScreen = 0;
        screen_position.TestScreen = 0;
        screen_position.makeREDGroupScreen = 0;
        // EntireData.GroupAll_reddata_param_count = 0;
    }
    //画面移動
    public void RadiCon_Click_Button()
    {
        //Debug.Log("Payment");
        PanelWalls.transform.localPosition = new Vector2(-6000.0f, 0.0f);
        screen_position.title = 0;
        screen_position.AlgoScreen = 0;
        screen_position.RadiconScreen = 1;
        screen_position.EntireScreen = 0;
        screen_position.GroupScreen = 0;
        screen_position.ImageScreen = 0;
        screen_position.TestScreen = 0;
        screen_position.makeREDGroupScreen = 0;
        //EntireData.GroupAll_reddata_param_count = 0;
    }
    //画面移動
    public void Entire_Click_Button()
    {
        //Debug.Log("Payment");
        PanelWalls.transform.localPosition = new Vector2(-9000.0f, 0.0f);
        screen_position.title = 0;
        screen_position.AlgoScreen = 0;
        screen_position.RadiconScreen = 0;
        screen_position.EntireScreen = 1;
        screen_position.GroupScreen = 0;
        screen_position.ImageScreen = 0;
        screen_position.TestScreen = 0;
        screen_position.makeREDGroupScreen = 0;
        //EntireData.GroupAll_reddata_param_count = 1;
    }
    //画面移動
    public void Group_Click_Button()
    {
        //Debug.Log("Payment");
        PanelWalls.transform.localPosition = new Vector2(-12000.0f, 0.0f);
        screen_position.title = 0;
        screen_position.AlgoScreen = 0;
        screen_position.RadiconScreen = 0;
        screen_position.EntireScreen = 0;
        screen_position.GroupScreen = 1;
        screen_position.ImageScreen = 0;
        screen_position.TestScreen = 0;
        screen_position.makeREDGroupScreen = 0;
        //EntireData.GroupAll_reddata_param_count = 0;
    }
    //画面移動
    public void Image_Click_Button()
    {
        //Debug.Log("Payment");
        PanelWalls.transform.localPosition = new Vector2(-15000.0f, 0.0f);
        screen_position.title = 0;
        screen_position.AlgoScreen = 0;
        screen_position.RadiconScreen = 0;
        screen_position.EntireScreen = 0;
        screen_position.GroupScreen = 0;
        screen_position.ImageScreen = 1;
        screen_position.TestScreen = 0;
        screen_position.makeREDGroupScreen = 0;
        //EntireData.GroupAll_reddata_param_count = 0;
    }

    public void Test_Click_Button()
    {
        //Debug.Log("Payment");
        PanelWalls.transform.localPosition = new Vector2(-18000.0f, 0.0f);
        screen_position.title = 0;
        screen_position.AlgoScreen = 0;
        screen_position.RadiconScreen = 0;
        screen_position.EntireScreen = 0;
        screen_position.GroupScreen = 0;
        screen_position.ImageScreen = 0;
        screen_position.TestScreen = 1;
        screen_position.makeREDGroupScreen = 0;
        //EntireData.GroupAll_reddata_param_count = 0;
    }

    public void makeREDGroup_Click_Button()
    {
        //Debug.Log("Payment");
        PanelWalls.transform.localPosition = new Vector2(-21000.0f, 0.0f);
        screen_position.title = 0;
        screen_position.AlgoScreen = 0;
        screen_position.RadiconScreen = 0;
        screen_position.EntireScreen = 0;
        screen_position.GroupScreen = 0;
        screen_position.ImageScreen = 0;
        screen_position.TestScreen = 0;
        screen_position.makeREDGroupScreen = 1;
        //EntireData.GroupAll_reddata_param_count = 0;
    }
}
