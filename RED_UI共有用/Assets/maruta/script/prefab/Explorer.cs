using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Explorer : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Vector3> exploredLocations = new List<Vector3>();  // �����{�b�g�̒T���ςݏꏊ
    public static List<Vector3> globalExploredLocations = new List<Vector3>();  // �S���{�b�g�̒T���ςݏꏊ
    public float moveSpeed = 3f;
    private bool isExploring = true;  // �T�����t���O
    private Vector3 lastPosition;
    private float checkInterval = 2f;  // ����_���`�F�b�N����Ԋu
    private float timeSinceLastCheck = 0f;  // �Ō�Ƀ`�F�b�N��������
    private float minimumDistance = 5f;  // ���̃��{�b�g�Ƃ̍ŏ�����

    public List<Explorer> otherExplorers;  // ���̃��{�b�g�̎Q�Ƃ��i�[���郊�X�g

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.autoBraking = false;
        lastPosition = transform.position;
        StartExploring();
    }

    void Update()
    {
        if (isExploring)
        {
            // �ړ�������ʒu���L�^
            if (Vector3.Distance(transform.position, lastPosition) > 1f)
            {
                lastPosition = transform.position;
                exploredLocations.Add(transform.position);  // �����{�b�g�̒T���ς݃��X�g
                globalExploredLocations.Add(transform.position);  // �S���{�b�g�ŒT���ςݏꏊ�����L
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // ���̒T����ɐi��
                MoveToNextPoint();
            }
        }

        // ����_���`�F�b�N����^�C�~���O�𐧌�
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck >= checkInterval)
        {
            timeSinceLastCheck = 0f;
            CheckForSplitPaths();
        }
    }

    void StartExploring()
    {
        // �����̈ړ����ݒ�
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        Vector3 nextPosition = GetNextPosition();
        if (nextPosition != Vector3.zero)
        {
            agent.SetDestination(nextPosition);  // �V�����ړ���
        }
        else
        {
            // �s���~�܂�̏ꍇ�A�߂�
            isExploring = false;  // �T�����~
            StartCoroutine(ReturnToStartPosition());
        }
    }

    Vector3 GetNextPosition()
    {
        // ���̈ړ��悪���T���ł���A�i�r���b�V����ɂ���ꏊ��T��
        Vector3[] potentialLocations = new Vector3[]
        {
            transform.position + transform.forward * 10f,  // �O�i
            transform.position + transform.right * 10f,  // �E����
            transform.position + transform.right * -10f  // ������
        };

        List<Vector3> validPaths = new List<Vector3>();

        // �e�����ɐi�ނ��߂̈ʒu���m�F
        foreach (var location in potentialLocations)
        {
            if (!globalExploredLocations.Contains(location) && NavMesh.SamplePosition(location, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                // ���̃��{�b�g�Ƌ������\���Ɏ��邩�`�F�b�N
                if (IsFarEnoughFromOtherExplorers(hit.position))
                {
                    validPaths.Add(hit.position);
                }
            }
        }

        if (validPaths.Count > 0)
        {
            // �����_���ɕ�����I��
            Vector3 selectedPath = validPaths[Random.Range(0, validPaths.Count)];
            return selectedPath;
        }

        return Vector3.zero;  // �T���\�ȏꏊ���Ȃ��ꍇ
    }

    // ���̃��{�b�g�Ə\���ɋ��������Ă��邩�m�F
    bool IsFarEnoughFromOtherExplorers(Vector3 position)
    {
        foreach (var explorer in otherExplorers)
        {
            if (explorer != this && Vector3.Distance(position, explorer.transform.position) < minimumDistance)
            {
                return false;  // ���̃��{�b�g���߂�����ꍇ��NG
            }
        }

        return true;  // �߂��Ƀ��{�b�g�����Ȃ����OK
    }

    // ����_�����o���ă��{�b�g�������_���ɕʂ̕����֐i�܂���
    void CheckForSplitPaths()
    {
        Vector3[] directions = new Vector3[]
        {
            transform.position + transform.forward * 10f,
            transform.position + transform.right * 10f,
            transform.position + transform.right * -10f
        };

        List<Vector3> validPaths = new List<Vector3>();
        foreach (var direction in directions)
        {
            if (!globalExploredLocations.Contains(direction) && NavMesh.SamplePosition(direction, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                if (IsFarEnoughFromOtherExplorers(hit.position))  // ���̃��{�b�g�Ə\���ɋ���������ꍇ
                {
                    validPaths.Add(hit.position);
                }
            }
        }

        if (validPaths.Count > 0)
        {
            // �����_���ɕ�����I��
            Vector3 selectedPath = validPaths[Random.Range(0, validPaths.Count)];
            agent.SetDestination(selectedPath);  // �����_���ȕ����ɐi��
            Debug.Log($"����őI�����ꂽ�ꏊ: {selectedPath}");
        }
    }

    System.Collections.IEnumerator ReturnToStartPosition()
    {
        // �s���~�܂�Ŗ߂鏈��
        Vector3 startPosition = exploredLocations[0];  // �����ʒu�ɖ߂�
        agent.SetDestination(startPosition);

        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            yield return null;  // �ړI�n�ɓ��B����܂őҋ@
        }

        isExploring = true;
        exploredLocations.Clear();  // �ĒT���p�ɒT�����������Z�b�g
        globalExploredLocations.Clear();  // �S���{�b�g�̒T�����������Z�b�g
        StartExploring();  // �ēx�T�����J�n
    }
}
