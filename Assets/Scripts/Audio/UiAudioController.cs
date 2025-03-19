using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UiAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip breadSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip enemyHitSound;
    [SerializeField] private AudioClip enemyKillSound;

    private AudioSource audioSource;
    public AudioSource AudioSource => audioSource == null ? audioSource = GetComponent<AudioSource>() : audioSource;

    public void PlayButtonSound()
    {
        Play(buttonSound);
    }
    public void PlayBreadSound()
    {
        Play(breadSound);
    }
    public void PlayCoinSound()
    {
        Play(coinSound);
    }
    public void PlayEnemyHitSound()
    {
        Play(enemyHitSound);
    }

    public void PlayEnemyKillSound()
    {
        Play(enemyKillSound);
    }
    private void Play(AudioClip clip)
    {
        AudioUtility.PlayAudioCue(AudioSource, clip);
    }
}
