using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [Header("References")]
    public playerMovwe player;
    protected List<ActionClass> Actions;
    void Awake()
    {
        
    }
    private void OnEnable()
    {
        Actions = new List<ActionClass>();
        StartCoroutine(mainRoutine());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    [System.Serializable]
    public class ActionClass
    {
        public float startTime;
        public string coroutineName;
    }
    IEnumerator test()
    {
        Debug.Log("test");
        yield return null;
    }

    IEnumerator test2()
    {
        Debug.Log("test2");
        yield return null;
    }
    IEnumerator mainRoutine()
    {
        float startTime = 0;
        int idx = 0;
        do
        {
            if (Actions.Count>0&&!player.isReady() && !player.isClear() && !player.isDie())
            {
                startTime += Time.deltaTime;
                if (idx >= Actions.Count)
                {
                    idx = 0;
                    startTime = 0;
                }
                if (startTime >= Actions[idx].startTime)
                {
                    StartCoroutine(Actions[idx].coroutineName);
                    idx++;
                }
            }
            yield return null;
        } while (true);
    }
}
