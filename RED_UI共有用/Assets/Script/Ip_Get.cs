using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_RED_IP
{
    public static List<string> Select_RED_IP_Entire = new List<string>();
    public static List<string> Select_RED_IP_Group = new List<string>();
    public static int IPButton_trigger = 0;
}

public class Ip_Get : MonoBehaviour
{
    string IP_textValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(toggle.toggle_trigger == 1)
        //{
        //    Ipaddress_Button();
        //    toggle.toggle_trigger = 0;
        //}

        if (screen_position.EntireScreen == 0)
        {
            Clone_RED_IP.Select_RED_IP_Entire.Clear();
        }
        else if (screen_position.GroupScreen == 0)
        {
            Clone_RED_IP.Select_RED_IP_Group.Clear();
        }
    }

    public void Ipaddress_Button()
    {
        Clone_RED_IP.IPButton_trigger = 1;
        // Textコンポーネントからテキストを取得
        IP_textValue = this.GetComponent<Text>().text;
        if(screen_position.EntireScreen == 1)
        {
            if (Clone_RED_IP.Select_RED_IP_Entire.Contains(IP_textValue))//含まれてる
            {
                Clone_RED_IP.Select_RED_IP_Entire.Remove(IP_textValue);
            }
            else//含まれてない
            {
                Clone_RED_IP.Select_RED_IP_Entire.Add(IP_textValue);
            }

            for (int i = 0; i < Clone_RED_IP.Select_RED_IP_Entire.Count; i++)
            {
                Debug.Log("Clone_RED_IP_Entire: " + Clone_RED_IP.Select_RED_IP_Entire[i]);
            }
        }
        else if (screen_position.GroupScreen == 1)
        {
            Debug.Log("Push!!!!!!!!");
            if (Clone_RED_IP.Select_RED_IP_Group.Contains(IP_textValue))//含まれてる
            {
                Clone_RED_IP.Select_RED_IP_Group.Remove(IP_textValue);
            }
            else//含まれてない
            {
                Clone_RED_IP.Select_RED_IP_Group.Add(IP_textValue);
            }

            for (int i = 0; i < Clone_RED_IP.Select_RED_IP_Group.Count; i++)
            {
                Debug.Log("Clone_RED_IP: " + Clone_RED_IP.Select_RED_IP_Group[i]);
            }
        }
        

        title_data.RED_IP_title = "SelectRED";
        //Clone_RED_IP.IPButton_trigger = 1;
    }
}
