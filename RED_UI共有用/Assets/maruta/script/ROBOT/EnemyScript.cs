using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    public Transform Target;
    public Transform random;
    private NavMeshAgent agent;
    private bool isMoving = false;
    private float speed;

    public TMP_InputField inputFieldInstance; // TMP_InputFieldに変更

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = 1.0f; // 初期速度を設定
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            agent.destination = random.position;

            // Speedの取得
            if (inputFieldInstance != null)
            {
                string inputValue = inputFieldInstance.text; // textプロパティを使用
                if (float.TryParse(inputValue, out float parsedValue))
                {
                    speed = parsedValue; // 入力値を float に変換して speed に代入
                }
                else
                {
                    speed = 1.0f; // パースに失敗した場合のデフォルト値
                }
            }
            agent.speed = speed;
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
        agent.velocity = Vector3.zero; // 停止時に速度をゼロに設定
    }
}
