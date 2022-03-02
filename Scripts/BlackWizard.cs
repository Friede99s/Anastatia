using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWizard : MonoBehaviour
{
    public int nextMove;
    public float cooltime;
    private float curtime;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public bool rage;
    public Transform player;
    public GameObject skill1L;
    public GameObject skill1R;
    public GameObject skill2;
    public GameObject skill3;
    Vector2 pos;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Invoke("Think", 1);
        curtime = cooltime;
    }
    
    void FixedUpdate()
    {
        pos = new Vector2(transform.position.x, transform.position.y);
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        Detection();
        PlatformCheck();

        curtime -= Time.deltaTime;

        if(rage == true)
        {
            if(curtime <= 0)
            {
                anim.SetBool("atk", true);
                int skill = Random.Range(0, 5);
                switch (skill)
                {
                    case 0:
                        Debug.Log("Skill1");
                        Skill1();
                        break;
                    case 1:
                    case 2:
                        Debug.Log("Skill2");
                        Skill2();
                        break;
                    case 3:
                    case 4:
                        Debug.Log("Skill3");
                        Skill3();
                        break;
                }
                curtime = cooltime;
            }
        }
        else
        {
            anim.SetBool("atk", false);
        }
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

    void Think()
    {
        Debug.Log("Thinking...");
        nextMove = Random.Range(-1, 2);

        float randThinkTime = Random.Range(1, 5);
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
        Invoke("Think", 1.5f);
    }

    void Detection()
    {

        if (Mathf.Abs(transform.position.x - player.position.x) <= 7)
        {
            if (rage == false) // first detection
            {
                rage = true;
            }
        }
        else
        {
            rage = false;
        }
    }

    void Skill1()
    {
        if (transform.position.x - player.position.x < 0)
            Instantiate(skill1R, pos, Quaternion.identity);
        else
            Instantiate(skill1L, pos, Quaternion.identity);
    }

    void Skill2()
    {
        Instantiate(skill2, new Vector3(player.position.x, -0.6f, 0), Quaternion.identity);
    }
    
    void Skill3()
    {
        Instantiate(skill3, pos, Quaternion.identity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            anim.SetTrigger("damaged");
            // 데미지 받는 명령어
        }
    }
}
