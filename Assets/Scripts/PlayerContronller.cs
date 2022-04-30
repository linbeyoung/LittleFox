using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerContronller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    // 以上改成pricvate, 就不会在unity上显示
    public Collider2D coll;
    public float speed;
    public float jumpForce;
    public LayerMask ground;
    public int Cherry;
    // 22.09 创建text类
    public Text CherryNum;

    // public int Gem;
    // 22.09 创建text类
    // public Text GemNum;


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
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)) {
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

    private void OnTriggerEnter2D(Collider2D collision) {  // 只有碰撞到isTrigger触发效果时
        // if (collision.gameObject.CompareTag("Coin")){
        if (collision.tag == "Collection"){
            Destroy(collision.gameObject);
            Cherry++;
            CherryNum.text = Cherry.ToString();  // 这里容易忘了.text
        }
    }


    // 消灭敌人
    private void OnCollisionEnter2D(Collision2D collision) { //区别在于,这是两个刚体碰撞
        // if (collision.gameObject.CompareTag("Enemy")){  // 函数写法不同, 需要collision后加上gameObject, 调用的是整个大的部分
        if (anim.GetBool("falling")){
            if (collision.gameObject.tag == "Enemy"){  // 函数写法不同, 需要collision后加上gameObject, 调用的是整个大的部分

                Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);  // 实现有个小跳的效果
                // 跳跃动画
                anim.SetBool("jumping", true);

        }
        }
    }

}
