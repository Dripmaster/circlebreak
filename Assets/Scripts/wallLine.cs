using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallLine : MonoBehaviour
{
    wallBase wall;
    // Start is called before the first frame update
    void Awake()
    {
        wall = transform.parent.GetComponent<wallBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        wall.OnLineEnter(collision);
    }
}
