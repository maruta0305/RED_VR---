using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggle
{
    public static int toggle_trigger = 0;
}

public class Select_Toggle : MonoBehaviour
{
    private Image SelectRED_Connect;
    private RectTransform handle_SelectRED;

    int first_count = 0;

    /// <summary>
    /// トグルの値
    /// </summary>
    [NonSerialized] public bool Value_SelectRED;

    private float handlePosX_SelectRED;
    private Sequence sequence_SelectRED;


    private static readonly Color OFF_BG_COLOR = new Color(255.0f, 255.0f, 255.0f);
    private static readonly Color ON_BG_COLOR = new Color(0.2f, 0.84f, 0.3f);

    private const float SWITCH_DURATION = 0.36f;

    // Start is called before the first frame update
    void Start()
    {
        // Imageコンポーネントを取得
        SelectRED_Connect = this.transform.Find("Background_Image").GetComponent<Image>();
        // RectTransformコンポーネントを取得
        handle_SelectRED = this.transform.Find("Handle_Image").GetComponent<RectTransform>();

        handlePosX_SelectRED = Mathf.Abs(handle_SelectRED.anchoredPosition.x);
        UpdateToggle_SelectRED(0);

    }

    // Update is called once per frame
    void Update()
    {
        //if (Clone_RED_IP.IPButton_trigger == 1)
        //{
        //    //Debug.Log("koko1");
        //    //// 親オブジェクトの名前を取得
        //    //string parentObjectName = transform.parent.gameObject.name;

        //    //Debug.Log("親オブジェクトの名前: " + parentObjectName);
        //    // Imageコンポーネントを取得
        //    SelectRED_Connect = this.transform.Find("Background_Image").GetComponent<Image>();
        //    // RectTransformコンポーネントを取得
        //    handle_SelectRED = this.transform.Find("Handle_Image").GetComponent<RectTransform>();

        //    Debug.Log("koko2" + SelectRED_Connect);

        //    //if (first_count == 0)
        //    //{
        //    handlePosX_SelectRED = Mathf.Abs(handle_SelectRED.anchoredPosition.x);
        //    UpdateToggle_SelectRED(0);
        //    //}

        //    //SwitchToggle_SelectRED();
        //    Clone_RED_IP.IPButton_trigger = 0;
        //    first_count = 1;
        //}
    }

    ///////////////  Algorithm Onの方の処理  //////////////////////////////
    public void SwitchToggle_SelectRED()
    {
        Value_SelectRED = !Value_SelectRED;
        UpdateToggle_SelectRED(SWITCH_DURATION);
        Debug.Log("koko2");
        toggle.toggle_trigger = 1;
    }
    private void UpdateToggle_SelectRED(float duration)
    {
        var bgColor = Value_SelectRED ? ON_BG_COLOR : OFF_BG_COLOR;
        var handleDestX = Value_SelectRED ? handlePosX_SelectRED : -handlePosX_SelectRED;

        sequence_SelectRED?.Complete();
        sequence_SelectRED = DOTween.Sequence();
        sequence_SelectRED.Append(object_1.SelectRED_Connect.DOColor(bgColor, duration))
            .Join(object_2.handle_SelectRED.DOAnchorPosX(handleDestX, duration / 2));
    }
    /////////////////////////////////////////////////////////////////////
    
}
