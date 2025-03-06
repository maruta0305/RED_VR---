using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SaveTest : MonoBehaviour
{
    public InputActionReference rightHandGripAction; // InputActionReference���C���X�y�N�^�[�Őݒ�
    public Text displayText; // UI��Text�R���|�[�l���g���C���X�y�N�^�[�Őݒ�

    private string csvFilePath;

    void Start()
    {
        // CSV�t�@�C���̃p�X��ݒ�
        csvFilePath = Path.Combine(Application.persistentDataPath, "data.csv");

        // �t�@�C�������݂��Ȃ��ꍇ�́A�w�b�_�[����������
        if (!File.Exists(csvFilePath))
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(csvFilePath))
                {
                    sw.WriteLine("Timestamp,DisplayedText");
                }
            }
            catch (IOException e)
            {
                Debug.LogError("Error writing to CSV: " + e.Message);
            }
        }
    }

    void Update()
    {
        // �O���b�v�{�^���������ꂽ�����`�F�b�N
        if (rightHandGripAction.action.WasPressedThisFrame())
        {
            Debug.Log("Grip button pressed!"); // �f�o�b�O���O
            SaveData();
        }
    }

    void SaveData()
    {
        // ���݂̃^�C���X�^���v���擾
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Text�R���|�[�l���g�̓��e���擾
        string displayedText = displayText.text;

        // �f�o�b�O���O
        Debug.Log("Displayed Text: " + displayedText);

        // CSV�ɏ������ރf�[�^���\�z
        string data = $"{timestamp},\"{displayedText}\""; // �J���}���܂ޏꍇ�̂��߂ɃN�H�[�g�ň͂�

        // CSV�t�@�C���Ƀf�[�^��ǉ�
        try
        {
            using (StreamWriter sw = new StreamWriter(csvFilePath, true))
            {
                sw.WriteLine(data);
            }
            Debug.Log("Data saved: " + data);
        }
        catch (IOException e)
        {
            Debug.LogError("Error writing to CSV: " + e.Message);
        }
    }
}
