using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    public int dir;
    //public bool isGround = false;
    public Vector3 leftDir = new Vector3(1,1,1);
    public Vector3 rightDir = new Vector3(-1, 1, 1);
    public float initPlayTime = 3f;
    public bool isMove = false;
    public bool isGround = true;
    public float timer = 0;
    public float invincibleTime = 6;
    public float speedNum = 6;
    public Animator ani;
    public GameObject BulletPrefabs;
    public Transform BulletBox;
    public Vector3 initLocation = new Vector3(0,-3.7f,0);
    public float dieTime = 0;
    public bool isKeyDown = false;
    void Start()
    {
        ani = transform.GetComponent<Animator>();
        BulletBox = transform.Find("BulletBox");
    }

    // Update is called once per frame
    void Update()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Born"))
        {
            isMove = false;
        }
        else
        {
            isMove = true;
        }
        if (isMove && invincibleTime >0)
        {
            timer += Time.deltaTime;
            if (timer >= invincibleTime)
            {
                invincibleTime = 0;
                timer = 0;
                SpriteRenderer spr = transform.GetComponent<SpriteRenderer>();
                gameObject.layer = LayerMask.NameToLayer("Default");
                gameObject.tag = "Player";
                spr.color = Color.white;
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("PlayerHit");
                gameObject.tag = "PlayerHit";
                InvincibleFuc();
            }
        }
        float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        bool jump = Input.GetButtonDown("Jump");
        if (transform.localScale == leftDir)
        {
            dir = 1;
        }else if(transform.localScale == rightDir)
        {
            dir = -1;
        }
        if (isMove && dieTime == 0)
        {
            if (h > 0.01f)
            {
                transform.localScale = leftDir;
            }else if(h < -0.01f)
            {
                transform.localScale = rightDir;
            }
            if (Mathf.Abs(h) > 0)
            {
                ani.SetBool("isWalk", true);
            }
            else
            {
                ani.SetBool("isWalk", false);
            }
            //rigid.velocity = new Vector2(h * speed, vy);
            Rigidbody2D rigid = transform.GetComponent<Rigidbody2D>();
            float vy = rigid.velocity.y;
            if (jump && isGround)
            {
                ani.SetTrigger("Jump");
                ani.SetBool("isGround", false);
                isGround = false;
                vy = speedNum;
            }
            rigid.velocity = new Vector2(h * speedNum, vy);
            //Vector3 dirTemp = new Vector3(h, 0, jump?1:0);
            //transform.position +=  dirTemp * speedNum * Time.deltaTime;
            if (Input.GetButtonDown("Fire1"))
            {
                if (isGround)
                {
                    ani.SetTrigger("Fire");
                }
                isKeyDown = true;
                GameObject bullet = Instantiate(BulletPrefabs, BulletBox.position, Quaternion.identity);
                BulletControl bulletControl = bullet.GetComponent<BulletControl>();
                bulletControl.dir = dir;
                GameObject.Find("Ball").GetComponent<BallControl>().MoveFuc(dir);
            }
            if (Input.GetButtonUp("Fire1"))
            {
                isKeyDown = false;
            }
        }
        if (dieTime != 0 && dieTime + 1 <= Time.time)
        {
            transform.position = initLocation;
            transform.GetComponent<CapsuleCollider2D>().enabled = true;
            dieTime = 0;
            timer = 0;
            invincibleTime = 6;
            timer = 0;
        }
    }


    void InvincibleFuc()
    {
        SpriteRenderer spr= transform.GetComponent<SpriteRenderer>();

        if (timer % 0.5f < 0.125f)
        {
            spr.color = Color.red;
        }
        else if(timer % 0.5f < 0.25f)
        {
            spr.color = Color.yellow;
        }
        else if(timer % 0.5f < 0.375f)
        {
            spr.color = Color.blue;
        }
        else if (timer % 0.5f < 0.5f)
        {
            spr.color = Color.green;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag=="Floor")
        {
            isGround = true;
            ani.SetBool("isGround", true);
        }
        if (collision.collider.tag == "Son" || collision.gameObject.name == "Boss")
        {
            dieTime = Time.time;
            isMove = false;
            ani.SetTrigger("Die");
            transform.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
}
