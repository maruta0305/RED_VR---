using UnityEngine;
using UnityEngine.InputSystem;

public class UIChange : MonoBehaviour
{
    public GameObject UI; // �\������UI�I�u�W�F�N�g
    public InputActionReference rightHandGripAction; // InputActionReference���C���X�y�N�^�[�Őݒ�

    void Start()
    {
        UI.SetActive(false); // ������Ԃ�UI���\���ɂ���
    }

    void Update()
    {
        // �O���b�v�{�^���������ꂽ�����`�F�b�N
        if (rightHandGripAction.action.WasPressedThisFrame())
        {
            ChangeUI();
        }
    }

    public void ChangeUI()
    {
        // UI�̏�Ԃ��g�O��
        UI.SetActive(!UI.activeSelf);
    }
}
