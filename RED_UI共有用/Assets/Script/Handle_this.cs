using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class object_2
{
    [SerializeField] public static RectTransform handle_SelectRED;
}

public class Handle_this : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        object_2.handle_SelectRED = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
