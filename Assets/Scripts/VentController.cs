using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentController : MonoBehaviour
{
    [SerializeField] private float ventForce = 5f;
    [SerializeField] private bool isOn;
    
    
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().GlideInput)
            {
                //int numberTouchingPlayer = collision.OverlapCollider(ventLayer, colliders);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (ventForce / collision.gameObject.GetComponent<PlayerController>().VentsCount), ForceMode2D.Impulse);
            }
        }
        
    }
}
