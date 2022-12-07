using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    // Start is called before the first frame update
    public int dir;
    public float speed = 6;
    public float flightTime = 0.6f;
    public float downTime = 0.3f;
    public float timer;
    void Start()
    {
        transform.localScale = new Vector3(-dir, 1,1);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Rigidbody2D rigid = transform.GetComponent<Rigidbody2D>();
        if (timer > flightTime)
        {
            rigid.isKinematic = false;
            transform.position+= transform.right * dir * 0.8f * Time.deltaTime;
        }
        else
        {
            transform.position+= transform.right * dir * speed * Time.deltaTime;
        }
        if (timer > flightTime + downTime)
        {
            timer = 0;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor" || collision.tag == "Wall" || collision.gameObject.name == "Boss")
        {
            timer = 0;
            Destroy(gameObject);
            if(collision.gameObject.name == "Boss")
            {
                collision.GetComponent<BossControl>().HpSum(1);
            }
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.tag == "Floor" || collision.collider.tag == "Wall")
    //    {
    //        timer = 0;
    //        Destroy(gameObject);
    //    }
    //}
}
