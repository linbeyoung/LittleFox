using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    public void Death()
    {
        // FindObjectOfType<PlayerContronller>().CherryCount();  // 找到这行代码
        FindObjectOfType<FinalMovement>().CherryCount();  // 找到这行代码
        Destroy(gameObject);

    }
}
