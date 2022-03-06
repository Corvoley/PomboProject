using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterFacing2D))]
public class EnemyAIController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 movementInput;
    CharacterFacing2D enemyFacing;
    [Header("Movement")]
    [SerializeField] private bool startWalkLeft;
    [Range(0.0f, 10.0f)]
    [SerializeField] float moveSpeed; 
    [Range(0.0f, 10.0f)]
    [SerializeField] float walkTime = 2;
    [Range(0.0f, 10.0f)]
    [SerializeField] float waitTime = 2;



    private void Start()
    {
        StartCoroutine(WALK());
        rigidbody2d = GetComponent<Rigidbody2D>();
        enemyFacing = GetComponent<CharacterFacing2D>();
    }
    void Update()
    {
        Movement(movementInput, moveSpeed);
        enemyFacing.UpdateFacing(movementInput);

    }

    IEnumerator WALK()
    {
        while (true)
        {
            if (startWalkLeft)
            {
                movementInput.x = -1;
                yield return new WaitForSeconds(walkTime);
                movementInput.x = 0;
                yield return new WaitForSeconds(waitTime);
                movementInput.x = 1;
                yield return new WaitForSeconds(walkTime);
                movementInput.x = 0;
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                movementInput.x = 1;
                yield return new WaitForSeconds(walkTime);
                movementInput.x = 0;
                yield return new WaitForSeconds(waitTime);
                movementInput.x = -1;
                yield return new WaitForSeconds(walkTime);
                movementInput.x = 0;
                yield return new WaitForSeconds(waitTime);
            }
            
        }
        

    }

    private void Movement(Vector2 movementInput, float moveSpeed)
    {
        float speed = movementInput.x * moveSpeed;
        rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y);
    }
}
