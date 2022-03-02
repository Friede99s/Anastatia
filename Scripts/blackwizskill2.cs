using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackwizskill2 : MonoBehaviour
{
    SpriteRenderer sprite;
    float brightness=1;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Invoke("Boom", 0.4f);
    }

    void FixedUpdate()
    {
        if (brightness >= 0)
            brightness -= Time.deltaTime/2;
        sprite.color = new Color(sprite.material.color.r, sprite.material.color.g, sprite.material.color.b, brightness);
        if (brightness == 0)
            Destroy(gameObject);
    }

    void Boom()
    {
        transform.Translate(0, 1, 0);
    }
}
