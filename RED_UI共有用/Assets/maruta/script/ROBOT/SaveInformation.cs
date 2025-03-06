using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveInformation : MonoBehaviour
{
    public Dropdown dropdown1;
    public Dropdown dropdown2;
    private string selectedOption1;
    private string selectedOption2;

    // ExploreParam �N���X���`
    public class ExploreParam
    {
        public string name;  // 1���: ������
        public float value1;   // 6���: ����
        public float value2;   // 7���: ����

        // �R���X�g���N�^
        public ExploreParam(string name, int value1, int value2)
        {
            this.name = name;
            this.value1 = value1;
            this.value2 = value2;
        }
    }

    public static List<ExploreParam> exploreList = new List<ExploreParam>();


    private string groupsCSV;
    private string parametersCSV;

    // Start is called before the first frame update
    void Start()
    {
        // �h���b�v�_�E���̑I�����ύX���ꂽ���̏�����ݒ�
        dropdown1.onValueChanged.AddListener(OnDropdown1ValueChanged);
        dropdown2.onValueChanged.AddListener(OnDropdown2ValueChanged);

        // CSV�t�@�C����ǂݍ���
        groupsCSV = LoadCSV(Path.Combine(Application.persistentDataPath, "RED_groups.csv"));
        parametersCSV = LoadCSV(Path.Combine(Application.persistentDataPath, "RED_parameter_sets.csv"));

        // �ǂݍ��񂾓��e���f�o�b�O���O�ɕ\��
        if (!string.IsNullOrEmpty(groupsCSV))
        {
            Debug.Log("RED_groups.csv �̓��e������ɓǂݍ��܂�܂���:\n" + groupsCSV);
        }
        else
        {
            Debug.LogError("RED_groups.csv �̓ǂݍ��݂Ɏ��s���܂����B");
        }

        if (!string.IsNullOrEmpty(parametersCSV))
        {
            Debug.Log("RED_parameter_sets.csv �̓��e������ɓǂݍ��܂�܂���:\n" + parametersCSV);
        }
        else
        {
            Debug.LogError("RED_parameter_sets.csv �̓ǂݍ��݂Ɏ��s���܂����B");
        }
    }

    // �{�^�����N���b�N���ꂽ���ɌĂ΂�鏈��
    public void OnSearchButtonClicked()
    {
        // CSV�̓��e������
        if (!string.IsNullOrEmpty(groupsCSV))
        {
            ProcessGroupsCSV(groupsCSV);
        }

        // �܂���groupsCSV���������parametersCSV�̏������s��
        if (!string.IsNullOrEmpty(groupsCSV) && !string.IsNullOrEmpty(parametersCSV))
        {
            ProcessParameterSets(parametersCSV);
        }

        // �ŏI�I�ȃ��X�g�����O�ŕ\��
        LogFinalList();
    }

    // CSV�t�@�C����ǂݍ��ރ��\�b�h
    string LoadCSV(string filePath)
    {
        if (File.Exists(filePath))
        {
            string fileContent = File.ReadAllText(filePath); // �t�@�C���̓��e�𕶎���Ƃ��ēǂݍ���
            if (string.IsNullOrEmpty(fileContent))
            {
                Debug.LogWarning($"�t�@�C�� {filePath} �̓��e����ł��B");
            }
            return fileContent;
        }
        else
        {
            Debug.LogError($"�t�@�C�� {filePath} ��������܂���B");
            return string.Empty;
        }
    }

    // �h���b�v�_�E��1�̑I�������ύX���ꂽ���̏���
    void OnDropdown1ValueChanged(int index)
    {
        selectedOption1 = dropdown1.options[index].text;
        Debug.Log("�I�����ꂽ�h���b�v�_�E��1�̃I�v�V����: " + selectedOption1);
    }

    // �h���b�v�_�E��2�̑I�������ύX���ꂽ���̏���
    void OnDropdown2ValueChanged(int index)
    {
        selectedOption2 = dropdown2.options[index].text;
        Debug.Log("�I�����ꂽ�h���b�v�_�E��2�̃I�v�V����: " + selectedOption2);
    }

    // RED_groups.csv ���������郁�\�b�h
    void ProcessGroupsCSV(string groupsCSV)
    {
        string[] lines = groupsCSV.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            // �s���J���}�ŕ������ė�ɂ���
            string[] columns = line.Split(',');

            if (columns.Length >= 7) // 7��ȏ�̏ꍇ
            {
                string name = columns[0].Trim();  // 1��ڂ̖��O
                string selectedOption = selectedOption1.Trim();  // �h���b�v�_�E��1�̑I����

                // name �� selectedOption1 �ƈ�v���邩�`�F�b�N
                if (name == selectedOption)
                {
                    // ��v����ꍇ�Aname��2��ڂ���7��ڂ������Avalue1, value2 �� 0
                    for (int i = 1; i <= 6; i++) // 2��ڂ���7��ڂ܂�
                    {
                        string paramName = columns[i].Trim();
                        ExploreParam param = new ExploreParam(paramName, 0, 0);
                        exploreList.Add(param);  // ���X�g�ɒǉ�
                        Debug.Log($"���O: {paramName}, value1: {param.value1}, value2: {param.value2}");
                    }
                }
            }
        }
    }

    // RED_parameter_sets.csv ���������郁�\�b�h
    void ProcessParameterSets(string parametersCSV)
    {
        Debug.Log("1");
        string[] lines = parametersCSV.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            Debug.Log("3");
            // �s���J���}�ŕ������ė�ɂ���
            string[] columns = line.Split(',');

            if (columns.Length >= 7) // 7��ȏ�̏ꍇ
            {
                Debug.Log("4");
                string paramName = columns[0].Trim();  // 1��ڂ̖��O
                if (paramName == selectedOption2)
                {
                    Debug.Log("5");
                    // ��v����ꍇ�A���̍s��6��ڂ�7��ڂ� value1 �� value2 �ɑ��
                    int value1 = int.Parse(columns[5].Trim());  // 6���
                    int value2 = int.Parse(columns[6].Trim());  // 7���

                    // ���łɃ��X�g�Ɋi�[���� ExploreParam �� value1 �� value2 ���X�V
                    for (int i = 0; i < exploreList.Count; i++)
                    {
                        // �X�V: value1, value2 ��ݒ�
                        exploreList[i].value1 = value1;
                        exploreList[i].value2 = value2;
                        Debug.Log($"�X�V���ꂽ�l: name: {exploreList[i].name}, value1: {value1}, value2: {value2}");
                        //break;
                        //if (exploreList[i].name == paramName)
                        //{
                            
                        //}
                    }
                }
            }
        }
    }

    // �ŏI�I�ȃ��X�g�̓��e�����O�ŕ\�����郁�\�b�h
    void LogFinalList()
    {
        Debug.Log("�ŏI�I�ȃ��X�g:");
        foreach (var param in exploreList)
        {
            Debug.Log($"name: {param.name}, value1: {param.value1}, value2: {param.value2}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �K�v�ɉ�����Update���ŏ�����ǉ��ł��܂�
    }
}
