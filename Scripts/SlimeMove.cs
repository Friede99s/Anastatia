using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;
    private bool isAttacking;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 3.5f);
        isAttacking = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isAttacking == false)
        {
            //Move
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

            //Platform Check
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayhit.collider == null)
            {
                Turn();
            }

            //Player Detection
            if (nextMove != 0)
            {
                Vector2 detectVec = new Vector2(rigid.position.x, rigid.position.y);
                //If Player is in the left
                if (nextMove == -1)
                {
                    Debug.DrawRay(detectVec, Vector3.left, new Color(1, 0, 0));
                    RaycastHit2D attack = Physics2D.Raycast(detectVec, Vector3.left, 1.5f, LayerMask.GetMask("Player"));
                    if (attack.collider != null)
                    {
                        if (isAttacking != true)
                        {
                            anim.SetBool("isPlayer", true);
                            Attack(detectVec);
                        }
                    }
                    else
                    {
                        if (isAttacking != true)
                        {
                            anim.SetBool("isPlayer", false);
                        }
                    }
                }
                //If Player is in the right
                else
                {
                    Debug.DrawRay(detectVec, Vector3.right, new Color(1, 0, 0));
                    RaycastHit2D attack = Physics2D.Raycast(detectVec, Vector3.right, 1.5f, LayerMask.GetMask("Player"));
                    if (attack.collider != null)
                    {
                        if (isAttacking != true)
                        {
                            anim.SetBool("isPlayer", true);
                            Attack(detectVec);
                        }
                    }
                    else
                    {
                        if (isAttacking != true)
                        {
                            anim.SetBool("isPlayer", false);
                        }
                    }
                }
            }
        }
        // Platform Landing
        if(this.rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(1, 0, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayhit.collider != null)
            {
                if(rayhit.distance < 0.7f)
                {
                    anim.SetBool("isPlayer", false);
                    isAttacking = false;
                }
            }
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float randomThinkTime = Random.Range(2f, 5f);
        Invoke("Think", randomThinkTime);
        //Sprite Animation
        
        //Change Direction
        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 3.5f);
    }

    void Attack(Vector2 playerVec)
    {
        isAttacking = true;
        this.rigid.velocity = new Vector2(playerVec.x * 1.5f, 4);
    }

    
}
