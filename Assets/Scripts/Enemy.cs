using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator Anim;  // protected 可以被子类调用, 仅限于父子关系中可以使用
    protected AudioSource deathAudio;


    // Start is called before the first frame update
    protected virtual void Start()  // 虚拟的, 暂时的, 临时的, 这样可以改写父类的start
    {
        Anim = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void Death() // 希望能够调用,使用public
    {
        GetComponent<Collider2D>().enabled = false;  // 获得collider然后取消使用
        Destroy(gameObject);
        
    }

    public void JumpOn()
    {
        Anim.SetTrigger("death");
        deathAudio.Play();
    }
}
