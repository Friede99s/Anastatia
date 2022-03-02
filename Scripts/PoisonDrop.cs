using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDrop : MonoBehaviour
{
    public float cooltime;
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        cooltime -= Time.deltaTime;
        if(cooltime <= 0)
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
