using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [Header("References")]
    public playerMovwe player;
    public blockSpawner centerSpawner;
    [SerializeField] protected List<ActionClass> Actions;
    void Awake()
    {
    }
    protected void OnEnable()
    {
        Actions = new List<ActionClass>();
        StartCoroutine(mainRoutine());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void OnReadyDone()
    {

    }
    public virtual void onDie()
    {

    }
    public virtual void OnYolo()
    {

    }
    public virtual void OnGameClear()
    {

    }
    [System.Serializable]
    public class ActionClass
    {
        public float startTime;
        public string coroutineName;
        public ActionClass(float startTime, string coroutineName)
        {
            this.startTime = startTime;
            this.coroutineName = coroutineName;
        }
        public ActionClass()
        {
        }
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
    public IEnumerator mainRoutine()
    {
        float startTime = 0;
        int idx = 0;
        do
        {
            if (Actions.Count>0&&!player.isReady() && !player.isClear() && !player.isDie())
            {
                startTime += Time.unscaledDeltaTime;
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
