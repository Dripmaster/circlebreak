using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SpaceMapManager : MapManager
{
    public GameObject YoloObject;
    public GameObject starObject;
    public AudioClip BGMclip;
    public AudioClip distortionSound;
    public AudioClip starfallSound;
    public PostProcessProfile post;
    public PostProcessVolume p;
    public float starSpawnTime = 5f;

    public float normalTimeScale = 5;
    public override void OnReadyDone()
    {
        soundManager.PlayMusic(BGMclip);
    }
    bool distorted = false;
    private void Update()
    {
        if(!distorted && player.isFever())
        {
            distorted = true;
            StartCoroutine(colorDistort());
        }
    }
    private new void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(spawnStar());
        distorted = false;
        post = p.profile;
        ActionClass a2 = new ActionClass();
        a2.startTime = 15f;
        a2.coroutineName = "distort";
        Actions.Add(a2);

        ActionClass a3 = new ActionClass();
        a3.startTime = 20;
        a3.coroutineName = "spawnYolo";
        Actions.Add(a3);
    }
    IEnumerator spawnStar()
    {
        do
        {
            yield return new WaitForSeconds(starSpawnTime);
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
        distorted = false;
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
        do
        {
            yield return null;
        } while (!player.isFever());

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
        distorted = false;
    }

    public override void onDie()
    {
        soundManager.StopMusic();
        StartCoroutine(clearDistort());
    }
    public override void OnGameClear()
    {
        soundManager.StopMusic();
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
