using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleCanvas : MonoBehaviour
{
    // �Ώۂ�Canvas���A�^�b�`����
    public GameObject targetCanvas;

    // InputActionReference��ݒ�
    public InputActionReference rightHandGripAction;

    void OnEnable()
    {
        // �A�N�V�������L���ɂȂ����Ƃ��ɃC�x���g��o�^
        if (rightHandGripAction != null)
        {
            rightHandGripAction.action.performed += OnGripActionPerformed;
        }
    }

    void OnDisable()
    {
        // �A�N�V�����������ɂȂ����Ƃ��ɃC�x���g������
        if (rightHandGripAction != null)
        {
            rightHandGripAction.action.performed -= OnGripActionPerformed;
        }
    }

    private void OnGripActionPerformed(InputAction.CallbackContext context)
    {
        // Canvas�̗L��/������؂�ւ���
        if (targetCanvas != null)
        {
            bool isActive = targetCanvas.activeSelf;
            targetCanvas.SetActive(!isActive);
        }
    }
}
