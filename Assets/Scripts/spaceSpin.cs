using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceSpin : MonoBehaviour
{
    public playerMovwe p;
    float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (p.isFever())
        {
            speed = 10;
        }
        else
        {
            speed = 5;
        }

        transform.Rotate(0,0,-Time.deltaTime*speed);
    }
}
