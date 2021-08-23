using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonListener : MonoBehaviour
{
    [SerializeField] Image whiteCover;
    private void Start()
    {

        SoundManager.Singleton.PlayMusic(SoundManager.Singleton.titleMusic);
        StartCoroutine(OpenScene());
    }
    IEnumerator OpenScene()
    {
        whiteCover.gameObject.SetActive(true);
        float eTime = 0f;
        while (eTime < 0.5f)
        {
            yield return null;
            eTime += Time.deltaTime;
            float x = TimeCurves.ExponentialMirrored(eTime / 1);
            whiteCover.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, x));
        }
    }
    IEnumerator CoverToStartCoroutine(string sceneName)
    {
        float eTime = 0f;
        while (eTime < 1)
        {
            yield return null;
            eTime += Time.deltaTime;
            float x = TimeCurves.ExponentialMirrored( eTime / 1);
            whiteCover.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, x));
        }
        SceneManager.LoadScene(sceneName);
    }
    public void switchToGameScene()
    {
        SoundManager.Singleton.PlaySound(SoundManager.Singleton.buttonClickSound);
        StartCoroutine(CoverToStartCoroutine("StageSelectScene"));
    }

    public void ShowCredits()
    {
        SoundManager.Singleton.PlaySound(SoundManager.Singleton.buttonClickSound);
        StartCoroutine(CoverToStartCoroutine("CreditsScene"));
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
