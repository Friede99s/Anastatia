using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigid;
    Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerPos = GameObject.Find("player").transform;
        Atk();
    }

    void Atk()
    {
        Vector2 dir = (playerPos.position - transform.position).normalized * 5;
        rigid.velocity = dir * speed;
        Invoke("Return", 2);
    }

    void Return()
    {
        Vector2 dir = (playerPos.position - transform.position).normalized * -5;
        rigid.velocity = dir * speed;
        Destroy(gameObject, 2);
    }
    
}
