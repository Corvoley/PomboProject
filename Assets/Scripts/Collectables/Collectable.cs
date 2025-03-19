using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public void OnPickup()
    {
        
        sprite.enabled = false;
        Destroy(gameObject);
    }
    
}
