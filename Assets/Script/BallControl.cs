using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BallControl : MonoBehaviour
{
    // Start is called before the first frame update
    public int num = 0;
    public GameObject Son;
    public Rigidbody2D rigid;
    public int speed = 12;
    public bool isScrollTouch = false;
    public bool isScroll = false;
    public float resetTime = 0;
    public float resurrectionTime = 6;
    public PlayerControl Player;
    public int dir;
    void Start()
    {
        rigid = transform.GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Nick").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        resetTime += Time.deltaTime;
        if (isScrollTouch && isScroll)
        {
            transform.position += transform.right * dir * speed * Time.deltaTime;
        }
        if (num!=3 && Player.isKeyDown)
        {
            MoveFuc(Player.dir);
        }

        if(resetTime>= resurrectionTime && num > 0 && !isScroll)
        {
            num = num - 1;
            resetTime = 0;
            transform.GetComponent<Animator>().SetInteger("Num", num);
        }
        if(resetTime >= resurrectionTime && num == 0 && !isScroll)
        {
            resetTime = 0;
            GameObject SonObj = Instantiate(Son, transform.Find("ground").position, Quaternion.identity);
            SonControl SonControl = SonObj.GetComponent<SonControl>();
            SonControl.resetType = true;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            resetTime = 0;
            Destroy(collision.gameObject);
            if (num < 3)
            {
                num += 1;
                if (num < 2)
                {

                    if (LayerMask.LayerToName(transform.gameObject.layer) != "UnfinishedBall")
                    {
                        transform.gameObject.layer = LayerMask.NameToLayer("UnfinishedBall");
                        transform.gameObject.tag = "UnfinishedBall";
                    }
                }
                else
                {
                    num = 2;
                    transform.gameObject.layer = LayerMask.NameToLayer("Ball");
                    transform.gameObject.tag = "Ball";
                }
                transform.GetComponent<Animator>().SetInteger("Num", num);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Nick" && num != 3)
        {
            isScrollTouch = true;
        }
        if (collision.collider.tag == "Wall")
        {
            dir = collision.gameObject.name == "WallLeft" ? 1 : -1;
        }
        if(collision.gameObject.name == "WallLeftDie" || collision.gameObject.name == "WallRightDie" || collision.gameObject.name== "Boss")
        {
            Destroy(gameObject);
        }
        if(collision.gameObject.name == "Son")
        {
            Destroy(collision.gameObject);
        }
        if (num==3 && (collision.collider.tag == "ScrollBall" || collision.collider.tag == "UnfinishedBall"))
        {
            Destroy(collision.gameObject);
        }
        if (num == 3 && collision.gameObject.name == "Nick" && transform.gameObject.tag == "ScrollBall")
        {
            gameObject.layer = LayerMask.NameToLayer("BallHit");
            transform.gameObject.tag = "BallHit";
            SpriteRenderer spr = transform.GetComponent<SpriteRenderer>();
            spr.color = Color.blue;
            Player.invincibleTime = 6;
            Player.timer = 0;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Nick")
        {
            if(num != 3)
            {
                isScrollTouch = false;
            }
            else
            {
                if (transform.gameObject.tag == "Ball")
                {
                    transform.gameObject.tag = "ScrollBall";
                    gameObject.layer = LayerMask.NameToLayer("ScrollBall");
                }
            }
        }
    }

    public void MoveFuc(int dirNum)
    {
        if (isScrollTouch)
        {
            isScroll = true;
            dir = dirNum;
            num = 3;
            transform.GetComponent<Animator>().SetInteger("Num", num);
        }
    }
}
