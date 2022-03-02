using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beet : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    public int nextMove;
    bool rage = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 3.5f);
        Invoke("LittleJump", 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Check Rage
        if (rage == false)
            anim.SetBool("atk", false);
        else
            anim.SetBool("atk", true);

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.8f, rigid.position.y);
        RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayhit.collider == null)
        {
            Turn();
        }

        //Detection
        Detection();
    }

    void LittleJump()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        rigid.AddForce(Vector3.up*3, ForceMode2D.Impulse);
        Invoke("LittleJump", 1);
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float randThinkTime = Random.Range(2f, 5f);
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
        Invoke("LittleJump", 1);
        Invoke("Think", 3.5f);
    }

    void Detection()
    {
        if (nextMove != 0)
        {
            //Vector2 detectVec = new Vector2(rigid.position.x + nextMove * 5, rigid.position.y);
            if (nextMove == -1)
            {
                Debug.DrawRay(rigid.position, Vector3.left, new Color(1, 0, 0));
                RaycastHit2D attack = Physics2D.Raycast(rigid.position, Vector3.left, 5, LayerMask.GetMask("Player"));
                if (attack.collider != null)
                {
                    Attack();
                }
                else
                    rage = false;
            }
            else
            {
                Debug.DrawRay(rigid.position, Vector3.right, new Color(1, 0, 0));
                RaycastHit2D attack = Physics2D.Raycast(rigid.position, Vector3.right, 5, LayerMask.GetMask("Player"));
                if (attack.collider != null)
                {
                    Attack();
                }
                else
                    rage = false;
            }
        }
    }

    void Attack()
    {
        rage = true;
        CancelInvoke();
        rigid.velocity = new Vector2(rigid.position.x + nextMove * 4, rigid.velocity.y);
        Invoke("LittleJump", 2);
        Invoke("Think", 4.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerWeapon")
        {
            anim.SetTrigger("damaged");
        }
    }
}
