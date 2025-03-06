using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MultiCubeController : MonoBehaviour
{
    public GameObject[] cubes;                   // ����Ώۂ̃L���[�u
    public ParticleSystem selectionParticle;     // �I����Ԃ������p�[�e�B�N��
    public InputActionReference rightHandGripAction;  // �E���Grip�A�N�V����
    public LineRenderer leftHandLineRenderer;    // �����LineRenderer

    private GameObject selectedCube = null;      // �I��ҋ@��Ԃ̃L���[�u

    private void OnEnable()
    {
        // �O���b�v�A�N�V������MoveOrSelectCube���\�b�h��o�^
        rightHandGripAction.action.performed += ctx => MoveOrSelectCube();
    }

    private void OnDisable()
    {
        // �C�x���g�̉���
        rightHandGripAction.action.performed -= ctx => MoveOrSelectCube();
    }

    private void MoveOrSelectCube()
    {
        // ���C�L���X�g����
        Transform leftHandTransform = leftHandLineRenderer.transform;
        Vector3 rayOrigin = leftHandTransform.position;
        Vector3 rayDirection = leftHandTransform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (System.Array.Exists(cubes, cube => cube == hitObject)) // ���C���L���[�u�ɓ��������ꍇ
            {
                // �I����Ԃ̃L���[�u��ύX
                SetSelectedCube(hitObject);
            }
            else if (selectedCube != null) // �L���[�u�ȊO�̏ꏊ�ɓ��������ꍇ
            {
                // �I����Ԃ̃L���[�u���ړ�
                MoveSelectedCube(hit.point);
            }
        }
        else
        {
            Debug.Log("�����q�b�g���܂���ł���");
        }
    }

    private void SetSelectedCube(GameObject newSelectedCube)
    {
        if (selectedCube == newSelectedCube) return;

        // �ȑO�̑I�������Z�b�g
        if (selectedCube != null)
        {
            var particle = selectedCube.GetComponentInChildren<ParticleSystem>();
            if (particle != null) particle.Stop();
        }

        // �V�����I�����Z�b�g
        selectedCube = newSelectedCube;
        var newParticle = selectedCube.GetComponentInChildren<ParticleSystem>();
        if (newParticle != null) newParticle.Play();

        Debug.Log("�I�����ꂽ�L���[�u: " + selectedCube.name);
    }

    private void MoveSelectedCube(Vector3 targetPosition)
    {
        if (selectedCube == null) return;

        NavMeshAgent navMeshAgent = selectedCube.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(targetPosition);
            Debug.Log($"�L���[�u {selectedCube.name} ���ړ�: {targetPosition}");
        }
        else
        {
            Debug.LogError("NavMeshAgent��������܂���: " + selectedCube.name);
        }
    }
}
