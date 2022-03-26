using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    PlayerInput playerInput;
    GameHandler gameHandler;
    Animator animations;
    CharacterFacing2D playerFacing;
    Vector2 movementInput;
    bool canMove = true;
    bool jumpInput;
    bool jumpReleased;
    bool glideInput;
    bool canBeDamaged;
    public bool GlideInput => glideInput;
    [SerializeField] private ContactFilter2D ventFilter;

    
    private List<Collider2D> colliders = new List<Collider2D>();
    public int VentsCount { get; private set; } = 1;


    BoxCollider2D boxCollider2d;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Text breadCountText;
    [SerializeField] private Text scoreCountText;


    [Header("Health")]
    [SerializeField] private int health;
    [SerializeField] private int numOfHearts;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    [Header("BreadCollectable")]
    [SerializeField] private int breadCount;
    [SerializeField] private int numOfBreads;
    [SerializeField] Image[] breads;
    [SerializeField] Sprite fullBread;
    [SerializeField] Sprite emptyBread;

    [Header("Movement")]
    [Range(1.0f, 10.0f)]
    [SerializeField] private float moveSpeed = 1f;
    [Range(1.0f, 20.0f)]
    [SerializeField] float jumpForce = 1f;
    [Range(1.0f, 5.0f)]
    [SerializeField] float glideScale = 1f;
    [Range(0.0f, 10.0f)]
    [SerializeField] float knockbackDistance = 5f;
    [Range(0.0f, 10.0f)]
    [SerializeField] float knockbackHeight = 5f;
    [Range(0.0f, 20.0f)]
    [SerializeField] float knockbackTime = 2f;
    [Range(0.0f, 2.0f)]
    [SerializeField] float invunerabilityTime = 2f;
    [Range(0.0f, 10.0f)]
    [SerializeField] float jumpKillKnockbackHeight;
    [SerializeField] float fJumpPressedRememberTime = 0.15f;
    [SerializeField] float fGroundedRememberTime = 0.1f;

    float fJumpPressedRemember = 0;
    float fGroundedRemember = 0;
    float invunerabilityTimeRemaining;
    float knockbackTimeRemaining;

    int breadCrumbsCount;
    int score;

    [Header("Camera")]
    public Transform cameraTarget;
    [Range(0.0f, 5.0f)]
    public float cameraTargetOffsetX;
    [Range(0.5f, 50.0f)]
    public float cameraTargetFlipSpeed;
    [Range(0.0f, 5.0f)]
    public float characterSpeedInfluence;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        gameHandler = GameObject.FindObjectOfType<GameHandler>();
        animations = GetComponent<Animator>();
        playerFacing = GetComponent<CharacterFacing2D>();
        breadCrumbsCount = PlayerPrefs.GetInt("BreadCount");
        score = PlayerPrefs.GetInt("Score");
        health = PlayerPrefs.GetInt("Health");
        breadCountText.text = breadCrumbsCount.ToString("D2");

    }

    void Update()
    {        
        KnockbackCounter();
        movementInput = playerInput.GetMovementInput();
        jumpInput = playerInput.IsJumpButtonDown();
        jumpReleased = playerInput.IsJumpButtonUp();
        glideInput = playerInput.IsJumpButtonHeld();
        playerFacing.UpdateFacing(movementInput);
        BreadCollectable();
        Health();
        BreadCrumbsCheck();
        AnimationHandler(movementInput);
        CanBeDamaged();
        scoreCountText.text = score.ToString("D6");

        if (canMove)
        {
            Movement(movementInput, moveSpeed);
            Jump(jumpForce);
        }

       


    }

    private void FixedUpdate()
    {
        Glide(glideScale);
        //controle do target da camera dependendo da direcao do sprite e da velocidade do jogador
        bool isFacingRight = playerFacing.IsFacingRight();
        float targetOffSetX = isFacingRight ? cameraTargetOffsetX : -cameraTargetOffsetX;
        float currentOffsetX = Mathf.Lerp(cameraTarget.localPosition.x, targetOffSetX, cameraTargetFlipSpeed * Time.fixedDeltaTime);
        currentOffsetX += rigidbody2d.velocity.x * characterSpeedInfluence * Time.fixedDeltaTime;
        cameraTarget.localPosition = new Vector3(currentOffsetX, cameraTarget.localPosition.y, cameraTarget.localPosition.z);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable_Big"))
        {
            Destroy(collision.gameObject);
            breadCount += 1;
            score += 5000;
            PlayerPrefs.SetInt("Score", score);

        }

        if (collision.CompareTag("Collectable_Small"))
        {

            collision.GetComponent<Collectable>().OnPickup();
            breadCrumbsCount += 1;
            PlayerPrefs.SetInt("BreadCount", breadCrumbsCount);
            
            score += 100;
            PlayerPrefs.SetInt("Score", score);
            breadCountText.text = breadCrumbsCount.ToString("D2");
        }
        if (collision.CompareTag("DeathPit"))
        {
            rigidbody2d.simulated = false;
            gameHandler.EndGame();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (!IsGrounded())
            {
                Destroy(collision.gameObject);
                JumpKillKnockback();
                score += 1000;
                PlayerPrefs.SetInt("Score", score);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Spikes")
        {
            if (health > 0 && canBeDamaged)
            {

                DoDamage();
                Knockback(collider.gameObject);
                invunerabilityTimeRemaining = invunerabilityTime;
            }
            else if (health <= 0)
            {
                gameHandler.EndGame();
            }

        }

        if (collider.gameObject.tag == "Vent")
        {            
            VentsCount = GetComponent<Collider2D>().OverlapCollider(ventFilter, colliders);            
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (health > 0 && canBeDamaged)
            {

                DoDamage();
                Knockback(collision.gameObject);
                invunerabilityTimeRemaining = invunerabilityTime;
            }
            else if (health <= 0)
            {
                gameHandler.EndGame();
            }

        }

    }

    private void Movement(Vector2 movementInput, float moveSpeed)
    {
        float speed = movementInput.x * moveSpeed;
        rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y);
    }
    private void Jump(float jumpForce)
    {
        fGroundedRemember -= Time.deltaTime;
        if (IsGrounded())
        {
            fGroundedRemember = fGroundedRememberTime;
        }

        fJumpPressedRemember -= Time.deltaTime;
        if (jumpInput)
        {
            fJumpPressedRemember = fJumpPressedRememberTime;
        }
        if (fJumpPressedRemember > 0 && fGroundedRemember > 0)
        {
            fGroundedRemember = 0;
            fJumpPressedRemember = 0;
            rigidbody2d.velocity = Vector2.up * jumpForce;
        }
        if (jumpReleased && rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y * 0.5f);
        }


    }
    private void Glide(float glideScale)
    {
        if (rigidbody2d.velocity.y <= -1f && glideInput)
        {

            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y / glideScale);
        }
        else
        {
           
        }

    }
    private void Knockback(GameObject collision)
    {

        if (collision.gameObject.transform.position.x > transform.position.x)
        {
            //enemy is to my right therefore i should be damaged and move left

            canMove = false;

            rigidbody2d.velocity = new Vector2(-knockbackDistance, knockbackHeight);
            knockbackTimeRemaining = knockbackTime;


        }
        else if (collision.gameObject.transform.position.x < transform.position.x)
        {
            //enemy is to my right therefore i should be damaged and move right
            canMove = false;
            rigidbody2d.velocity = new Vector2(knockbackDistance, knockbackHeight);
            knockbackTimeRemaining = knockbackTime;
        }
    }
    private void JumpKillKnockback()
    {
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpKillKnockbackHeight);
    }
    private void KnockbackCounter()
    {
        if (knockbackTimeRemaining <= 0)
        {
            canMove = true;
        }
        if (knockbackTimeRemaining >= 0)
        {
            knockbackTimeRemaining -= Time.deltaTime;
        }
    }
    private void AnimationHandler(Vector2 movementInput)
    {

        if (!IsGrounded())
        {
            animations.SetBool("IsMoving", false);
            animations.SetBool("IsJumping", true);
        }
        else if (movementInput.x != 0)
        {
            animations.SetBool("IsMoving", true);
            animations.SetBool("IsJumping", false);
        }
        else
        {
            animations.SetBool("IsMoving", false);
            animations.SetBool("IsJumping", false);

        }




    }
    private void DoDamage()
    {
        health--;       
        animations.SetTrigger("WasDamaged");
        PlayerPrefs.SetInt("Health", health);
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, .1f, groundLayerMask);

        //Debug.Log(raycastHit2d.collider);

        return raycastHit2d.collider != null;
    }
    private bool CanBeDamaged()
    {
        if (invunerabilityTimeRemaining <= 0)
        {
            return canBeDamaged = true;
        }
        else if (invunerabilityTimeRemaining >= 0)
        {
            invunerabilityTimeRemaining -= Time.deltaTime;
        }
        return canBeDamaged = false;
    }

    private void Health()
    {
        
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }



        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void BreadCollectable()
    {
        if (breadCount > numOfBreads)
        {
            breadCount = numOfBreads;
        }



        for (int i = 0; i < breads.Length; i++)
        {
            if (i < breadCount)
            {
                breads[i].sprite = fullBread;
            }
            else
            {
                breads[i].sprite = emptyBread;
            }
            if (i < numOfBreads)
            {
                breads[i].enabled = true;
            }
            else
            {
                breads[i].enabled = false;
            }
        }
    }
    private void BreadCrumbsCheck()
    {
        if (breadCrumbsCount >= 100)
        {
            breadCrumbsCount = 0;
            breadCountText.text = breadCrumbsCount.ToString("D2");
            if (health <= hearts.Length)
            {
                health++;
                PlayerPrefs.SetInt("Health", health);
            }
            
        }
    }
}

