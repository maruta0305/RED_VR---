using UnityEngine;
using System.Collections.Generic;

public class ExplorerController : MonoBehaviour
{
    public List<GameObject> explorers;  // 5つの探索者オブジェクト

    void Start()
    {
        foreach (var explorer in explorers)
        {
            // 各探索者に探索スクリプトをアタッチ
            explorer.AddComponent<Explorer>();
        }
    }
}
