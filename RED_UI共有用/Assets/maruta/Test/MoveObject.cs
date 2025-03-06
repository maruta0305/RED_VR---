using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MoveObject : MonoBehaviour
{
    public GameObject[] targetObjects; // �ړ���̃Q�[���I�u�W�F�N�g
    public float interval = 5f;         // �ړ��̊Ԋu�i�b�j

    private int currentTargetIndex = 0; // ���݂̈ړ���̃C���f�b�N�X
    public InputActionReference rightHandGripAction; // InputActionReference���C���X�y�N�^�[�Őݒ�

    private void Update()
    {
        if (rightHandGripAction.action.WasPressedThisFrame())
        {
            StartCoroutine(MoveToTargets());
        }
        // �J��Ԃ����s���邽�߂̃R���[�`�����J�n
        //StartCoroutine(MoveToTargets());
    }

    private System.Collections.IEnumerator MoveToTargets()
    {
        while (true)
        {
            // ���݂̃^�[�Q�b�g�̈ʒu���擾
            if (currentTargetIndex < targetObjects.Length)
            {
                Vector3 targetPosition = targetObjects[currentTargetIndex].transform.position;
                transform.position = targetPosition; // �ړ�
                Debug.Log("Moved to: " + targetPosition);

                // ���̃^�[�Q�b�g�̃C���f�b�N�X���X�V
                currentTargetIndex++;
            }
            else
            {
                // �S�Ẵ^�[�Q�b�g�Ɉړ������烊�Z�b�g
                currentTargetIndex = 0;
            }

            // �w�肳�ꂽ���Ԃ����ҋ@
            yield return new WaitForSeconds(interval);
        }
    }
}


