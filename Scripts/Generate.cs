using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public GameObject passiveL;
    public GameObject passiveR;

    void Start()
    {
        Generating();
    }

    // Update is called once per frame
    void Generating()
    {
        int rand = Random.Range(-3, 4);
        Vector3 posL = new Vector3(rand, transform.position.y, 0);
        Vector3 posR = new Vector3(rand+3, transform.position.y, 0);
        Instantiate(passiveL, posL, Quaternion.identity);
        Instantiate(passiveR, posR, Quaternion.identity);
        Invoke("Generating", 2);
    }
}
