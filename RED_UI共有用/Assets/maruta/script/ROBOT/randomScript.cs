using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RandomScript : MonoBehaviour
{
    public TMP_InputField inputField1;
    public TMP_InputField inputField2;
    public TMP_InputField inputField3;
    public TMP_InputField inputField4;
    public TMP_InputField inputField5;

    private Coroutine warpCoroutine;
    private float defaultTime = 6f; // デフォルトの待機時間
    private float range = 4;
    public Transform target;

    void Start()
    {
        // InputFieldの値が変更されたときにイベントを購読する
        if (inputField1 != null)
        {
            inputField1.onValueChanged.AddListener(delegate { OnInputValueChanged(); });
        }
        if (inputField2 != null)
        {
            inputField2.onValueChanged.AddListener(delegate { OnInputValueChanged(); });
        }
        if (inputField3 != null)
        {
            inputField3.onValueChanged.AddListener(delegate { OnInputValueChanged(); });
        }
        if (inputField4 != null)
        {
            inputField4.onValueChanged.AddListener(delegate { OnInputValueChanged(); });
        }
        if (inputField5 != null)
        {
            inputField5.onValueChanged.AddListener(delegate { OnInputValueChanged(); });
        }
    }

    // InputFieldの値が変更されたときに呼ばれるメソッド
    public void OnInputValueChanged()
    {
        // ワープ中であればキャンセルして、新しい位置からワープを再開する
        if (warpCoroutine != null)
        {
            StopCoroutine(warpCoroutine);
        }
        //StartWarp();
    }

    // ワープを開始するメソッド
    private void StartWarp()
    {
        warpCoroutine = StartCoroutine(Warp());
    }

    public void Follow()
    {
        StartCoroutine(NoAuto());
        //StopCoroutine(Warp());
    }

    private IEnumerator NoAuto()
    {
        float time = defaultTime;
        for (int i = 1; i < 1000; i++)
        {
            Vector3 targetPosition = target.position;
            float posX = Random.Range(target.position.x - range, target.position.x + range);
            float posZ = Random.Range(target.position.z - range, target.position.z + range);
            transform.position = new Vector3(posX, 0, posZ);
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Warp()
    {
        float x = 0;
        float z = 0;
        float radius = 8;
        float time = defaultTime;

        if (inputField1 != null && float.TryParse(inputField1.text, out float parsedValue1))
        {
            x = parsedValue1;
        }
        if (inputField2 != null && float.TryParse(inputField2.text, out float parsedValue2))
        {
            z = parsedValue2;
        }
        if (inputField3 != null && float.TryParse(inputField3.text, out float parsedValue3))
        {
            radius = parsedValue3;
        }
        if (inputField4 != null && float.TryParse(inputField4.text, out float parsedValue4))
        {
            time = parsedValue4;
        }
        if (inputField5 != null && float.TryParse(inputField5.text, out float parsedValue5))
        {
            range = parsedValue5;
        }

        for (int i = 1; i < 1000; i++)
        {
            if (i % 2 == 0)
            {
                // 偶数の場合
                for (int l = 1; l <= i; l++)
                {
                    x -= radius;
                    for (int n = 0; n < 3; n++)
                    {
                        float posX = Random.Range(x - range, x + range);
                        float posZ = Random.Range(z - range, z + range);
                        transform.position = new Vector3(posX, 0, posZ);
                        yield return new WaitForSeconds(time); // time 変数で待機時間を指定
                    }
                }
                for (int m = 1; m <= i; m++)
                {
                    z -= radius;
                    for (int n = 0; n < 3; n++)
                    {
                        float posX = Random.Range(x - range, x + range);
                        float posZ = Random.Range(z - range, z + range);
                        transform.position = new Vector3(posX, 0, posZ);
                        yield return new WaitForSeconds(time); // time 変数で待機時間を指定
                    }
                }
            }
            else
            {
                // 奇数の場合
                for (int j = 1; j <= i; j++)
                {
                    x += radius;
                    for (int n = 0; n < 3; n++)
                    {
                        float posX = Random.Range(x - range, x + range);
                        float posZ = Random.Range(z - range, z + range);
                        transform.position = new Vector3(posX, 0, posZ);
                        yield return new WaitForSeconds(time); // time 変数で待機時間を指定
                    }
                }
                for (int k = 1; k <= i; k++)
                {
                    z += radius;
                    for (int n = 0; n < 3; n++)
                    {
                        float posX = Random.Range(x - range, x + range);
                        float posZ = Random.Range(z - range, z + range);
                        transform.position = new Vector3(posX, 0, posZ);
                        yield return new WaitForSeconds(time); // time 変数で待機時間を指定
                    }
                }
            }
        }
    }
}

