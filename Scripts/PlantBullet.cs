using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask isLayer;
    PlantMove plant;
    int dir;

    void Start()
    {
        dir = GameObject.Find("ScaryPlant").GetComponent<PlantMove>().nextMove;
        Invoke("DestroyBullet", 2);    
    }

    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if(ray.collider != null)
        {
            DestroyBullet();
        }

        if(dir == 1)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else if(dir == -1)
        {
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
