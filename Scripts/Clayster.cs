using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clayster : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    Transform player;
    public int nextMove;
    private byte rage = 0;
    private float curtime;
    public float sCooltime;
    private float sCurtime;
    GameObject mark;
    noticedmark markC;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("player").transform;
        Invoke("Think", 1);
        curtime = 1.2f;
        sCurtime = sCooltime;
        mark = GameObject.Find("claymark");
        markC = GameObject.Find("claymark").GetComponent<noticedmark>();
        mark.SetActive(false);
    }

    void FixedUpdate()
    {
        Detection();
        PlatformCheck();
        
        switch (rage)
        {
            case 0: // Normal state
                Debug.Log("0");
                sCurtime -= Time.deltaTime;
                if (sCurtime <= 0)
                {
                    rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
                    anim.SetInteger("walkSpeed", nextMove);
                    sCurtime = sCooltime;
                }
                mark.SetActive(false);
                break;
            case 1: // In doubt
                Debug.Log("1");
                rigid.velocity = new Vector2(nextMove * 0.6f, rigid.velocity.y);
                if (nextMove == -1)
                    sprite.flipX = false;
                else if (nextMove == 1)
                    sprite.flipX = true;
                markC.sprite.flipX = sprite.flipX;
                mark.SetActive(true);
                break;
            case 2: // Player detected
                Debug.Log("2");
                if (transform.position.x - player.position.x < 0)
                {
                    if(sprite.flipX == false)
                        sprite.flipX = true;
                    markC.sprite.flipX = sprite.flipX;
                    rigid.velocity = new Vector2(1.5f, rigid.velocity.y);
                }
                else if (Mathf.Abs(transform.position.x - player.position.x) <= 0.3f)
                    rigid.velocity = new Vector2(0, rigid.velocity.y);
                else
                {
                    if (sprite.flipX == true)
                        sprite.flipX = false;
                    markC.sprite.flipX = sprite.flipX;
                    rigid.velocity = new Vector2(-1.5f, rigid.velocity.y);
                }
                mark.SetActive(true);
                break;
            case 3: // Attack
                Debug.Log("3");
                if (transform.position.x - player.position.x < 0)
                {
                    if (sprite.flipX == false)
                        sprite.flipX = true;
                    markC.sprite.flipX = sprite.flipX;
                    rigid.velocity = new Vector2(0.6f, rigid.velocity.y);
                }
                else if (Mathf.Abs(transform.position.x - player.position.x) <= 0.4f)
                {
                    rigid.velocity = new Vector2(0, rigid.velocity.y);
                    Attack();
                }
                else
                {
                    if (sprite.flipX == true)
                        sprite.flipX = false;
                    markC.sprite.flipX = sprite.flipX;
                    rigid.velocity = new Vector2(-0.6f, rigid.velocity.y);
                }
                break;
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float randThinkTime = Random.Range(1, 5);
        Invoke("Think", randThinkTime);
        //Change Direction
        if (nextMove != 0)
        {
            sprite.flipX = nextMove == 1;
            markC.sprite.flipX = sprite.flipX;
        }

    }

    void Attack()
    {
        CancelInvoke();
        do
        {
            if (rage == 3)
            {
                anim.SetTrigger("atk");
                curtime -= Time.deltaTime;
            }
        } while (curtime > 0);
        curtime = 1.2f;
        Invoke("Think", 2);
    }

    void Turn()
    {
        nextMove *= -1;
        sprite.flipX = nextMove == 1;
        markC.sprite.flipX = sprite.flipX;
        CancelInvoke();
        Invoke("Think", 1.5f);
    }

    void PlatformCheck()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayhit.collider == null)
        {
            Turn();
        }
    }

    void Detection()
    {
        if (Mathf.Abs(player.position.x - transform.position.x) <= 2)
        {
            rage = 3;
        }
        else if(Mathf.Abs(player.position.x - transform.position.x) <= 4)
        {
            rage = 2;
        }
        else if(Mathf.Abs(player.position.x - transform.position.x) <= 6)
        {
            rage = 1;
        }
        else
        {
            rage = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            CancelInvoke();
            anim.SetTrigger("damaged");
            nextMove = 0;
            curtime = 1;
            while (curtime > 0)
                curtime -= Time.deltaTime;
            curtime = 1.2f;
            Invoke("Think", 0);
        }
    }
}
