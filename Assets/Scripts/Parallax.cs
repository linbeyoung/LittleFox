using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform Cam;
    public float moveRate;  // 需要获取得到当前的位置
    private float startPointX, startPointY;
    public bool lockY; // false表示不锁定y轴
    // Start is called before the first frame update
    void Start()
    {
        startPointX = transform.position.x;
        startPointY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockY) {
            transform.position = new Vector2(startPointX + (Cam.position.x * moveRate), transform.position.y); 
        }
        else
        {
            transform.position = new Vector2(startPointX + (Cam.position.x * moveRate), startPointY + (Cam.position.y * moveRate));
        }
    }
}
