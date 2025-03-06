using UnityEngine;
using UnityEngine.InputSystem;

public class CameraChange : MonoBehaviour
{
    public GameObject mainCamera1;
    //public GameObject mainCamera1left;
    //public GameObject mainCamera1right;
    //public GameObject mainCamera2;
    public GameObject subCamera1;
    //public GameObject subCamera2;
    //public GameObject subCamera1left;
    //public GameObject subCamera1right;
    public InputActionReference rightHandGripAction; // InputActionReferenceをインスペクターで設定

    //private bool isCameraActive = false; // カメラの状態を追跡

    void Start()
    {

        subCamera1.SetActive(false);
        //subCamera2.SetActive(false);
        //subCamera1left.SetActive(false);
        //subCamera1right.SetActive(false);
    }

    void Update()
    {
        if (rightHandGripAction.action.WasPressedThisFrame()) // グリップボタンが押されたかをチェック
        {
            ChangeCamera();
        }
    }

    public void ChangeCamera()
    {
        bool anyMainCameraActive = mainCamera1.activeSelf;
        if (anyMainCameraActive)
        {
            //mainCamera1left.SetActive(false);
            //mainCamera1right.SetActive(false);
            mainCamera1.SetActive(false);
            //mainCamera2.SetActive(false);
            subCamera1.SetActive(true);
            //subCamera2.SetActive(false);
            //subCamera1left.SetActive(true);
            //subCamera1right.SetActive(true);
        }
        else
        {
            //mainCamera1left.SetActive(true);
            //mainCamera1right.SetActive(true);
            mainCamera1.SetActive(true);
            //mainCamera2.SetActive(true);
            subCamera1.SetActive(false);
            //subCamera2.SetActive(true);
            //subCamera1left.SetActive(false);
            //subCamera1right.SetActive(false);
        }
    }
}

