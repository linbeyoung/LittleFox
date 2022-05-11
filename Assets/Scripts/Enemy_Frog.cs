using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;  // 首先先定义一个rigidbody2D
    private Collider2D Coll;  // 为了判断是否接地或者是否碰撞
    // private Animator Anim;
    public LayerMask Ground;

    public Transform leftpoint, rightpoint;  // 定义左右两个点
    public float Speed, jumpForce;  // 定义移动速度和跳跃力度
    // 获取运动范围
    private float leftX, rightX;

    // 设置朝向
    private bool Faceleft = true;  // 图片本身是朝左的

    // [Space]

    // Start is called before the first frame update
    protected override void Start()  // 怎么获得父类的start? 标准写法,override可以重写父类的方法
    {   
        base.Start();  // 先执行父类的start, 这是一个固定的写法
        // ❗❗❗ 获取rigidbody2D
        rb = GetComponent<Rigidbody2D>(); 
        // Anim = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();


        transform.DetachChildren();  // 删除子物体, ❗ 删除子物体是在transform里面定义的
        leftX = leftpoint.position.x;
        rightX = rightpoint.position.x;
        // 获取完之后就删除
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Movement();  // 添加动画事件之后就不需要每帧都调用这个函数了
        SwithAnim();
    }

    // void Movement(){
    //     if (Faceleft)  // 不只是改朝向, 还要改速度
    //     {
    //         if (Coll.IsTouchingLayers(Ground))
    //         {
    //             Anim.SetBool("jumping", true);
    //             rb.velocity = new Vector2(-Speed, jumpForce);
    //         }
    //         if (transform.position.x < leftX)
    //         {
    //             transform.localScale = new Vector3(-1, 1, 1);
    //             Faceleft = false;
    //         }

    //     }
    //     else  // ❗ 这里的else是反的,容易出错
    //         {
    //             if (Coll.IsTouchingLayers(Ground))
    //             {
    //                 Anim.SetBool("jumping", true);
    //                 rb.velocity = new Vector2(Speed, jumpForce);
    //             }
    //             if (transform.position.x > rightX)
    //             {
    //                 transform.localScale = new Vector3(1, 1, 1);
    //                 Faceleft = true;
    //             }
    //         }
    //     // 当角色的位置小于左边点的位置时, 改变朝向
    //     // {
    //     //     transform.localScale = new Vector3(1, 1, 1);
    //     // }
    //     // else
    //     // {
    //     //     transform.localScale = new Vector3(-1, 1, 1);
    //     // }    
    // }
void Movement()
    {
        if (Coll.IsTouchingLayers(Ground))
        {
            Anim.SetBool("jumping", true);
            if (Faceleft)
            {
                if (transform.position.x < leftX)
                {
                    rb.velocity = new Vector2(Speed, jumpForce);
                    transform.localScale = new Vector2(-1, 1);
                    Faceleft = false;
                }
                else
                {
                    rb.velocity = new Vector2(-Speed, jumpForce);
                }
            }            else
            {
                if (transform.position.x > rightX )
                {
                    rb.velocity = new Vector2(-Speed, jumpForce);
                    transform.localScale = new Vector2(1, 1);
                    Faceleft = true;
                }
                else
                {
                    rb.velocity = new Vector2(Speed, jumpForce);
                }
            }
        }
    }
    void SwithAnim(){
        if (Anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1)
            {
                Anim.SetBool("jumping", false);
                Anim.SetBool("falling", true);
            }
        }
        if (Coll.IsTouchingLayers(Ground) && Anim.GetBool("falling"))  // 接触地面才能跳
        {
            Anim.SetBool("falling", false);
        }
    
    }


}