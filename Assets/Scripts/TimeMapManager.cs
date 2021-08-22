using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMapManager : MapManager
{
    public GameObject ClockObject;
    public GameObject YoloObject;
    public GameObject ClockBtnObject;
    public AudioClip BGMclip;
    public GameObject bigNiddle;
    public GameObject smallNiddle;
    
    public float normalTimeScale = 5;
    float timeCounter;
    float timeScalar;
    float yoloScalar;
    public override void OnReadyDone()
    {
        timeCounter = 0;
        timeScalar = 1;
        StartCoroutine(timeCount());
        StartCoroutine(timeSpawn());
        soundManager.PlayMusic(BGMclip);
    }
    public override void onDie()
    {
        base.onDie();
        soundManager.StopMusic();
    }
    public override void OnGameClear()
    {
        base.OnGameClear();

        soundManager.StopMusic();
    }
    private void Update()
    {
        if (player.isFever())
        {
            yoloScalar = 5;
        }
        else
        {
            yoloScalar = 1;
        }
    }
    private new void OnEnable()
    {
        base.OnEnable();

        ActionClass a2 = new ActionClass();
        a2.startTime = 9f;
        a2.coroutineName = "spawnClock";
        Actions.Add(a2);
        ActionClass a5 = new ActionClass();
        a5.startTime = 18f;
        a5.coroutineName = "spawnClock";
        Actions.Add(a5);
        ActionClass a1 = new ActionClass();
        a1.startTime = 20f;
        a1.coroutineName = "spawnYolo";
        Actions.Add(a1);

    }
    public float timesc()
    {
        return timeScalar* yoloScalar;
    }
    public void setTimesc(float v)
    {
        timeScalar = v/yoloScalar;
    }

    IEnumerator spawnYolo()
    {
        GameObject g = centerSpawner.SpawnButton(YoloObject);
        g.transform.Rotate(0, 0, 90);
        yield return null;
    }
    IEnumerator spawnClock()
    {
        GameObject g = centerSpawner.SpawnButton(ClockBtnObject);
        g.transform.Rotate(0, 0, 90);
        yield return null;
    }
    IEnumerator timeCount()
    {
        do
        {
            float tmpTime = Time.deltaTime * timeScalar* yoloScalar * normalTimeScale;
            smallNiddle.transform.Rotate(0, 0, -tmpTime);
            bigNiddle.transform.Rotate(0, 0, -tmpTime / 12f);
            yield return null;
        } while (true);
    }
    IEnumerator timeSpawn()
    {
        do
        {
            GameObject g = Instantiate(ClockObject);
            g.GetComponent<clockScript>().mgr = this;
            g.transform.position = new Vector3(Random.Range(13f,11f+11f), Random.Range(0f, -9f), 0);
            yield return new WaitForSeconds(2);
        } while (true);
    }

}
