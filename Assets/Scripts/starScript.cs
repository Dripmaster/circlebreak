using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class starScript : MonoBehaviour
{
    SpriteRenderer sr;
    float al;
    float speed;
    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        Color c = sr.color;
        c.a = 0.5f;
        al = c.a;
        sr.color = c;
        speed = Random.Range(0.1f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        al += Time.deltaTime * speed;
        Color c = sr.color;
        c.a= al;
        sr.color = c;
        if (al >= 1)
        {
            speed *= -1;
        }
        if (al <=0.45f)
        {
            speed *= -1;
        }
    }
}
