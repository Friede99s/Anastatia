using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicBoomMove : MonoBehaviour
{
    public PlayerMove player;
    Vector3 dir;

    void Awake()
    {
        player = GetComponent<PlayerMove>();
    }

    void Start()
    {
        // Calculating player's direction
        dir = player.transform.position - this.transform.position;
    }

    void Update()
    {
        // Moving to player
        transform.position += dir * 3 * Time.deltaTime;

        // Destroy Gameobject
        if(this.transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
