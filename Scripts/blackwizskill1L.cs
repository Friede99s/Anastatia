using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackwizskill1L : MonoBehaviour
{
    Rigidbody2D rigid;
    public float speed;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        speed += Time.deltaTime;
        rigid.velocity = new Vector2(-speed, rigid.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
