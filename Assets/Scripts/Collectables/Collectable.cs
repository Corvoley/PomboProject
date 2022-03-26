using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private SpriteRenderer sprite;
    public void OnPickup()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        AudioUtility.PlayAudioCue(audioSource, pickupSound);
        sprite.enabled = false;
        Destroy(gameObject,pickupSound.length);
    }
    
}
