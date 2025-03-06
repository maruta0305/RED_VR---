using UnityEngine;
using System.Collections.Generic;

public class ExplorerController : MonoBehaviour
{
    public List<GameObject> explorers;  // 5�̒T���҃I�u�W�F�N�g

    void Start()
    {
        foreach (var explorer in explorers)
        {
            // �e�T���҂ɒT���X�N���v�g���A�^�b�`
            explorer.AddComponent<Explorer>();
        }
    }
}
