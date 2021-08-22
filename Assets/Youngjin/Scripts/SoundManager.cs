using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip blockBreakSound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] AudioClip yoloSound;
    [SerializeField] AudioClip trapSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip beginSound;
    [SerializeField] AudioClip endSound;

    [SerializeField] AudioClip buttonClickSound;
    [SerializeField] AudioClip mapSelectSound;
    [SerializeField] AudioClip mapMoveSound;



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
        if (music != null)
        {
            musicSource.clip = music;
            musicSource.Play();
        }
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
}
