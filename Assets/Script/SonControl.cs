using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonControl : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 2f;
    public bool isGround = false;
    public bool isDistance = false;
    public int dir = -1;
    public Animator ani;
    public GameObject Ball;
    public bool isInit = true;
    public float initTime = 0;
    public bool resetType = false;
    void Start()
    {
        ani = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (resetType == true)
        {
            resetType = false;
            isInit = false;
            isGround = true;
        }
        initTime += Time.deltaTime;
        if (isInit && initTime< Random.Range(1.5f,3))
        {
            transform.position += transform.right * -speed * Time.deltaTime;
            transform.GetComponent<Rigidbody2D>().isKinematic = true;
            //transform.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        else
        {
            transform.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        if (isGround)
        {
            transform.position += transform.right * dir * speed * Time.deltaTime;
            ani.SetBool("move", true);
            //Rigidbody2D rigid = transform.GetComponent<Rigidbody2D>();
            //Vector2 v = new Vector2(dir * speed, rigid.velocity.y);
            //rigid.velocity = v;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag== "Floor")
        {
            isGround = true;
            isInit = false;
            ani.SetBool("isGround", true);
            ani.SetBool("move", true);
        }
        if(collision.collider.tag == "Wall")
        {
            dir = collision.gameObject.name == "WallLeft" ? 1 : -1;
            transform.localScale = new Vector3(-dir, 1, 1);
        }
        if (collision.gameObject.name == "WallLeftDie" || collision.gameObject.name == "WallRightDie")
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor")
        {
            isGround = false;
            ani.SetBool("move", false);
            ani.SetBool("isGround", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            //isGround = false;
            GameObject BallObj = Instantiate(Ball, transform.Find("SonGround").position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
