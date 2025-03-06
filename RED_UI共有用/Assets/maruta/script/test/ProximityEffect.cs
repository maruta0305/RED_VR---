using UnityEngine;

public class ProximityEffect : MonoBehaviour
{
    public GameObject targetPrefab; // �ΏۂƂȂ�Prefab
    public GameObject effectPrefab; // ����G�t�F�N�g��Prefab
    public float triggerDistance = 10f; // �������鋗��

    private GameObject currentEffect;

    void Update()
    {
        if (targetPrefab != null)
        {
            // �������v�Z
            float distance = Vector3.Distance(transform.position, targetPrefab.transform.position);

            // ������triggerDistance�ȉ��̏ꍇ�ɃG�t�F�N�g��\��
            if (distance <= triggerDistance)
            {
                if (currentEffect == null)
                {
                    // �G�t�F�N�g�𐶐�
                    currentEffect = Instantiate(effectPrefab, targetPrefab.transform.position, Quaternion.identity);
                }
                // �G�t�F�N�g���^�[�Q�b�g�̈ʒu�ɒǏ]������
                currentEffect.transform.position = targetPrefab.transform.position;
            }
            else
            {
                // ���������ꂽ��G�t�F�N�g���\��
                if (currentEffect != null)
                {
                    Destroy(currentEffect);
                    currentEffect = null;
                }
            }
        }
    }
}
