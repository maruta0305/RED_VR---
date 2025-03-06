using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    public Transform Target;
    public Transform random;
    private NavMeshAgent agent;
    private bool isMoving = false;
    private float speed;

    public TMP_InputField inputFieldInstance; // TMP_InputField�ɕύX

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = 1.0f; // �������x��ݒ�
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            agent.destination = random.position;

            // Speed�̎擾
            if (inputFieldInstance != null)
            {
                string inputValue = inputFieldInstance.text; // text�v���p�e�B���g�p
                if (float.TryParse(inputValue, out float parsedValue))
                {
                    speed = parsedValue; // ���͒l�� float �ɕϊ����� speed �ɑ��
                }
                else
                {
                    speed = 1.0f; // �p�[�X�Ɏ��s�����ꍇ�̃f�t�H���g�l
                }
            }
            agent.speed = speed;
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
        agent.velocity = Vector3.zero; // ��~���ɑ��x���[���ɐݒ�
    }
}
