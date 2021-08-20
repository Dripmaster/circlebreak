using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blockObject;
    public playerMovwe p;

    public float fillSpeed = 1;

    float scaleFactor;
    Vector2 normalScale;
    void Awake()
    {
        
    }
    private void OnEnable()
    {
        scaleFactor = 1;
        normalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {//이렇게 해도 되고 FSM으로 idle일땐 차오르고 꽝찍어서 쪼그라드는 STATE도 있고 그렇게 해도 되고 그건 나중에..
        scaleFactor += Time.deltaTime* fillSpeed;

        transform.localScale = normalScale * scaleFactor;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnBlocks();
            cutCenter();
            p.setBoom();
        }
    }
    public void cutCenter(float scaleF = 0.5f)
    {
        scaleFactor = scaleF;
    }
    public void SpawnBlocks(int count = 10,float timeOfSpawn = 0.5f)
    {
        float RangeMin = p.rangeMin;
        float RangeMax = p.rangeMax;
        for (int i = 0; i < count; i++)
        {
            var g = Instantiate(blockObject);
            g.transform.parent = transform.parent;
            g.transform.localPosition = Vector2.zero;
            float theta = Random.Range(0,Mathf.PI*2);
            float Range = Random.Range(RangeMin, RangeMax);
            g.GetComponent<blockBase>().setDest(theta,Range,timeOfSpawn,p);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            p.centerCollisionEnter(this);
            Debug.Log("aa");
        }
    }

}
