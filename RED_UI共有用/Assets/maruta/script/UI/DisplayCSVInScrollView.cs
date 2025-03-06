using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class DisplayCSVInScrollView : MonoBehaviour
{
    public ScrollRect scrollView;  // �X�N���[���r���[
    public GameObject contentPanel;  // �X�N���[���r���[���̃R���e���c�p�l��
    public GameObject textPrefab;  // CSV�̊e�s��\�����邽�߂�Text�v���n�u

    private string fileName = "output.csv";  // CSV�t�@�C����
    private float updateInterval = 5f;  // �X�V�Ԋu
    private float nextUpdateTime = 0f;  // ���̍X�V����

    void Start()
    {
        DisplayCSVContent();
        //StartCoroutine(UpdateCSVContent());
    }

    void Update()
    {
        // ���݂̎��Ԃ����̍X�V���Ԃ𒴂����ꍇ�ACSV���X�V
        if (Time.time >= nextUpdateTime)
        {
            DisplayCSVContent();
            nextUpdateTime = Time.time + updateInterval;  // ����̍X�V���Ԃ�ݒ�
        }
    }
    //IEnumerator UpdateCSVContent()
    //{
    //    while (true)
    //    {
    //        DisplayCSVContent();
    //        yield return new WaitForSeconds(5f);  // 5�b���ƂɍX�V
    //    }
    //}

    void DisplayCSVContent()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);  // �t�@�C���p�X

        if (File.Exists(filePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // Content�p�l�����̊���UI�v�f���폜
                foreach (Transform child in contentPanel.transform)
                {
                    Destroy(child.gameObject);
                }

                // �e�s�����̂܂�Text�ɔ��f
                foreach (string line in lines)
                {
                    // CSV�̍s���J���}��؂�̏ꍇ�A�K�v�ɉ����ĕ���
                    string[] columns = line.Split(',');

                    // �e�J�����i�Z���j�̓��e��Text�Ƃ��ĕ\��
                    string fullText = string.Join("  |  ", columns);  // ��: �J���}���u | �v�ɕϊ�

                    // �v���n�u����V����Text�I�u�W�F�N�g�𐶐�
                    GameObject newText = Instantiate(textPrefab);
                    newText.transform.SetParent(contentPanel.transform, false);

                    // Text�R���|�[�l���g��CSV�̓��e���Z�b�g
                    Text textComponent = newText.GetComponent<Text>();
                    textComponent.text = fullText;

                    // Text�̃T�C�Y�����i�K�v�ɉ����āj
                    textComponent.horizontalOverflow = HorizontalWrapMode.Wrap;
                    textComponent.verticalOverflow = VerticalWrapMode.Overflow;

                    // Text�R���|�[�l���g�ɍ��킹�ăR���e���c�̃T�C�Y�𒲐�
                    ContentSizeFitter contentFitter = newText.GetComponent<ContentSizeFitter>();
                    if (contentFitter == null)
                    {
                        contentFitter = newText.AddComponent<ContentSizeFitter>();
                    }
                    contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                    contentFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                }

                // Content�p�l���̃T�C�Y����������
                contentPanel.GetComponent<ContentSizeFitter>().SetLayoutVertical();

                // �X�N���[���r���[����ԏ�ɃX�N���[��
                Canvas.ForceUpdateCanvases();
                scrollView.verticalNormalizedPosition = 1f;
            }
            catch (IOException ex)
            {
                Debug.LogError($"CSV�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("CSV�t�@�C����������܂���: " + filePath);
        }
    }
}
