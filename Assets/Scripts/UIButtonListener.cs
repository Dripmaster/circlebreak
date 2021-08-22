using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonListener : MonoBehaviour
{
    [SerializeField] SpriteRenderer whiteCover;
    private void Start()
    {

        SoundManager.Singleton.PlayMusic(SoundManager.Singleton.titleMusic);
    }
    IEnumerator CoverToStartCoroutine()
    {
        float eTime = 0f;
        while (eTime < 1)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
            float x = TimeCurves.ExponentialMirrored( eTime / 1);
            whiteCover.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, x));
        }
        SceneManager.LoadScene("StageSelectScene");
    }
    public void switchToGameScene()
    {
        SoundManager.Singleton.PlaySound(SoundManager.Singleton.buttonClickSound);
        StartCoroutine(CoverToStartCoroutine());
    }

    public void exitGame()
    {
        SoundManager.Singleton.PlaySound(SoundManager.Singleton.buttonClickSound);
        Application.Quit();
    }
    
}
