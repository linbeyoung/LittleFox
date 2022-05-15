using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalMovement : MonoBehaviour
{   
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;

    public Collider2D DisColl;  // 可以关掉的碰撞器;

    public int Cherry;
    // 22.09 创建text类
    public Text CherryNum;
    // 判断hurt
    private bool isHurt;


    public float speed, jumpForce, climbSpeed;  // 横向的移动速度 + 纵向的力
    public LayerMask ground, stairs;
    public Transform cellingCheck, groundCheck, attackCheck;

    public bool isGround, isJump, isStairs, isClimbing, isFall;

    bool jumpPressed;
    int jumpCount;

    public Joystick joystick;

    private float gravity;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        gravity = rb.gravityScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")  && jumpCount > 0)
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        
        isStairs = Physics2D.OverlapCircle(groundCheck.position, 0.5f, stairs);
        // isStairs = rb.IsTouchingLayers(LayerMask.GetMask("stairs"));


        if (!isHurt){
            GroundMovement();
        }

        // if (isStairs)
        // {
        Climb();
        // }


        Jump();

        SwitchAnim();

        CherryNum.text = Cherry.ToString();  // 这里容易忘了.text

        Crouch();
    }

    // 更新一个地面移动的新内容, 有摩擦力效果
    void GroundMovement()
    {
        // 横向移动
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);  // 尽量使用v3,因为xyz三个值都会影响他的大小
        }

    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            // rb.velocity = Vector2.up * jumpForce;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            // SoundManager soundManager = gameObject.GetComponent<SoundManager>();
            // soundManager.JumpAudio();
            SoundManager.instance.JumpAudio();
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && !isGround){
            // rb.velocity = Vector2.up * jumpForce;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);        
            jumpCount--;
            SoundManager.instance.JumpAudio();
            jumpPressed = false;
        }
    }

    void SwitchAnim(){
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isHurt){
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                // anim.SetBool("idle", true);
                isHurt = false;
            }
        }

        if (isGround)
        {
            anim.SetBool("falling", false);
            if (isHurt){
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                // anim.SetBool("idle", true);
                isHurt = false;
            }
            }
        }
        
        
        else if (rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
            if (isHurt){
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                // anim.SetBool("idle", true);
                isHurt = false;
            }
        }
        }
        if (!isStairs)
        {
        if (rb.velocity.y < -0.2f)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
            isFall = true;
        }
        else
        {
            anim.SetBool("falling", false);
            isFall = false;
        }
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

        // 趴下
    void Crouch()
    {   
        float crouchRadius = 0.1f;
        if (!isStairs && !Physics2D.OverlapCircle(cellingCheck.position, crouchRadius, ground))  // 检查是否有一个判断器在我定义的范围,圆形比较适合判断是否在我的角色上面,这里希望不在我的上面所以返回false
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

    void Climb()
    {   

        if (isStairs)
        {
            float yMove = Input.GetAxis("Vertical");
            if (Mathf.Abs(yMove) > 0.3f)
            {
                anim.SetBool("climb", true);
                anim.SetBool("idle", false);
                rb.velocity = new Vector2(rb.velocity.x, yMove * climbSpeed);  // 爬梯子的速度会比较小,要加一个爬梯子的速度
                rb.gravityScale = 0.0f;  // 爬梯子时不受重力影响
                isClimbing = true;


            }
            else
            {
                if (isJump || isFall || !isGround)
                {   
                    anim.SetBool("jumping", false);
                    anim.SetBool("falling", false);
                    anim.SetBool("idle", false);
                    anim.SetBool("climb", false);
                    rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                }
                else  // 停在梯子上面
                {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", false);
                anim.SetBool("idle", false);
                anim.SetBool("climb", false);
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                }
            }
        }
        else
        {
            anim.SetBool("climb", false);
            rb.gravityScale = gravity;
        }
        if (isClimbing && isGround)
        {
            anim.SetBool("climb", false);
            anim.SetBool("idle", true);
            rb.gravityScale = gravity;
        }

    }

    void Restart(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // 碰撞到这个标签时激活某个场景,这里是原场景,所以使用getactivescene().name

    }

    public void CherryCount(){
        Cherry += 100;
        // CherryNum.text = Cherry.ToString();
}

}
