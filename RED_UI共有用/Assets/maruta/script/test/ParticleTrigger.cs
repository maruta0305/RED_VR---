using UnityEngine;
using UnityEngine.UI; // UI�R���|�[�l���g���g�p���邽�߂̖��O���

public class ParticleTrigger : MonoBehaviour
{
    public GameObject particlePrefab; // �p�[�e�B�N����Prefab
    public Text resultText; // UI��Text�R���|�[�l���g���w�肷��
    private bool hasTriggered = false; // ���̃I�u�W�F�N�g�ň�x�����������邽�߂̃t���O
    private static int result = 0; // �S�I�u�W�F�N�g�Ԃŋ��L�����ÓI�ϐ�

    void Start()
    {
        // ������Ԃ�Text��0��\��
        UpdateUIText();
    }

    void Update()
    {
        // ���C���J�����̈ʒu���擾
        Transform mainCameraTransform = Camera.main.transform;

        // ���̃I�u�W�F�N�g�ƃJ�����̋������v�Z
        float distance = Vector3.Distance(transform.position, mainCameraTransform.position);

        // ������5f�ȓ��ŁA�܂����������s����Ă��Ȃ��ꍇ
        if (distance <= 7f && !hasTriggered)
        {
            // �p�[�e�B�N���𐶐�
            Instantiate(particlePrefab, transform.position, Quaternion.identity);

            // result�𑝉�
            result++;

            // �����ς݃t���O��ݒ�
            hasTriggered = true;

            // UI��Text���X�V
            UpdateUIText();

            // result�̒l���f�o�b�O���O�ɕ\���i�K�v�ɉ����č폜�j
            Debug.Log($"Result: {result}");
        }
    }

    private void UpdateUIText()
    {
        if (resultText != null)
        {
            resultText.text = $"Result: {result}";
        }
        else
        {
            Debug.LogWarning("ResultText���ݒ肳��Ă��܂���B");
        }
    }
}
