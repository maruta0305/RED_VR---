using UnityEngine;
using UnityEngine.UI;

public class Savepoint : MonoBehaviour
{
    public Text resultText;
    public GameObject particlePrefab;
    public Transform[] targetObjects; // �����𑪒肷��Ώۂ̃I�u�W�F�N�g�i5�j

    private bool hasTriggered = false;
    private static int save = 0;

    void Start()
    {
        UpdateUIText();
    }

    void Update()
    {
        if (hasTriggered || save >= 10) return; // ���łɏ����ς݁A�܂��͏���Ȃ牽�����Ȃ�

        foreach (Transform target in targetObjects)
        {
            if (target == null) continue; // �O�̂���null�`�F�b�N

            float distanceSquared = (transform.position - target.position).sqrMagnitude;

            if (distanceSquared <= 16f) // 5f * 5f �̋�����
            {
                save++;
                hasTriggered = true;
                UpdateUIText();

                if (particlePrefab != null)
                {
                    Instantiate(particlePrefab, transform.position, Quaternion.identity);
                }

                Debug.Log($"SavePoint triggered by {target.name}! Current save count: {save}");
                break; // ��x���Z���ꂽ�烋�[�v�I��
            }
        }
    }

    private void UpdateUIText()
    {
        if (resultText != null)
        {
            resultText.text = $"Save: {save}/10";
        }
        else
        {
            Debug.LogWarning("ResultText���ݒ肳��Ă��܂���B");
        }
    }
}
