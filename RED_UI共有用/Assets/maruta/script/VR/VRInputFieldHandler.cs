using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class VRInputFieldHandler : MonoBehaviour
{
    public List<InputField> inputFields; // �Ǘ�����InputField�̃��X�g
    public Transform rightController; // �E�R���g���[���[��Transform
    public float positionThreshold = 0.05f; // ��v�Ƃ݂Ȃ������̂������l

    void Update()
    {
        // �E�R���g���[���[�̈ʒu���擾
        Vector3 controllerPosition = rightController.position;

        foreach (var inputField in inputFields)
        {
            // InputField�̈ʒu���擾
            Vector3 inputFieldPosition = inputField.transform.position;

            // �R���g���[���[�̈ʒu��InputField�ɋ߂��ꍇ
            if (Vector3.Distance(inputFieldPosition, controllerPosition) < positionThreshold)
            {
                // InputField��I����Ԃɂ���
                EventSystem.current.SetSelectedGameObject(inputField.gameObject);

                // ���̓t�B�[���h�Ƀt�H�[�J�X�𓖂Ă�
                if (!inputField.isFocused)
                {
                    inputField.ActivateInputField();
                }

                // Enter�L�[�œ��͂��m��i��j
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    inputField.DeactivateInputField(); // ���̓t�B�[���h���A�N�e�B�u��
                }
            }
            else
            {
                // �R���g���[���[�����ꂽ�ꍇ�AInputField�̑I��������
                if (EventSystem.current.currentSelectedGameObject == inputField.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }
    }
}
