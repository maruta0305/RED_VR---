using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class VRPointerController : MonoBehaviour
{
    public Camera mapCamera;           // �~�j�}�b�v�̃J����
    public RawImage rawImage;          // �}�b�v�\���p��RawImage
    public GameObject agentObject;     // �ړ�������I�u�W�F�N�g
    private NavMeshAgent agent;

    public Transform rightController;  // �E��R���g���[���[
    public InputActionReference rightHandGripAction;  // VR�R���g���[���[�̓��̓A�N�V����

    public LineRenderer lineRenderer;  // LineRenderer

    void Start()
    {
        agent = agentObject.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent ��������܂���B");
        }
    }

    void Update()
    {
        // �R���g���[���[�̓��͏�Ԃ��擾
        if (rightHandGripAction.action.WasPressedThisFrame())
        {
            //Debug.Log("Grip �{�^����������܂���");

            // LineRenderer���g���ĉE��R���g���[���[���烌�C�𔭎�
            Vector3 rayOrigin = rightController.position;
            Vector3 rayDirection = rightController.forward;

            Ray ray = new Ray(rayOrigin, rayDirection);

            // RawImage�Ƀq�b�g���邩����
            RaycastHit hit;  // RaycastHit���ɐ錾
            if (Physics.Raycast(ray, out hit))
            {
                // �q�b�g�����ʒu���擾
                //Debug.Log($"LineRenderer��RawImage�Ƀq�b�g���܂���: {hit.point}");

                // �q�b�g�������[���h���W���X�N���[�����W�ɕϊ�
                Vector3 screenPos = mapCamera.WorldToScreenPoint(hit.point);
                //Debug.Log($"�X�N���[�����W: {screenPos}");

                // �X�N���[�����W��RawImage�̃��[�J�����W�ɕϊ�
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rawImage.rectTransform,
                    screenPos,
                    mapCamera,
                    out localPoint
                );
                //Debug.Log($"RawImage��̃N���b�N�ʒu (���[�J�����W): {localPoint}");

                // ���[�J�����W�𐳋K�����ꂽUV���W�ɕϊ�
                Vector2 normalizedPosition = localPoint / rawImage.rectTransform.sizeDelta;
                //Debug.Log($"���K�����ꂽ�ʒu (UV���W): {normalizedPosition}");

                // UV���W���X�N���[�����W�ɕϊ�
                Vector3 finalScreenPos = mapCamera.ViewportToScreenPoint(new Vector3(normalizedPosition.x, normalizedPosition.y, 0f));
                //Debug.Log($"�ŏI�X�N���[�����W: {finalScreenPos}");

                // �X�N���[�����W�����[���h���W�ɕϊ�
                Ray mapRay = mapCamera.ScreenPointToRay(finalScreenPos);
                RaycastHit mapHit;  // mapRay�p��RaycastHit��錾
                if (Physics.Raycast(mapRay, out mapHit, 100f))  // mapRay���g���ă��C�L���X�g
                {
                    //Debug.Log("���C�L���X�g�Ńq�b�g���܂���");
                    // �q�b�g�����ʒu���^�[�Q�b�g�ʒu�Ƃ��Đݒ�
                    Vector3 targetPosition = mapHit.point;

                    // x �� z ���W�𒲐�
                    targetPosition.x += 12.58f;
                    targetPosition.z += 11.655f;

                    // NavMeshAgent ���g���Ďw�肵���ʒu�Ɉړ�
                    if (agent != null)
                    {
                        //Debug.Log($"�^�[�Q�b�g�ʒu�Ɉړ�: {targetPosition}");
                        agent.SetDestination(targetPosition);
                    }
                    else
                    {
                        //Debug.LogError("NavMeshAgent ��������܂���");
                    }
                }
                else
                {
                    //Debug.LogWarning("mapRay�Ń��C�L���X�g���q�b�g���܂���ł���");
                }
            }
        }

        // LineRenderer���R���g���[���[�̈ʒu�ɍ��킹�čX�V
        lineRenderer.SetPosition(0, rightController.position);
        lineRenderer.SetPosition(1, rightController.position + rightController.forward * 10f);  // �K���ȋ���
    }
}
