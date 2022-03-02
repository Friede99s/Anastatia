using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public GameObject bulletPrefab;
    public int nextMove;
    public Transform pos;
    public float cooltime;
    private float curtime;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 1);
    }
    
    void FixedUpdate()
    {
        if(nextMove == 1)
        {
            Debug.DrawRay(rigid.position, Vector3.right, new Color(1, 0, 1));
            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.right, 1, LayerMask.GetMask("Player"));
            if(rayhit.collider != null)
            {
                anim.SetBool("IsPlayer", true);
                if (curtime <= 0)
                {
                    Attack();
                    curtime = cooltime;
                }
            }
            else
            {
                anim.SetBool("IsPlayer", false);
            }
        }
        else
        {
            Debug.DrawRay(rigid.position, Vector3.left, new Color(1, 0, 1));
            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.left, 1, LayerMask.GetMask("Player"));
            if (rayhit.collider != null)
            {
                anim.SetBool("IsPlayer", true);
                if (curtime <= 0)
                {
                    Attack();
                    curtime = cooltime;
                }
            }
            else
            {
                anim.SetBool("IsPlayer", false);
            }
        }
        curtime -= Time.deltaTime;
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float randomThinkTime = Random.Range(2f, 4f);
        Invoke("Think", randomThinkTime);

        //Change Direction
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;
    }

    void Attack()
    {
        Instantiate(bulletPrefab, pos.position, transform.rotation);
    }
}
