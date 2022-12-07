using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    // Start is called before the first frame update
    public int jumpNum = -1;
    public float timer = 0;
    public bool isWait = false;
    public float timer1 = 0;
    public float minJumpTime = 0.2f;
    public float restTime = 1f;
    public bool isGround = true;
    public GameObject SonPrefabs;
    public Animator ani;
    public int sonNum = 2;
    public int Hp = 1000;
    void Start()
    {
        ani = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isGround)
        {
            timer1 += Time.deltaTime;
            if (timer1 >= restTime)
            {
                timer = 0;
            }
        }
        if (Time.time > 4)
        {
            if (timer == 0 && isGround)
            {
                timer = Time.time;
            }
        }
        if (Hp > 0)
        {
            JumpFuc();
        }
        else
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("DieEnd"))
            {
                Destroy(gameObject);
            }
        }

    }

    public void JumpFuc()
    {
        if (timer > 0 && Time.time <= timer + minJumpTime)
        {
            Vector3 vect = transform.position;
            Rigidbody2D rigid = transform.GetComponent<Rigidbody2D>();
            if (jumpNum < 3 || jumpNum == 4)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 3);
                if (jumpNum == 4)
                {
                    gameObject.layer = LayerMask.NameToLayer("BossDown");
                }
            }
            else if (jumpNum == 3)
            {

                rigid.velocity = new Vector2(rigid.velocity.x, 8);
                ani.SetTrigger("BigJump");
            }
            ani.SetTrigger("Jump");
        }
        if (jumpNum == 3 && ((transform.position.y > -0.5f && sonNum == 2) || (transform.position.y > 2f && sonNum == 1)))
        {
            sonNum -= 1;
            AddSon();
        }
        //if (jumpNum == 4 && ((transform.position.y < -1f && sonNum == 1) || (transform.position.y > 2f && sonNum == 2)))
        //{
        //    sonNum -= 1;
        //    AddSon();
        //}
    }

    public void AddSon()
    {

        GameObject Son = Instantiate(SonPrefabs, transform.Find("InitSon").position, Quaternion.identity);
        //Son.GetComponent<Rigidbody2D>().isKinematic = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumpNum<=3 && (collision.gameObject.name == "FloorBottom" || jumpNum >= 3 && collision.gameObject.name == "Floor5"))
        {
            timer1 = 0;
            isGround = true;
            jumpNum += 1;
            ani.SetBool("isGround", true);
        }
        if (jumpNum == 4 && collision.gameObject.name == "FloorBottom")
        {
            timer1 = 0;
            isGround = true;
            jumpNum =0;
            sonNum = 2;
            ani.SetBool("isGround", true);
            gameObject.layer = LayerMask.NameToLayer("Monster");
        }
        if(collision.gameObject.tag == "ScrollBall" || collision.gameObject.tag == "BallHit")
        {
            HpSum(2);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "FloorBottom" || collision.gameObject.name == "Floor5")
        {
            isGround = false;
            ani.SetBool("isGround", false);
            if(jumpNum == 4)
            {
                sonNum = 2;
            }
        }
    }

    public void HpSum(int type)
    {
        Debug.Log(type);
        Hp = Hp - (type == 1 ? 1 : 100);
        if (Hp<= 0)
        {
            ani.SetBool("isGround", false);
            ani.SetBool("Jump", false);
            isGround = false;
            ani.SetTrigger("isDie");
            gameObject.layer = LayerMask.NameToLayer("BossDie");
        }
        GameObject.Find("BossText").GetComponent<Text>().text = "BOSS HP : " + (Hp<0?0: Hp);
    }
}
