using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clockScript : MonoBehaviour
{
    public GameObject bigNiddle;
    public GameObject smallNiddle;
    public TimeMapManager mgr;
    float timeScalar;
    float normalTimeScale;
    private void Awake()
    {
    }
    void OnEnable()
    {
        normalTimeScale = Random.Range(10, 100);
        timeScalar = 1;
        StartCoroutine(timeCount());
    }
    IEnumerator timeCount()
    {
        float startTime = 0;
        do
        {

            yield return null;
            timeScalar = mgr.timesc();
            float tmpTime = Time.deltaTime * timeScalar * normalTimeScale;
            
            smallNiddle.transform.Rotate(0, 0, -tmpTime);
            bigNiddle.transform.Rotate(0, 0, -tmpTime / 12f);
            transform.Translate(-Time.deltaTime*timeScalar*normalTimeScale/10, 0, 0);
            startTime += Time.deltaTime;
            if (startTime >= 10)
            {
                Destroy(gameObject);
            }
        } while (true);
    }
}
