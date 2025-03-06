using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public GameObject targetObject; // アタッチする対象のオブジェクト

    void Update()
    {
        if (targetObject != null)
        {
            // ターゲットオブジェクトの位置を取得
            Vector3 targetPosition = targetObject.transform.position;

            // 現在のオブジェクトをターゲットのX, Z座標に移動させる
            transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        }
    }
}
