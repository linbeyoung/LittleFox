using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerContronller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    // 以上改成pricvate, 就不会在unity上显示
    public Collider2D coll;
    public Collider2D DisColl;  // 可以关掉的碰撞器;

    public Transform CellingCheck, GroundCheck;

    // public AudioSource jumpAudio, hurtAudio, cherryAudio;
    public Joystick joystick;
    [Space]
    public float speed;
    public float jumpForce;
    public LayerMask ground;
    [Space]
    public int Cherry;
    // 22.09 创建text类
    public Text CherryNum;
    // 判断hurt
    private bool isHurt;
    // 5.11
    private bool isGround;
    private int extraJump;

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
    // void FixedUpdate()  // 改用fixupate 适应其他系统
    void FixedUpdate() 
    // void Update() 
    {   
        if (!isHurt)
        {
            Movement();
        }
        SwithAnim();
        isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, ground);  // groundcheck的位置, 检测的范围, 检测的层
    }

    void Update() // update和fixedupdate可以同时使用
    // void Update() 
    {   
        // Jump();
        Crouch();
        CherryNum.text = Cherry.ToString();  // 这里容易忘了.text
        newJump();
    }


    void Movement(){
        // 角色移动  mac端
        float x = Input.GetAxis("Horizontal");  // -1, 0, 1 从input获取水平轴的值
        float face = Input.GetAxisRaw("Horizontal");  // -1, 0, 1 只有三个取值, 获取人物当前转向

        // ios端
        // float x = joystick.Horizontal;  // -1, 0, 1 从input获取水平轴的值
        // float face = Input.GetAxisRaw("Horizontal");  // -1, 0, 1 只有三个取值, 获取人物当前转向

        // float y = Input.GetAxis("Vertical");
        // float horizontalmove;
        // horizontalmove = Input.GetAxis("Horizontal");
        // Vector2 movement = new Vector2(x,y);
        // rb.AddForce(movement * speed);
        // if(Input.GetKey(KeyCode.W)){
        //     rb.velocity = new Vector2(0,speed);
        // }
        if(x != 0){
            rb.velocity = new Vector2(x * speed * Time.fixedDeltaTime, rb.velocity.y);  // 系统物理时钟运行Time.deltaTime,使得画面更平滑
            // rb.velocity = new Vector2(x * speed, rb.velocity.y);  // 系统物理时钟运行Time.deltaTime,使得画面更平滑
            // 增加动画效果
            anim.SetFloat("running", Mathf.Abs(x));
        
        }
        // 实现转向
        if (face != 0){
            transform.localScale = new Vector3(face,1,1);  // 对应unity中的transform.localScale 同时此时是一个三维向量,包含xyz
        }

    }

    // 单独写一个动画切换的函数
    void SwithAnim(){
        // 5.9 把所有跟idle有关的动画效果都删除
        // anim.SetBool("idle", false);  // 默认情况下都会执行这个动画的效果

        // 4.30 补充
        // if (rb.velocity.y < 0.1f  && !coll.IsTouchingLayers(ground))  // 没有向上的速度,且没有触碰到地面
        // {
        //     anim.SetBool("falling", true);
        // }
        if (anim.GetBool("jumping")){
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
            // anim.SetBool("jumping", false);
        }else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                // anim.SetBool("idle", true);
                isHurt = false;
            }
        
        }else if (coll.IsTouchingLayers(ground)){  // 此时碰撞地面了
            anim.SetBool("falling", false);
            // anim.SetBool("idle", true);  // 掉落地面后会一直保持为true
        }
    }


    // 碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision) {  // 只有碰撞到isTrigger触发效果时
        // if (collision.gameObject.CompareTag("Coin")){
        // 收集物品
        if (collision.tag == "Collection"){
            // cherryAudio.Play();
            // Destroy(collision.gameObject);
            // Cherry++;
            collision.GetComponent<Animator>().Play("isGot");
            // CherryNum.text = Cherry.ToString();  // 这里容易忘了.text
            SoundManager.instance.CherryAudio();
        }

        if (collision.tag == "Deadline"){
            Invoke("Restart", 2f);  // 字符型的函数名字,逗号分开

        }
    }


    // 消灭敌人
    private void OnCollisionEnter2D(Collision2D collision) { //区别在于,这是两个刚体碰撞
        // if (collision.gameObject.CompareTag("Enemy")){  // 函数写法不同, 需要collision后加上gameObject, 调用的是整个大的部分
        if (collision.gameObject.tag == "Enemy"){

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();  // 生成一个实体

            if (anim.GetBool("falling")){  // 函数写法不同, 需要collision后加上gameObject, 调用的是整个大的部分

                enemy.JumpOn();
                // rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);  // 实现有个小跳的效果
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // 实现有个小跳的效果

                // 跳跃动画
                // anim.SetBool("falling", false);
                anim.SetBool("jumping", true);

                // Destroy(collision.gameObject);

            }
        // 受伤
        else if (transform.position.x > collision.gameObject.transform.position.x){  // 处在敌人右边
                rb.velocity = new Vector2( -5, rb.velocity.y);
                // hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }
        else if (transform.position.x < collision.gameObject.transform.position.x){  // 处在敌人左边
                rb.velocity = new Vector2(5, rb.velocity.y);
                // hurtAudio.Play();
                SoundManager.instance.HurtAudio();
                isHurt = true;
        }
        }
    }

    // void Jump(){
    //     // 实现跳跃
    //     if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)) {
    //         // rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
    //         rb.velocity = new Vector2(0, jumpForce * Time.deltaTime);
    //         jumpAudio.Play();
    //         // 跳跃动画
    //         anim.SetBool("jumping", true);
    //     }

    //    // Crouch();
    // }
    void newJump(){
        // 实现跳跃
        if (isGround)
        {
            extraJump = 1;
        }
        if (Input.GetButtonDown("Jump")  && extraJump > 0) {
            rb.velocity = Vector2.up * jumpForce;  // 一个可以学习的新写法
            extraJump--;
            // SoundManager soundManager = gameObject.GetComponent<SoundManager>();
            // soundManager.jumpAudio();
            SoundManager.instance.JumpAudio();
            anim.SetBool("jumping", true);
    }
    }

    // 趴下
    void Crouch()
    {
        float crouchRadius = 0.2f;
        if (!Physics2D.OverlapCircle(CellingCheck.position, crouchRadius, ground))  // 检查是否有一个判断器在我定义的范围,圆形比较适合判断是否在我的角色上面,这里希望不在我的上面所以返回false
        {
        if (Input.GetButton("Crouch")){
            anim.SetBool("crouching", true);
            DisColl.enabled = false;  // 关闭碰撞
        }else{
            anim.SetBool("crouching", false);
            DisColl.enabled = true;  // 开启碰撞
        }
        }
    }

    void Restart(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // 碰撞到这个标签时激活某个场景,这里是原场景,所以使用getactivescene().name

    }

    public void CherryCount(){
        Cherry++;
        // CherryNum.text = Cherry.ToString();
}
}
