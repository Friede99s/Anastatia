using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    AntSpawner spawner;
    Transform player;
    public int nextMove;
    public float speed;
    private bool rage;
    public int hp;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        spawner = GameObject.FindWithTag("AntSpawner").GetComponent<AntSpawner>();
        player = GameObject.Find("player").transform;
        rage = false;
        Invoke("Think", 1);
    }

    // Update is called once per frame
    void Update()
    {
        Detection();
        PlatformCheck();
        //Check is it dead
        if (hp <= 0)
        {
            Destroy(gameObject);
            spawner.spawnCnt -= 1;
        }

        if(rage == true)
        {
            if (transform.position.x - player.position.x < 0)
            {
                sprite.flipX = true;
                rigid.velocity = new Vector2(speed, rigid.velocity.y);
            }
            else if (Mathf.Abs(transform.position.x - player.position.x) <= 0.3f)
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            else
            {
                sprite.flipX = false;
                rigid.velocity = new Vector2(-speed, rigid.velocity.y);
            }
        }
        else
        {
            rigid.velocity = new Vector2(nextMove*speed, rigid.velocity.y);
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float randThinkTime = Random.Range(1, 5);
        Invoke("Think", randThinkTime);
        //Change Direction
        if (nextMove != 0)
            sprite.flipX = nextMove == 1;

    }

    void Turn()
    {
        nextMove *= -1;
        sprite.flipX = nextMove == 1;
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
        if (Mathf.Abs(player.position.x - transform.position.x) <= 8)
        {
            if (Mathf.Abs(player.position.y - transform.position.y) <= 3)
                rage = true;
            else
                rage = false;
        }
        else
            rage = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            hp--;
        }
    }
}
