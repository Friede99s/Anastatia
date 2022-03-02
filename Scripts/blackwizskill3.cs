using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class blackwizskill3 : MonoBehaviour
{
    Transform target;
    public float speed = 5f;
    public float rotateSpeed = 200f;

    //public GameObject blood;

    private Rigidbody2D rb;

    private void Start()
    {
        target = GameObject.Find("player").transform;   //여기 player는 플레이어 오브젝트 이름임
        rb = GetComponent<Rigidbody2D>();
    }
  
    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.up * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        //Instantiate(blood, transform.position, Quaternion.Euler(new Vector3(0,0,0)) );
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
