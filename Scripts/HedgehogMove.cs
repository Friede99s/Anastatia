using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;
    public bool isAtking;


    void Awake()
    {
        isAtking = false;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 1);
    }

    void FixedUpdate()
    {
        if(isAtking != true)
        {
            if(nextMove != 0)
            {
                anim.SetBool("IsWalking", true);
            }
            else
            {
                anim.SetBool("IsWalking", false);
            }

            //Turn
            spriteRenderer.flipX = nextMove == 1;

            // Move
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

            //Platform Check
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayhit.collider == null)
            {
                Turn();
            }

            // Player Detection
            Vector2 detectVec = new Vector2(rigid.position.x, rigid.position.y);

            //If Player is in the left
            if (nextMove == -1)
            {
                Debug.DrawRay(detectVec, Vector3.left, new Color(1, 0, 0));
                RaycastHit2D attack = Physics2D.Raycast(detectVec, Vector3.left, 6, LayerMask.GetMask("Player"));
                if (attack.collider != null)
                {
                    Debug.Log("공격 실시");
                    isAtking = true;
                    anim.SetBool("IsPlayer", true);
                    anim.SetBool("IsWalking", false);
                    rigid.velocity = new Vector2(nextMove * 6, rigid.velocity.y);
                    Invoke("UnlockAtk", 3);
                    nextMove *= -1;
                }
                
            }
            //If Player is in the right
            else
            {
                Debug.DrawRay(detectVec, Vector3.right, new Color(1, 0, 0));
                RaycastHit2D attack = Physics2D.Raycast(detectVec, Vector3.right, 6, LayerMask.GetMask("Player"));
                if (attack.collider != null)
                {
                    Debug.Log("공격 실시");
                    isAtking = true;
                    anim.SetBool("IsPlayer", true);
                    anim.SetBool("IsWalking", false);
                    rigid.velocity = new Vector2(nextMove * 6, rigid.velocity.y);
                    Invoke("UnlockAtk", 3);
                    nextMove *= -1;
                }

            }
        }
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 3.5f);
    }

    void Think()
    {
        if(isAtking != true)
        {
            nextMove = Random.Range(-1, 2);
            Invoke("Think", 1.5f);
        }
        else
        {
            CancelInvoke("Think");
            Invoke("Think", 2);
        }
    }

    void UnlockAtk()
    {
        Debug.Log("공격중지!");
        isAtking = false;
        anim.SetBool("IsPlayer", false);
    }
}
