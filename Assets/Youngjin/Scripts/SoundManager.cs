using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip dashSound;
    public AudioClip blockBreakSound;
    public AudioClip boomSound;
    public AudioClip yoloSound;
    public AudioClip trapSound;
    public AudioClip deathSound;
    public AudioClip deathSound2;
    public AudioClip clearSound;
    public AudioClip endSound;

    public AudioClip buttonClickSound;
    public AudioClip mapSelectSound;
    public AudioClip mapMoveSound;

    public AudioClip titleMusic;

    public AudioClip duduMusic;



    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;

    public static SoundManager Singleton { get { return instance; } }
    static SoundManager instance = null;

    protected void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<SoundManager>();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    public void PlayMusic(AudioClip music)
    {
        if (music == null)
            return;
        if (musicSource.clip == music)
            return;
        musicSource.Stop();
        musicSource.clip = music;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PlaySound(AudioClip sound)
    {
        PlaySound(sound, 1f);
    }
    public void PlaySound(AudioClip sound, float volume)
    {
        if (sound != null)
        {
            sfxSource.PlayOneShot(sound, volume);
        }
    }
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
}
