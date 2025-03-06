using UnityEngine;

public class AddColliderToChildren : MonoBehaviour
{
    void Start()
    {
        foreach (Transform child in transform)
        {
            // �q�I�u�W�F�N�g��Box Collider��ǉ�
            if (child.GetComponent<Collider>() == null) // ���ɃR���C�_�[���Ȃ��ꍇ
            {
                child.gameObject.AddComponent<BoxCollider>();
            }
        }
    }
}
