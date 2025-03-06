using UnityEngine;

public class SyncPrefabPosition : MonoBehaviour
{
    public GameObject[] prefabs;  // �v���n�u�̔z��
    public GameObject[] prefabInstances;  // �V�[���ɔz�u����Ă���C���X�^���X�̔z��
    private Vector3[] previousPositions;  // �e�C���X�^���X�̑O��̈ʒu

    void Start()
    {
        // �v���n�u�ƃC���X�^���X�̔z�񂪓������ł��邱�Ƃ��m�F
        if (prefabs.Length != prefabInstances.Length)
        {
            Debug.LogError("�v���n�u�ƃC���X�^���X�̐�����v���܂���I");
            return;
        }

        previousPositions = new Vector3[prefabs.Length];

        // �C���X�^���X�̑O��ʒu��������
        for (int i = 0; i < prefabInstances.Length; i++)
        {
            previousPositions[i] = prefabInstances[i].transform.position;
        }
    }

    void Update()
    {
        // �e�C���X�^���X�̈ʒu���ς�����ꍇ�A�Ή�����v���n�u�̈ʒu���X�V
        for (int i = 0; i < prefabInstances.Length; i++)
        {
            // �C���X�^���X�̈ʒu���ύX���ꂽ�ꍇ
            if (prefabInstances[i].transform.position != previousPositions[i])
            {
                // �v���n�u�̈ʒu���C���X�^���X�̈ʒu�ɍ��킹��
                prefabs[i].transform.position = prefabInstances[i].transform.position;
                previousPositions[i] = prefabInstances[i].transform.position; // �O��ʒu���X�V
            }
        }
    }
}
