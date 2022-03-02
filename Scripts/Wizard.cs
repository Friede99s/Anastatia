using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    public GameObject poison;
    public GameObject enerball;
    Transform player;
    public float cooltime;
    public float ecooltime;
    private float curtime;
    private float ecurtime;
    public int enerballNum;
    private int enerballCnt;
    public int nextMove = 0;
    public int nextAtk = 0;
    private bool rage = false;
    Vector2 poisonPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").transform;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        curtime = cooltime;
        ecurtime = ecooltime;
        enerballCnt = enerballNum;
        poisonPos = new Vector2(player.position.x, player.position.y + 1);
        Invoke("Think", 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlatformCheck();
        Detection();
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        
        if (rage == true)
        {
            anim.SetBool("atk", true);
            if (nextAtk == 0)
            {
                enerballCnt = enerballNum;
                curtime -= Time.deltaTime;
                if (curtime <= 0)
                {
                    PoisonDrop();
                    curtime = cooltime;
                    nextAtk = 1;
                }
            }
            else if (nextAtk == 1)
            {
                ecurtime -= Time.deltaTime;
                EnergyBall();
                if(ecurtime < 0)
                {
                    ecurtime = ecooltime;
                    nextAtk = 0;
                }
                
            }
        }
        else
            anim.SetBool("atk", false);
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

    void PoisonDrop()
    {
        Debug.Log("Poison Drop!");
        nextMove = 0;
        Debug.Log(player);
        Instantiate(poison, poisonPos, Quaternion.identity);
    }

    void EnergyBall()
    {
        Debug.Log("EneryBall!");
        nextMove = 0;

        if (ecurtime >= 2 && ecurtime <= 2.1f)
        {
            if(enerballCnt > 0)
            {
                int i, angle = 0;
                for (i = 0; i < 3; i++)
                {
                    Instantiate(enerball, transform.position, Quaternion.Euler(0, 0, angle));
                    angle += 120;
                }
                enerballCnt--;
            }
        }
        else if (ecurtime >= 1 && ecurtime <= 1.1f)
        {
            if(enerballCnt > -enerballNum)
            {
                int i, angle = 0;
                for (i = 0; i < 4; i++)
                {
                    Instantiate(enerball, transform.position, Quaternion.Euler(0, 0, angle));
                    angle += 90;
                }
                enerballCnt--;
            }
        }
        else if (ecurtime >= 0 && ecurtime <= 0.1f)
        {
            if(enerballCnt < -enerballNum+1)
            {
                int i, angle = 0;
                for (i = 0; i < 6; i++)
                {
                    Instantiate(enerball, transform.position, Quaternion.Euler(0, 0, angle));
                    angle += 60;
                }
                enerballCnt++;
            }
        }
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
