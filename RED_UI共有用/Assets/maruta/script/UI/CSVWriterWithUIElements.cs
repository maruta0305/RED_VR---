using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CSVWriterWithUIElements : MonoBehaviour
{
    public string fileName = "output.csv";

    // �{�^���A���b�Z�[�W�AInputField�ADropdown���ꊇ�Ǘ�����N���X
    [System.Serializable]
    public class UIElement
    {
        public Button button;           // �{�^��
        public string message;          // ���b�Z�[�W
        public InputField inputField;   // InputField
        public Dropdown dropdown;       // Dropdown
    }

    // UI�v�f���Ǘ�����z��
    public UIElement[] uiElements;

    void Start()
    {
        // �e�{�^���Ƀ��X�i�[��ǉ�
        foreach (UIElement element in uiElements)
        {
            element.button.onClick.AddListener(() => WriteToCSV(element));
        }
    }

    void WriteToCSV(UIElement element)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            // CSV�ɋL�^���郁�b�Z�[�W���\�z
            string csvCell = $"\"{element.message}"; // �{�^���̃��b�Z�[�W

            // InputField�̒l��ǉ��i���͂�����΁j
            if (element.inputField != null && !string.IsNullOrEmpty(element.inputField.text))
            {
                csvCell += $" {element.inputField.text}";
            }

            // Dropdown�̑I�����ʂ�ǉ��i�I����������΁j
            if (element.dropdown != null && element.dropdown.options.Count > 0)
            {
                csvCell += $" {element.dropdown.options[element.dropdown.value].text}";
            }

            csvCell += "\""; // �_�u���N�H�[�g�ŕ���

            // CSV�t�@�C���ɒǋL
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(csvCell);
            }

            Debug.Log($"CSV�ɏ������݊���: {csvCell}");
        }
        catch (IOException ex)
        {
            Debug.LogError($"CSV�t�@�C���̏������ݒ��ɃG���[���������܂���: {ex.Message}");
        }
    }
}
