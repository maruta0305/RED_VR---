using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControllerRotation : MonoBehaviour
{
    public Transform hmd; // HMD��Transform
    public Transform leftController; // ����R���g���[���[��Transform
    public Transform rightController; // �E��R���g���[���[��Transform

    public GameObject axisPrefab; // ���W����Prefab
    private GameObject xAxis;
    private GameObject yAxis;
    private GameObject zAxis;

    // UI�p��Text�I�u�W�F�N�g
    public Text leftControllerText; // ����R���g���[���[�̑��Έʒu��\������Text
    public Text rightControllerText; // �E��R���g���[���[�̑��Έʒu��\������Text
    public Text hmdRotationText; // HMD�̉�]��\������Text

    // �{�^���̎Q��
    public Button myButton; // ���삳����{�^��

    public GameObject UI; // �\������UI�I�u�W�F�N�g
    public GameObject ButtonUI; // �\������ButtonUI�I�u�W�F�N�g
    public GameObject LogUI;
    public GameObject Warp;//���[�v�pUI

    public InputActionReference rightHandGripAction; // InputActionReference���C���X�y�N�^�[�Őݒ�
    public InputActionReference leftHandGripAction;//���[�v�{�^���o���悤

    void Start()
    {
        // ���W���𐶐�
        xAxis = Instantiate(axisPrefab, hmd.position, Quaternion.identity);
        yAxis = Instantiate(axisPrefab, hmd.position, Quaternion.identity);
        zAxis = Instantiate(axisPrefab, hmd.position, Quaternion.identity);

        // �F��ݒ�
        xAxis.GetComponent<Renderer>().material.color = Color.red;   // X��
        yAxis.GetComponent<Renderer>().material.color = Color.green; // Y��
        zAxis.GetComponent<Renderer>().material.color = Color.blue;  // Z��

        ButtonUI.SetActive(false); // ������Ԃ�ButtonUI���\���ɂ���
        LogUI.SetActive(false);
        UI.SetActive(false); // ������Ԃ�UI���\���ɂ���
        Warp.SetActive(false);
    }

    void Update()
    {
        // HMD�̈ʒu�Ɋ�Â��č��W�����X�V
        Vector3 hmdPosition = hmd.position;
        xAxis.transform.position = hmdPosition;
        yAxis.transform.position = hmdPosition;
        zAxis.transform.position = hmdPosition;

        // HMD�̉�]�ɍ��킹�č��W����ݒ�
        Quaternion hmdRotation = hmd.rotation;
        xAxis.transform.rotation = hmdRotation * Quaternion.Euler(0, 0, 0);
        yAxis.transform.rotation = hmdRotation * Quaternion.Euler(0, 90, 0);
        zAxis.transform.rotation = hmdRotation * Quaternion.Euler(90, 0, 0);

        // �R���g���[���[�̑��Έʒu���v�Z
        Vector3 leftRelativePosition = Quaternion.Inverse(hmdRotation) * (leftController.position - hmdPosition);
        Vector3 rightRelativePosition = Quaternion.Inverse(hmdRotation) * (rightController.position - hmdPosition);

        // �e�L�X�g�ɑ��Έʒu��\��
        leftControllerText.text = $"Left Controller Position: {leftRelativePosition}";
        rightControllerText.text = $"Right Controller Position: {rightRelativePosition}";

        // HMD�̉�]�p���I�C���[�p�Ƃ��Ď擾
        Vector3 hmdEulerAngles = hmd.rotation.eulerAngles;
        hmdRotationText.text = $"HMD Rotation - X: {hmdEulerAngles.x:F2}, Y: {hmdEulerAngles.y:F2}, Z: {hmdEulerAngles.z:F2}";

        // �O���b�v�{�^���������ꂽ�����`�F�b�N
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
        // HMD�̈ʒu���擾
        Vector3 hmdPosition = hmd.position;

        // �E�R���g���[���[�̈ʒu���擾
        Vector3 rightControllerPosition = rightController.position;

        // HMD����E�R���g���[���[�܂ł̋������v�Z
        float distanceToRightController = Vector3.Distance(hmdPosition, rightControllerPosition);

        // UI�̈ʒu��HMD��z�����ɁA�E�R���g���[���[�Ƃ̋������l�����Đݒ�
        UI.transform.position = hmdPosition + hmd.forward * distanceToRightController*2;

        // ButtonUI�̈ʒu���E�R���g���[���[����ɐݒ�
        ButtonUI.transform.position = hmdPosition + hmd.forward * distanceToRightController * 0.1f + Vector3.down * 0.25f- hmd.right * distanceToRightController * 2;

        LogUI.transform.position = hmdPosition + hmd.forward * distanceToRightController * 1+hmd.right* distanceToRightController * 2;
        // UI�����HMD�̕����������悤�ɐݒ�
        UI.transform.LookAt(hmd);
        ButtonUI.transform.LookAt(hmd);
        LogUI.transform.LookAt(hmd);

        // UI��ButtonUI�̏�Ԃ��g�O��
        UI.SetActive(!UI.activeSelf);
        ButtonUI.SetActive(!ButtonUI.activeSelf);
        LogUI.SetActive(!LogUI.activeSelf);
    }
    void ChangeWarpUI()
    {
        Vector3 hmdPosition = hmd.position;

        // �E�R���g���[���[�̈ʒu���擾
        Vector3 leftControllerPosition = leftController.position;

        // HMD����E�R���g���[���[�܂ł̋������v�Z
        float distanceToLeftController = Vector3.Distance(hmdPosition, leftControllerPosition);

        // UI�̈ʒu��HMD��z�����ɁA�E�R���g���[���[�Ƃ̋������l�����Đݒ�
        Warp.transform.position = hmdPosition + hmd.forward * distanceToLeftController * 2;
        
        // UI�����HMD�̕����������悤�ɐݒ�
        Warp.transform.LookAt(hmd);
        Warp.SetActive(!Warp.activeSelf);
    }
}