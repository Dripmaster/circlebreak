using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubMapManager : MapManager
{
    [SerializeField] AudioClip music;

    [SerializeField] float beatInterval = 0.37f;


    [SerializeField] GameObject rotateButtonPrefab;
    [SerializeField] GameObject uturnButtonPrefab;

    [SerializeField] GameObject deco;
    [SerializeField] SpriteRenderer whiteCover;
    [SerializeField] Color[] decoColors;
    [SerializeField] Transform background;

    int colorIdx = 0;
    int count = -1;
    string pattern = "01000022110000100011";

    bool isRotating = false;

    internal void OnRotationButton()
    {
        isRotating = !isRotating;
    }
    public override void OnReadyDone()
    {
        soundManager.PlayMusic(music);
    }
    float eTime = 0f;
    public void Update()
    {
        if(isRotating)
        {
            background.Rotate(new Vector3(0, 0, player.turnSpeed * Mathf.Rad2Deg) * Time.deltaTime);
        }
    }
    private new void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(DecoCoroutine());
        StartCoroutine(SpawnRotationButton());
        AddAction(new ActionClass(beatInterval * (3), "Bump"));
        AddAction(new ActionClass(beatInterval * (4), "None"));
    }
    IEnumerator SpawnRotationButton()
    {
        while(true)
        {
            yield return new WaitForSeconds(6);
            SpawnRotation();
            yield return new WaitForSeconds(6);
            SpawnRotation();
            yield return new WaitForSeconds(6);
            SpawnUturn();
            yield return new WaitForSeconds(4);
            SpawnUturn();
        }

    }
    void SpawnRotation()
    {
        GameObject g = centerSpawner.SpawnButton(rotateButtonPrefab);
    }
    void SpawnUturn()
    {
        GameObject g = centerSpawner.SpawnButton(uturnButtonPrefab);
        if(player.FowardDir < 0)
            g.transform.Rotate(0, 0, 180);
    }
    IEnumerator None() { yield return null; }
    IEnumerator DecoCoroutine()
    {
        while(true)
        {
            SpawnDeco();
            yield return new WaitForSeconds(beatInterval);
        }
    }
    void AddAction(ActionClass action)
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            if (Actions[i].startTime > action.startTime)
            {
                Actions.Insert(i, action);
                return;
            }
        }

        Actions.Add(action);
    }
    IEnumerator ShowWhite()
    {
        whiteCover.gameObject.SetActive(true);
        float eTime = 0f;
        while (eTime < 0.3f)
        {
            whiteCover.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, TimeCurves.ExponentialMirrored(eTime / 0.5f)));
            yield return null;
            eTime += Time.unscaledDeltaTime;
        }
        whiteCover.color = new Color(1, 1, 1, 0);
    }
    IEnumerator ShowBlack()
    {
        whiteCover.gameObject.SetActive(true);
        float eTime = 0f;
        while (eTime < 0.3f)
        {
            whiteCover.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, TimeCurves.ExponentialMirrored(eTime / 0.5f)));
            yield return null;
            eTime += Time.unscaledDeltaTime;
        }
        whiteCover.color = new Color(0, 0, 0, 0);
    }
    void SpawnDeco()
    {
        Vector3 pos = new Vector3(Random.Range(-Screen.width, Screen.width),
            Random.Range(-Screen.height, Screen.height), 0) / 200f;
        GameObject go = Instantiate(deco, pos, Quaternion.identity);
        var main = go.GetComponent<ParticleSystem>().main;
        main.startColor = decoColors[colorIdx];
        colorIdx = (colorIdx + 1) % decoColors.Length;
        go.SetActive(true);
        count++;
    }
    IEnumerator Bump()
    {
        StartCoroutine(ShowWhite());
        float eTime = 0f;
        while (eTime < 0.3f)
        {
            background.localScale = Mathf.Lerp(1.1f, 1, eTime / 0.3f) * new Vector3(1,1,1);
            yield return null;
            eTime += Time.unscaledDeltaTime;
        }
        background.localScale = new Vector3(1, 1, 1);
    }

}
