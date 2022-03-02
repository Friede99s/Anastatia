using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grasshopper : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    public int nextMove;
    public GameObject go;
    private int rage=0; //0:no rage, 1:rage, 2:tired
    private bool exmark = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 3.5f);
        Invoke("Move", 0.5f);
    }

    void FixedUpdate()
    {
        if (rage == 1)
            anim.SetBool("atk", true);
        else
            anim.SetBool("atk", false);

        if (exmark == true)
        {
            Instantiate(go, transform);
            exmark = false;
        }
            
        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayhit.collider == null)
        {
            Turn();
        }
        //Detection
        Detection();
    }

    void Move()
    {
        anim.SetInteger("Walk", nextMove);
        rigid.velocity = new Vector2(nextMove*3.5f, rigid.velocity.y);
        float randThinkTime = Random.Range(1f, 2f);
        Invoke("Move", 1);
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float randThinkTime = Random.Range(1f, 3.5f);
        Invoke("Think", randThinkTime);
        //Change Direction
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Move", 1);
        Invoke("Think", 1.5f);
    }

    void Detection()
    {
        if (nextMove != 0)
        {
            if (nextMove == -1)
            {
                Debug.DrawRay(rigid.position, Vector3.left, new Color(1, 0, 0));
                RaycastHit2D attack = Physics2D.Raycast(rigid.position, Vector3.left, 3, LayerMask.GetMask("Player"));
                if (attack.collider != null)
                {
                    if(rage==0)
                        Invoke("Attack", 1);
                }
                else
                {
                    rage = 0;
                }
            }
            else
            {
                Debug.DrawRay(rigid.position, Vector3.right, new Color(1, 0, 0));
                RaycastHit2D attack = Physics2D.Raycast(rigid.position, Vector3.right, 3, LayerMask.GetMask("Player"));
                if (attack.collider != null)
                {
                    if (rage == 0)
                        Invoke("Attack", 1);
                }
                else
                {
                    rage = 0;
                }
            }
        }
    }

    void Attack()
    {
        exmark = true;  // Activate the exclamation mark
        rage = 1;
        CancelInvoke();
        /*
         * 데미지 주는 명령어
         */
        rigid.velocity = new Vector2(rigid.velocity.x + nextMove * 5, rigid.velocity.y);
        rigid.AddForce(Vector3.up * 3, ForceMode2D.Impulse);
        Invoke("Brake", 0.3f);
        rage = 2;
        Invoke("Move", 1);
        Invoke("Think", 2);
    }

    void Brake()
    {
        rigid.velocity = new Vector2(rigid.velocity.x + nextMove * (-1), rigid.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            anim.SetTrigger("damaged");
        }
    }
}
