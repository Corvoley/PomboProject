using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip startMenuMusic;
    [SerializeField] private AudioClip mainTrackMusic;

    private AudioSource audioSource;
    public AudioSource AudioSource => audioSource == null ? audioSource = GetComponent<AudioSource>() : audioSource;


    public void PlayStartMenuMusic()
    {
        PlayMusic(startMenuMusic);
    }
    public void PlayMainTrackMusic()
    {
        PlayMusic(mainTrackMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        AudioUtility.PlayMusic(AudioSource,clip);
    }
    private void StopMusic()
    {
        AudioSource.Stop();
    }
}
