using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SpaceMapManager : MapManager
{
    public GameObject YoloObject;
    public GameObject starObject;
    public AudioClip BGMclip;
    public PostProcessProfile post;
    public PostProcessVolume p;

    public float normalTimeScale = 5;
    public override void OnReadyDone()
    {
        soundManager.PlayMusic(BGMclip);
    }
    private void Update()
    {
    }
    private new void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(spawnStar());
        post = p.profile;
        ActionClass a2 = new ActionClass();
        a2.startTime = 15f;
        a2.coroutineName = "distort";

        ActionClass a3 = new ActionClass();
        a3.startTime = 30f;
        a3.coroutineName = "spawnYolo";
        Actions.Add(a2);
    }
    IEnumerator spawnStar()
    {
        do
        {
            yield return new WaitForSeconds(5);
            centerSpawner.SpawnButton(starObject,5);
        } while (!player.isClear()&&!player.isDie());
    }
    IEnumerator spawnYolo()
    {
        GameObject g = centerSpawner.SpawnButton(YoloObject);
        g.transform.Rotate(0, 0, 90);
        yield return null;
    }
    LensDistortion lensDistortion;
    ChromaticAberration chromaticAberration;
    IEnumerator distort()
    {
        float v = 0;
        float startTime = 0;
        do
        {
            startTime += Time.deltaTime;
            v = 70 * (startTime / 0.5f);
            if (p.profile.TryGetSettings(out lensDistortion))
            {
                lensDistortion.active = true;
                lensDistortion.intensity.value = v;
            }
            yield return null;
        } while (startTime <= 0.5f);
        startTime = 0;
        do
        {
            startTime += Time.deltaTime;
            v = 70 * ((0.5f - startTime) / 0.5f);
            if (p.profile.TryGetSettings(out lensDistortion))
            {
                lensDistortion.active = true;
                lensDistortion.intensity.value = v;
            }
            yield return null;
        } while (startTime <= 0.5f);
        v = 0;
        if (p.profile.TryGetSettings(out lensDistortion))
        {
            lensDistortion.active = true;
            lensDistortion.intensity.value = v;
        }
        yield return null;

    }
    IEnumerator colorDistort()
    {
        float n = 0.111f;
        float startTime = 0;
        do
        {
            startTime += Time.deltaTime;
            n = Mathf.Lerp(0.111f,1,startTime/0.5f);

            if (p.profile.TryGetSettings(out chromaticAberration))
            {
                chromaticAberration.active = true;
                chromaticAberration.intensity.value = n;
            }
            yield return null;
        } while (startTime <= 0.5f);

        yield return new WaitForSeconds(0.5f);

        float tmp = n;
        startTime = 0;
        do
        {
            startTime += Time.deltaTime;
            n = Mathf.Lerp(tmp, 0.111f, startTime / 0.5f);

            if (p.profile.TryGetSettings(out chromaticAberration))
            {
                chromaticAberration.active = true;
                chromaticAberration.intensity.value = n;
            }
            yield return null;
        } while (startTime <= 0.5f);

        yield return null;

    }

    public override void onDie()
    {
        StartCoroutine(clearDistort());
    }
    public override void OnGameClear()
    {
        StartCoroutine(clearDistort());
    }
    IEnumerator clearDistort()
    {
        float startTime = 0;
        float n,v;
        float tmp = 0, tmpp = 0;
        if (p.profile.TryGetSettings(out chromaticAberration))
        {
            chromaticAberration.active = true;
            tmp = chromaticAberration.intensity.value;
        }
        if (p.profile.TryGetSettings(out lensDistortion))
        {
            chromaticAberration.active = true;
            tmpp = lensDistortion.intensity.value;
        }
        do
        {
            startTime += Time.deltaTime;
            n = Mathf.Lerp(tmp, 0.111f, startTime / 0.5f);
            v = Mathf.Lerp(tmpp, 0, startTime / 0.5f);

            if (p.profile.TryGetSettings(out chromaticAberration))
            {
                chromaticAberration.active = true;
                chromaticAberration.intensity.value = n;
            }
            if (p.profile.TryGetSettings(out lensDistortion))
            {
                lensDistortion.active = true;
                lensDistortion.intensity.value = v;
            }
            yield return null;
        } while (startTime <= 0.5f);
    }
}
