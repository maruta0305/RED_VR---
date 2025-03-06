using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class ImageData
{
    public static int ImageRobotList_Trigger = 0;
    public static int ObtainingImage_Trigger = 0;
    public static int ImageGetBaseURL_Trigger = 0;
    public static string ImageBaseURL = "";
    public static int ImageList_RobotIPSel_Trigger = 0;
    public static string ImageList_RobotIPSel_RobotIP = "";
    public static int ImageList_ImageSel_Trigger = 0;
    public static string ImageList_ImageSel_ImageName = "";
}

public class ImageScreen : MonoBehaviour
{
    //ここの変数でDavidさんから送られてくる画像のURLを指定することで画像の表示が可能
    private string Image_URI_Floor = "https://www.nsozai.jp/photos/2014/04/21/img/DSC_0103p.jpg";
    private string Image_URI_Ceiling = "https://www.nsozai.jp/photos/2014/04/21/img/DSC_0103p.jpg";

    [SerializeField] private RawImage Ceiling_image;
    [SerializeField] private RawImage Floor_image;

    public GameObject RobotImage_List_Floor;
    public GameObject RobotImage_List_Ceiling;
    Dropdown ddtmp_RobotImage_List_Floor;
    Dropdown ddtmp_RobotImage_List_Ceiling;
    public GameObject Robot_List;
    Dropdown ddtmp_Robot_List;
    string Robot_IP = null;

    // Start is called before the first frame update
    void Start()
    {

        ddtmp_Robot_List = GameObject.Find("Select_RobotIP_Dropdown_images").GetComponent<Dropdown>();
        Debug.Log("Image start");

    }

    // Update is called once per frame
    void Update()
    {

//        Debug.Log("Image -" + ImageData.ImageBaseURL + "- tcp:" + tcpip_data.server_connect_command.ToString());

        if ( (ImageData.ImageBaseURL == "") && (tcpip_data.server_connect_command == 1) )
        {
            ImageData.ImageGetBaseURL_Trigger = 1;
            Debug.Log("ImageGetBaseURL trigger set");
            return;
        }

        if (tcpip_data.receive_count == 1 && screen_position.ImageScreen == 1)
        {
            // kammide
            if (ImageData.ImageGetBaseURL_Trigger == 1)
            {
                ImageData.ImageGetBaseURL_Trigger = 0;
                ImageData.ImageBaseURL = tcpip_data.RED_data;
                Debug.Log("ImageGetBaseURL trigger end. Base URL: " + ImageData.ImageBaseURL);
            }

            if (ImageData.ImageRobotList_Trigger == 1)
            {
                dropdown_Robot_List(tcpip_data.RED_data);
                ImageData.ImageRobotList_Trigger = 0;
                Debug.Log("ImageRobotList trigger end. Data: " + tcpip_data.RED_data);
            }

            if (ImageData.ImageList_RobotIPSel_Trigger == 1)
            {
                dropdown_RobotImage_List(tcpip_data.RED_data);
                ImageData.ImageList_RobotIPSel_Trigger = 0;
                Debug.Log("ImageRobotIPSel trigger end. Data: " + tcpip_data.RED_data);
            }

            tcpip_data.receive_count = 0;
        }
    }

    public void dropdown_Robot_List(string data_RobotList)
    {
        List<string> RobotList = new List<string>();

        string[] newline = { "\r\n" };
        string[] comma = { "," };
        string[] data_size = data_RobotList.Split(newline, StringSplitOptions.None);

        RobotList.Add("Select RED robot");
        for (int j = 0; j < data_size.Length; j++)
        {
            //Optionsに表示する文字列をリストに追加
            string[] robotlistline = data_size[j].Split(comma, StringSplitOptions.None);
            RobotList.Add(robotlistline[0]);
//            ImageList.Add(data_size[j]);
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        //ddtmp_Robot_List = Robot_List.GetComponent<Dropdown>();

        //一度すべてのOptionsをクリア
        ddtmp_Robot_List.ClearOptions();

        //リストを追加
        ddtmp_Robot_List.AddOptions(RobotList);
    }


    public void dropdown_RobotImage_List(string data_ImageList)
    {
        List<string> ImageList_Floor = new List<string>();
        List<string> ImageList_Ceiling = new List<string>();

        string[] newline = { "\r\n" };
        string[] underscore = { "_" };
        string[] allimageslist = data_ImageList.Split(newline, StringSplitOptions.None);

        ImageList_Floor.Add("Select Floor Image");
        ImageList_Ceiling.Add("Select Ceiling Image");
        // start from 1, not 0, to skip the first line which is not a filename, just the total count for the images
        for (int j = 1; j < allimageslist.Length; j++)
        {
            //Optionsに表示する文字列をリストに追加
            string[] imagelistline = allimageslist[j].Split(underscore, StringSplitOptions.None);
            if (imagelistline[0] == "FloorImage")
            {
                ImageList_Floor.Add(allimageslist[j]);
            } else {
                ImageList_Ceiling.Add(allimageslist[j]);
            }
        }

        if (ImageList_Floor.Count == 1)
        {
            ImageList_Floor.Clear();
            ImageList_Floor.Add("Not available");
        }

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp_RobotImage_List_Floor = RobotImage_List_Floor.GetComponent<Dropdown>();
        //一度すべてのOptionsをクリア
        ddtmp_RobotImage_List_Floor.ClearOptions();
        //リストを追加
        ddtmp_RobotImage_List_Floor.AddOptions(ImageList_Floor);

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        ddtmp_RobotImage_List_Ceiling = RobotImage_List_Ceiling.GetComponent<Dropdown>();
        //一度すべてのOptionsをクリア
        ddtmp_RobotImage_List_Ceiling.ClearOptions();
        //リストを追加
        ddtmp_RobotImage_List_Ceiling.AddOptions(ImageList_Ceiling);
    }


    public void dropdown_RobotList_call(int num)
    {
        Robot_IP = ddtmp_Robot_List.options[ddtmp_Robot_List.value].text;
        ImageData.ImageList_RobotIPSel_Trigger = 1;
        ImageData.ImageList_RobotIPSel_RobotIP = Robot_IP;
        Debug.Log("Robot List - User selected Robot IP: " + Robot_IP);
    }

    public void dropdown_RobotImageList_Floor_call(int num)
    {
        string Selected_Image_Name = ddtmp_RobotImage_List_Floor.options[ddtmp_RobotImage_List_Floor.value].text;
//        ImageData.ImageList_ImageSel_Trigger_Floor = 1;
//        ImageData.ImageList_ImageSel_Name_Floor = Selected_Image_Name;
        Image_URI_Floor = ImageData.ImageBaseURL + Selected_Image_Name;
        StartCoroutine(FloorImage_view());
        Debug.Log("User selected Floor Image: " + Selected_Image_Name + "num: " + num.ToString() + " Full URL:" + Image_URI_Floor);
    }

    public void dropdown_RobotImageList_Ceiling_call(int num)
    {
        string Selected_Image_Name = ddtmp_RobotImage_List_Ceiling.options[ddtmp_RobotImage_List_Ceiling.value].text;
//        ImageData.ImageList_ImageSel_Trigger = 1;
//        ImageData.ImageList_ImageSel_Name = Selected_Image_Name;
        Image_URI_Ceiling = ImageData.ImageBaseURL + Selected_Image_Name;
        StartCoroutine(CeilingImage_view());
        Debug.Log("User selected Ceiling Image: " + Selected_Image_Name + " num: " + num.ToString() + " Full URL:" + Image_URI_Ceiling);
    }

    IEnumerator CeilingImage_view()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Image_URI_Ceiling);

        //画像を取得できるまで待つ
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //取得した画像のテクスチャをRawImageのテクスチャに張り付ける
            Ceiling_image.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    IEnumerator FloorImage_view()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Image_URI_Floor);

        //画像を取得できるまで待つ
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //取得した画像のテクスチャをRawImageのテクスチャに張り付ける
            Floor_image.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    public void ObtainingImage_Button()
    {
        //送信部分を書いて、イメージを可視化する関数をUpdate関数で実行する
        StartCoroutine(CeilingImage_view());
        StartCoroutine(FloorImage_view());
    }

    public void ImageList_Button()
    {
 //       ImageData.ImageList_Trigger = 1;
        Debug.Log("ImageList Button pressed");
    }

}
