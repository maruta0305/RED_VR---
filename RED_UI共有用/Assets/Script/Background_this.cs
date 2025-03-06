using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
using UnityEngine.UI;

public class object_1
{
    [SerializeField] public static Image SelectRED_Connect;
}

public class Background_this : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        object_1.SelectRED_Connect = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
