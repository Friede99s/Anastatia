using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public bool isFlying;
    public bool isAtking;
    public int nextMove;
    public float span;
    //public PlayerMove player;
    public GameObject sonicPrefab;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //player = GetComponent<PlayerMove>();  //PlayerMove is the script which is Player's
        isFlying = false;
        isAtking = false;
        span = 10;
    }

    void FixedUpdate()
    {
        //Fly and Walk motion change delay
        span -= Time.deltaTime;
        if(span > 5)
        {
            isFlying = false;
        }
        else if(span > 0)
        {
            isFlying = true;
        }
        else
        {
            span = 10;
        }

        //Change Animation when going to walk
        if(isFlying == false && rigid.velocity.y < 0)
        {
            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayhit.collider != null)
            {
                if (rayhit.distance < 0.5f)
                {
                    anim.SetBool("isFlying", false);
                }
            }
        }

        //Player Detection while Flying in the sky
        if(isFlying == true)
        {
            Vector2 detectVec = new Vector2(rigid.position.x + nextMove * 3, rigid.position.y - 3);
            if(nextMove == -1)
            {
                RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, detectVec, 3, LayerMask.GetMask("Player"));
                if (rayhit.collider != null)
                {
                    Attack();
                }
            }
            else
            {
                RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, detectVec, 3, LayerMask.GetMask("Player"));
                if (rayhit.collider != null)
                {
                    Attack();
                }
            }
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float randomThinkTime = Random.Range(2f, 5f);
        Invoke("Think", randomThinkTime);
        
        //Change Direction
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;
    }

    void Flying()
    {
        rigid.AddForce(new Vector2(0, 4));
    }

    void Attack()
    {
        GameObject go = Instantiate(sonicPrefab) as GameObject;
        go.transform.position = this.transform.position;
    }


}
