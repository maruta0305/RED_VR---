using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ControllerPosition : MonoBehaviour
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

    private List<string[]> dataList = new List<string[]>(); // �f�[�^��ۑ����郊�X�g
    private float saveInterval = 1f; // �ۑ��̊Ԋu
    private float nextSaveTime = 0f;

    void Start()
    {
        // ���W���𐶐�
        xAxis = Instantiate(axisPrefab, hmd.position, Quaternion.Euler(0, 0, 0));
        yAxis = Instantiate(axisPrefab, hmd.position, Quaternion.Euler(0, 90, 0));
        zAxis = Instantiate(axisPrefab, hmd.position, Quaternion.Euler(90, 0, 0));

        // �F��ݒ�
        xAxis.GetComponent<Renderer>().material.color = Color.red;   // X��
        yAxis.GetComponent<Renderer>().material.color = Color.green; // Y��
        zAxis.GetComponent<Renderer>().material.color = Color.blue;  // Z��
    }

    void Update()
    {
        // HMD�̈ʒu�Ɋ�Â��č��W�����X�V
        Vector3 hmdPosition = hmd.position;

        xAxis.transform.position = hmdPosition;
        yAxis.transform.position = hmdPosition;
        zAxis.transform.position = hmdPosition;

        // HMD�̉�]�ɍ��킹�č��W������]
        Quaternion hmdRotation = hmd.rotation;
        xAxis.transform.rotation = hmdRotation;
        yAxis.transform.rotation = hmdRotation;
        zAxis.transform.rotation = hmdRotation;

        // �R���g���[���[�̑��Έʒu��\��
        Vector3 leftRelativePosition = leftController.position - hmdPosition;
        Vector3 rightRelativePosition = rightController.position - hmdPosition;

        // �e�L�X�g�ɑ��Έʒu��\��
        leftControllerText.text = $"Left Controller Position: {leftRelativePosition}";
        rightControllerText.text = $"Right Controller Position: {rightRelativePosition}";

        // ���Ԋu��CSV�ɕۑ�
        SaveToCSV(rightRelativePosition);
        /*if (Time.time >= nextSaveTime)
        {
            SaveToCSV(rightRelativePosition);
            nextSaveTime = Time.time + saveInterval;
        }*/
    }

    private void SaveToCSV(Vector3 rightRelativePosition)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Positiondata.csv");

        // ���݂̎��Ԃ��擾
        string currentTime = Time.time.ToString("F2"); // �����_�ȉ�2���܂�

        // �E��R���g���[���[�̑��Έʒu��CSV�ɒǉ�
        string[] data = new string[]
        {
            currentTime,
            rightRelativePosition.x.ToString(),
            rightRelativePosition.y.ToString(),
            rightRelativePosition.z.ToString()
        };

        // CSV�t�@�C�����쐬�܂��͒ǋL
        using (StreamWriter writer = new StreamWriter(filePath, true, System.Text.Encoding.UTF8))
        {
            writer.WriteLine(string.Join(",", data));
        }

        Debug.Log("Data saved to: " + filePath);
    }
}


