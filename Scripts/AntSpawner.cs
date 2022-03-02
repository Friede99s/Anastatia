using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    public int maxSpawn;
    public int spawnCnt;
    public float spawnCool;
    private float spawnCur;
    private Vector2 spawnPos;
    public GameObject ant;
    void Start()
    {
        spawnCur = spawnCool;
        spawnPos = new Vector2(transform.position.x, transform.position.y + 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        spawnCur -= Time.deltaTime;
        if(spawnCur <= 0)
        {
            if (spawnCnt < maxSpawn)
            {
                Instantiate(ant, spawnPos, Quaternion.identity);
                spawnCnt++;
            }
            Debug.Log(spawnCnt);
            spawnCur = spawnCool;
        }
    }
}
