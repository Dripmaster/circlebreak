using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class SampleMapManager : MapManager
{
    public GameObject YoloObject;
    public AudioClip BGMclip;
    public LazerSpawner lazerSpawner;
    public blockSpawner spawner;
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
    public Text text5;
    public Image img;

    public override void OnReadyDone()
    {
        soundManager.PlayMusic(BGMclip);
        StartCoroutine(tutorial());
    }
    private new void OnEnable()
    {
        base.OnEnable();
    }
    IEnumerator tutorial()
    {
        yield return new WaitForSeconds(2);
        img.gameObject.SetActive(true);
        text1.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        img.gameObject.SetActive(false);
        text1.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        img.gameObject.SetActive(true);
        text2.gameObject.SetActive(true);
        yield return StartCoroutine(waitForKey(KeyCode.Q));
        img.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        img.gameObject.SetActive(true);
        text3.gameObject.SetActive(true);
        yield return new WaitForSeconds(6);
        img.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);
        spawner.SpawnBlock(true);
        img.gameObject.SetActive(true);
        text4.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        GameObject g = spawner.SpawnButton(YoloObject);
        g.transform.Rotate(0, 0, 90);

        img.gameObject.SetActive(false);
        text4.gameObject.SetActive(false);
        img.gameObject.SetActive(true);
        text5.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        img.gameObject.SetActive(false);
        text5.gameObject.SetActive(false);
        spawner.passiveSpawn = true;

        ActionClass a1 = new ActionClass();
        a1.startTime = 5;
        a1.coroutineName = "spawnYolo";
        Actions.Add(a1);
    }
    IEnumerator waitForKey(KeyCode k)
    {
        do
        {
            if (Input.GetKey(k))
                break;
            yield return null;
        } while (true);
    }

    IEnumerator spawnYolo()
    {
        GameObject g = centerSpawner.SpawnButton(YoloObject);
        g.transform.Rotate(0, 0, 90);
        yield return null;
    }
}
