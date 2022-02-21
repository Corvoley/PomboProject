using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    private Rigidbody2D rigidbody2d;
    
    private BoxCollider2D boxCollider2d;
    [SerializeField]
    private float playerGravityGlideScale;
    private int breadCount;
    private GameHandler gameHandler;
    
    

    [SerializeField] private float move_speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Text breadText;

    private Animator animations;


    private void Awake()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        animations = GetComponent<Animator>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        
    }
    private void Start()
    {
        gameHandler = GameObject.FindObjectOfType<GameHandler>();

    }
    private void Update()
    {

        HandleMovement();

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            Destroy(collision.gameObject);
            breadCount += 1;


            breadText.text = breadCount.ToString();

        }
        if (collision.CompareTag("DeathPit"))
        {
            rigidbody2d.simulated = false;
            gameHandler.EndGame();
        }
    }



    private void HandleMovement()
    {
        float movingX = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rigidbody2d.velocity = Vector2.up * jumpForce;
            }
        }
        if (rigidbody2d.velocity.y <= -1f && Input.GetKey(KeyCode.Space))
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y / playerGravityGlideScale);  
        }

        if (Input.GetKey(KeyCode.D))
        {
            rigidbody2d.velocity = new Vector2(+move_speed, rigidbody2d.velocity.y);
            movingX = -1f;
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                rigidbody2d.velocity = new Vector2(-move_speed, rigidbody2d.velocity.y);
                movingX = +1f;
            }
            else
            {
                rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
            }
        }
        bool isIdle = movingX == 0;

        if (!IsGrounded())
        {
            
            animations.SetBool("IsJumping", true);
            animations.SetBool("IsMoving", false);
            animations.SetFloat("MovimentDir", movingX);
            

        } else
        
            if (isIdle)
            {
                animations.SetBool("IsJumping", false);
                animations.SetBool("IsMoving", false);
            
            }
            else
            {
                animations.SetBool("IsJumping", false);
                animations.SetBool("IsMoving", true);
                animations.SetFloat("MovimentDir", movingX);

            } 
        
        Debug.Log(movingX);




    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, .1f, groundLayerMask);

        //Debug.Log(raycastHit2d.collider);

        return raycastHit2d.collider != null;
    }


}







