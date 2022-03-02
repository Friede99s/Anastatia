using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplemanMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 3.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayhit.collider == null)
        {
            Turn();
        }

        //Player Detection
        if(nextMove != 0)
        {
            Vector2 detectVec = new Vector2(rigid.position.x + nextMove * 2, rigid.position.y);
            if(nextMove == -1)
            {
                Debug.DrawRay(detectVec, Vector3.left, new Color(1, 0, 0));
                RaycastHit2D attack = Physics2D.Raycast(detectVec, Vector3.left, 1, LayerMask.GetMask("Player"));
                if (attack.collider != null)
                {
                    rigid.velocity = new Vector2(0, rigid.velocity.y);
                    Attack(detectVec);
                }
            }
            else
            {
                Debug.DrawRay(detectVec, Vector3.right, new Color(1, 0, 0));
                RaycastHit2D attack = Physics2D.Raycast(detectVec, Vector3.right, 1, LayerMask.GetMask("Player"));
                if (attack.collider != null)
                {
                    rigid.velocity = new Vector2(0, rigid.velocity.y);
                    Attack(detectVec);
                }
            }
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float randThinkTime = Random.Range(2f, 5f);
        Invoke("Think", randThinkTime);
        //Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);
        //Change Direction
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 3.5f);
    }

    void Attack(Vector2 playerPos)
    {
        while(this.transform.position.x != playerPos.x)
        {
            rigid.velocity = (new Vector2(nextMove * 3, rigid.velocity.y));
        }
    }
}
