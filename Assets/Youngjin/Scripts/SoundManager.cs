using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;

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
