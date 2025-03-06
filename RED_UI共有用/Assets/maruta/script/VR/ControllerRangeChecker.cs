using UnityEngine;
using UnityEngine.UI;

public class ControllerRangeChecker : MonoBehaviour
{
    public Button targetButton; // �Ώۂ̃{�^��
    private bool isInRange = false; // �R���g���[���[���͈͓��ɂ��邩�ǂ���

    void Update()
    {
        // �R���g���[���[���͈͓��ŁA�{�^�����N���b�N���ꂽ�Ƃ�
        if (isInRange) // "Fire1"�͒ʏ�A���N���b�N�܂��̓Q�[���p�b�h�̃{�^���Ƀ}�b�v����Ă��܂�
        {
            Debug.Log("true");
            targetButton.onClick.Invoke(); // �{�^���̃N���b�N���V�~�����[�g
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �R���g���[���[���͈͂ɓ������Ƃ�
        if (other.CompareTag("Controller")) // �R���g���[���[�Ƀ^�O��t���Ă������Ƃ�O��
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �R���g���[���[���͈͂���o���Ƃ�
        if (other.CompareTag("Controller"))
        {
            isInRange = false;
        }
    }
}

