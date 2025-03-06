using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Centerwarp : MonoBehaviour
{
    public List<GameObject> WarpPointObjects;  // ���[�v��̃I�u�W�F�N�g�Q
    public Camera playerCamera;  // VR�J�����i�ʏ��XR�J�����܂���Main Camera�j
    public Transform xrOriginTransform;  // XROrigin��Transform

    // UI�{�^�����Q�Ƃ��郊�X�g�i�{�^��5���j
    public List<Button> warpButtons;  // �{�^�����X�g

    private void OnEnable()
    {
        // �e�{�^���ɑ΂��ăC�x���g��o�^
        for (int i = 0; i < warpButtons.Count; i++)
        {
            int index = i;  // ���[�J���ϐ��Ƃ��ăC���f�b�N�X��n��
            warpButtons[i].onClick.AddListener(() => WarpToPoint(index));
        }
    }

    private void OnDisable()
    {
        // �e�{�^���̃C�x���g������
        for (int i = 0; i < warpButtons.Count; i++)
        {
            int index = i;  // ���[�J���ϐ��Ƃ��ăC���f�b�N�X��n��
            warpButtons[i].onClick.RemoveListener(() => WarpToPoint(index));
        }
    }

    // �{�^���������ꂽ���Ƀ��[�v����
    void WarpToPoint(int index)
    {
        if (index < 0 || index >= WarpPointObjects.Count)
        {
            Debug.LogWarning("Invalid warp point index!");
            return;
        }

        // �I�΂ꂽ���[�v�n�_���擾
        Vector3 warpPosition = WarpPointObjects[index].transform.position;

        // �J���������[�v��̈ʒu�Ɉړ�
        playerCamera.transform.position = warpPosition;

        // XROrigin�����[�v��̈ʒu�Ɉړ�
        xrOriginTransform.position = warpPosition;

        // �K�v�ł���΁A�J�����̉�]���^�[�Q�b�g�I�u�W�F�N�g�ɍ��킹�Ē������邱�Ƃ��ł��܂�
        // playerCamera.transform.rotation = WarpPointObjects[index].transform.rotation;
        // xrOriginTransform.rotation = WarpPointObjects[index].transform.rotation;
    }
}
