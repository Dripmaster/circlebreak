using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubMapManager : MapManager
{
    [SerializeField] AudioClip music;

    public GameObject YoloObject;
    private new void OnEnable()
    {
        base.OnEnable();

        ActionClass a1 = new ActionClass();
        a1.startTime = 2f;
        a1.coroutineName = "spawnYolo";
        Actions.Add(a1);
    }

    IEnumerator spawnYolo()
    {
        GameObject g = centerSpawner.SpawnButton(YoloObject);
        g.transform.Rotate(0, 0, 90);
        yield return null;
    }
}
