using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public GameObject targetObject; // �A�^�b�`����Ώۂ̃I�u�W�F�N�g

    void Update()
    {
        if (targetObject != null)
        {
            // �^�[�Q�b�g�I�u�W�F�N�g�̈ʒu���擾
            Vector3 targetPosition = targetObject.transform.position;

            // ���݂̃I�u�W�F�N�g���^�[�Q�b�g��X, Z���W�Ɉړ�������
            transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        }
    }
}
