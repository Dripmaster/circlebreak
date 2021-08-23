using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseSceneManager : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 0f;
        SoundManager.Singleton.PauseMusic();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            SoundManager.Singleton.ResumeMusic();
            Destroy(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 1f;
            Destroy(gameObject);
            SoundManager.Singleton.PlaySound(SoundManager.Singleton.buttonClickSound);
            SoundManager.Singleton.ResumeMusic();
            SceneManager.LoadScene("TitleScene");
            
        }
    }
}
