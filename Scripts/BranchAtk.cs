using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchAtk : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform playerPos;
    public GameObject branch;

    void Update()
    {
        // Set Angle
        Vector2 dir = (playerPos.position - transform.position).normalized;
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(branch, spawnPoint.position, transform.rotation);
        }    
    }
}
