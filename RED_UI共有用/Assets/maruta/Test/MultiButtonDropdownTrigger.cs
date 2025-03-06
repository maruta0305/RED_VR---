using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MultiButtonDropdownTrigger : MonoBehaviour
{
    public List<Button> buttons; // �Ǘ�����{�^���̃��X�g
    public List<Dropdown> dropdowns; // �Ǘ�����v���_�E���̃��X�g
    public Transform controller; // �R���g���[���[��Transform
    public float positionThreshold = 0.1f; // ��v�Ƃ݂Ȃ������̂������l
    public List<ScrollRect> scrollViews; // ������ScrollView

    private HashSet<Button> clickedButtons = new HashSet<Button>(); // �N���b�N�ς݂̃{�^�����L�^

    void Update()
    {
        // �R���g���[���[�̈ʒu���擾
        Vector3 controllerPosition = controller.position;

        // �{�^���̏���
        foreach (var button in buttons)
        {
            Vector3 buttonPosition = button.transform.position;

            // �{�^���ƃR���g���[���[�̈ʒu����v���Ă��邩���`�F�b�N
            if (Vector3.Distance(buttonPosition, controllerPosition) < positionThreshold)
            {
                // �N���b�N�ς݂̃{�^���łȂ��ꍇ
                if (!clickedButtons.Contains(button))
                {
                    button.onClick.Invoke();
                    clickedButtons.Add(button); // �{�^�����N���b�N�ς݂Ƃ��ċL�^
                    Debug.Log($"{button.name} clicked programmatically!");
                }
            }
            else
            {
                // �R���g���[���[�����ꂽ�ꍇ�̓N���b�N��Ԃ����Z�b�g
                clickedButtons.Remove(button);
            }
        }

        // �v���_�E���̏���
        foreach (var dropdown in dropdowns)
        {
            Vector3 dropdownPosition = dropdown.transform.position;

            if (Vector3.Distance(dropdownPosition, controllerPosition) < positionThreshold)
            {
                dropdown.Show();

                int optionsCount = dropdown.options.Count;

                // �I�����̐������O�ɏo��
                Debug.Log($"Dropdown '{dropdown.name}' has {optionsCount} options.");

                // �h���b�v�_�E���̃e���v���[�g�̈ʒu���擾
                RectTransform templateRect = dropdown.GetComponent<Dropdown>().template.GetComponent<RectTransform>();

                for (int i = 0; i < optionsCount; i++)
                {
                    // �I�����̈ʒu���v�Z
                    RectTransform optionRect = templateRect.GetChild(0).GetChild(i).GetComponent<RectTransform>();
                    Vector3 optionPosition = optionRect.position;

                    Debug.Log($"Option {i}: {optionPosition}");

                    if (Vector3.Distance(optionPosition, controllerPosition) < positionThreshold)
                    {
                        dropdown.value = i;
                        Debug.Log($"Dropdown '{dropdown.name}' changed to {dropdown.options[i].text}");
                        Debug.Log($"Touched option: {dropdown.options[i].text}");
                    }
                }
            }
        }

        // ������ScrollView�̏���
        foreach (var scrollView in scrollViews)
        {
            // �X�N���[���r���[�̈ʒu���擾
            Vector3 scrollViewPosition = scrollView.transform.position;

            // �R���g���[���[��ScrollView�̈ʒu����v���邩���`�F�b�N
            if (Vector3.Distance(scrollViewPosition, controllerPosition) < positionThreshold)
            {
                // �R���g���[���[�̈ړ��ʂɊ�Â���ScrollView���X�N���[��������
                float scrollAmount = controllerPosition.y - scrollViewPosition.y; // Y���̍����g�p
                scrollView.verticalNormalizedPosition -= scrollAmount * 0.5f; // �X�N���[���ʂ𒲐�
                Debug.Log($"ScrollView '{scrollView.name}' moved by {scrollAmount}");
            }
        }
    }
}
