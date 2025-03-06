using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControllerRotation : MonoBehaviour
{
    public Transform hmd; // HMDのTransform
    public Transform leftController; // 左手コントローラーのTransform
    public Transform rightController; // 右手コントローラーのTransform

    public GameObject axisPrefab; // 座標軸のPrefab
    private GameObject xAxis;
    private GameObject yAxis;
    private GameObject zAxis;

    // UI用のTextオブジェクト
    public Text leftControllerText; // 左手コントローラーの相対位置を表示するText
    public Text rightControllerText; // 右手コントローラーの相対位置を表示するText
    public Text hmdRotationText; // HMDの回転を表示するText

    // ボタンの参照
    public Button myButton; // 動作させるボタン

    public GameObject UI; // 表示するUIオブジェクト
    public GameObject ButtonUI; // 表示するButtonUIオブジェクト
    public GameObject LogUI;
    public GameObject Warp;//ワープ用UI

    public InputActionReference rightHandGripAction; // InputActionReferenceをインスペクターで設定
    public InputActionReference leftHandGripAction;//ワープボタン出すよう

    void Start()
    {
        // 座標軸を生成
        xAxis = Instantiate(axisPrefab, hmd.position, Quaternion.identity);
        yAxis = Instantiate(axisPrefab, hmd.position, Quaternion.identity);
        zAxis = Instantiate(axisPrefab, hmd.position, Quaternion.identity);

        // 色を設定
        xAxis.GetComponent<Renderer>().material.color = Color.red;   // X軸
        yAxis.GetComponent<Renderer>().material.color = Color.green; // Y軸
        zAxis.GetComponent<Renderer>().material.color = Color.blue;  // Z軸

        ButtonUI.SetActive(false); // 初期状態でButtonUIを非表示にする
        LogUI.SetActive(false);
        UI.SetActive(false); // 初期状態でUIを非表示にする
        Warp.SetActive(false);
    }

    void Update()
    {
        // HMDの位置に基づいて座標軸を更新
        Vector3 hmdPosition = hmd.position;
        xAxis.transform.position = hmdPosition;
        yAxis.transform.position = hmdPosition;
        zAxis.transform.position = hmdPosition;

        // HMDの回転に合わせて座標軸を設定
        Quaternion hmdRotation = hmd.rotation;
        xAxis.transform.rotation = hmdRotation * Quaternion.Euler(0, 0, 0);
        yAxis.transform.rotation = hmdRotation * Quaternion.Euler(0, 90, 0);
        zAxis.transform.rotation = hmdRotation * Quaternion.Euler(90, 0, 0);

        // コントローラーの相対位置を計算
        Vector3 leftRelativePosition = Quaternion.Inverse(hmdRotation) * (leftController.position - hmdPosition);
        Vector3 rightRelativePosition = Quaternion.Inverse(hmdRotation) * (rightController.position - hmdPosition);

        // テキストに相対位置を表示
        leftControllerText.text = $"Left Controller Position: {leftRelativePosition}";
        rightControllerText.text = $"Right Controller Position: {rightRelativePosition}";

        // HMDの回転角をオイラー角として取得
        Vector3 hmdEulerAngles = hmd.rotation.eulerAngles;
        hmdRotationText.text = $"HMD Rotation - X: {hmdEulerAngles.x:F2}, Y: {hmdEulerAngles.y:F2}, Z: {hmdEulerAngles.z:F2}";

        // グリップボタンが押されたかをチェック
        if (rightHandGripAction.action.WasPressedThisFrame())
        {
            ChangeUI();
        }
        if (leftHandGripAction.action.WasPressedThisFrame())
        {
            ChangeWarpUI();
        }

    }

    void ChangeUI()
    {
        // HMDの位置を取得
        Vector3 hmdPosition = hmd.position;

        // 右コントローラーの位置を取得
        Vector3 rightControllerPosition = rightController.position;

        // HMDから右コントローラーまでの距離を計算
        float distanceToRightController = Vector3.Distance(hmdPosition, rightControllerPosition);

        // UIの位置をHMDのz方向に、右コントローラーとの距離を考慮して設定
        UI.transform.position = hmdPosition + hmd.forward * distanceToRightController*2;

        // ButtonUIの位置も右コントローラーを基準に設定
        ButtonUI.transform.position = hmdPosition + hmd.forward * distanceToRightController * 0.1f + Vector3.down * 0.25f- hmd.right * distanceToRightController * 2;

        LogUI.transform.position = hmdPosition + hmd.forward * distanceToRightController * 1+hmd.right* distanceToRightController * 2;
        // UIが常にHMDの方向を向くように設定
        UI.transform.LookAt(hmd);
        ButtonUI.transform.LookAt(hmd);
        LogUI.transform.LookAt(hmd);

        // UIとButtonUIの状態をトグル
        UI.SetActive(!UI.activeSelf);
        ButtonUI.SetActive(!ButtonUI.activeSelf);
        LogUI.SetActive(!LogUI.activeSelf);
    }
    void ChangeWarpUI()
    {
        Vector3 hmdPosition = hmd.position;

        // 右コントローラーの位置を取得
        Vector3 leftControllerPosition = leftController.position;

        // HMDから右コントローラーまでの距離を計算
        float distanceToLeftController = Vector3.Distance(hmdPosition, leftControllerPosition);

        // UIの位置をHMDのz方向に、右コントローラーとの距離を考慮して設定
        Warp.transform.position = hmdPosition + hmd.forward * distanceToLeftController * 2;
        
        // UIが常にHMDの方向を向くように設定
        Warp.transform.LookAt(hmd);
        Warp.SetActive(!Warp.activeSelf);
    }
}