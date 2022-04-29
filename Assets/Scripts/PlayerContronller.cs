using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContronller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    // 以上改成pricvate, 就不会在unity上显示
    public Collider2D coll;
    public float speed;
    public float jumpForce;
    public LayerMask ground;


    // Start is called before the first frame update
    void Start()
    {
        // 取得rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        // 取得animator
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    // void Update()  // 改用fixupate 适应其他系统
    void FixedUpdate() 
    {
        Movement();
        SwithAnim();
    }

    void Movement(){
        // 角色移动
        float x = Input.GetAxis("Horizontal");  // -1, 0, 1 从input获取水平轴的值
        float face = Input.GetAxisRaw("Horizontal");  // -1, 0, 1 只有三个取值, 获取人物当前转向
        // float y = Input.GetAxis("Vertical");
        // float horizontalmove;
        // horizontalmove = Input.GetAxis("Horizontal");
        // Vector2 movement = new Vector2(x,y);
        // rb.AddForce(movement * speed);
        // if(Input.GetKey(KeyCode.W)){
        //     rb.velocity = new Vector2(0,speed);
        // }
        if(x != 0){
            rb.velocity = new Vector2(x * speed * Time.deltaTime, rb.velocity.y);  // 系统物理时钟运行Time.deltaTime,使得画面更平滑
            // 增加动画效果
            anim.SetFloat("running", Mathf.Abs(face));
        
        }
        // 实现转向
        if (face != 0){
            transform.localScale = new Vector3(face,1,1);  // 对应unity中的transform.localScale 同时此时是一个三维向量,包含xyz
        }

        // 实现跳跃
        if (Input.GetButtonDown("Jump")){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            // 跳跃动画
            anim.SetBool("jumping", true);
        }

    }

    // 单独写一个动画切换的函数
    void SwithAnim(){
        anim.SetBool("idle", false);  // 默认情况下都会执行这个动画的效果

        if (anim.GetBool("jumping")){
            if (rb.velocity.y < 0){
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
            // anim.SetBool("jumping", false);
        }else if (coll.IsTouchingLayers(ground)){  // 此时碰撞地面了
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);  // 掉落地面后会一直保持为true
        }
    }

}
