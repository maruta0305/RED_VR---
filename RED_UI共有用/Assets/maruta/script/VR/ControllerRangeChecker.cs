using UnityEngine;
using UnityEngine.UI;

public class ControllerRangeChecker : MonoBehaviour
{
    public Button targetButton; // 対象のボタン
    private bool isInRange = false; // コントローラーが範囲内にいるかどうか

    void Update()
    {
        // コントローラーが範囲内で、ボタンがクリックされたとき
        if (isInRange) // "Fire1"は通常、左クリックまたはゲームパッドのボタンにマップされています
        {
            Debug.Log("true");
            targetButton.onClick.Invoke(); // ボタンのクリックをシミュレート
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // コントローラーが範囲に入ったとき
        if (other.CompareTag("Controller")) // コントローラーにタグを付けておくことを前提
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // コントローラーが範囲から出たとき
        if (other.CompareTag("Controller"))
        {
            isInRange = false;
        }
    }
}

