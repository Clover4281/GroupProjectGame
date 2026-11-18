using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//  Controls the players movement, movement abilities, animation and gameover/death
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //  player run and jump values
    public float moveSpeed = 6.5f;
    public float jumpForce = 14f;

    //  Player Dash values
    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    //  Player health value
    public int health = 1;

    //  ground check variables for jump/extra jump
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundlayer;
    public int extraJumpValue = 1;

    // coin manager
    public CoinManager cm;

    //  variables to track throughout the game
    private int extraJumps;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    private int facingDirection = 1;
    private float defaultGravityScale;
    private bool isDashing;
    private bool canDash = true;

    //  runs at the start to get and save starting values
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        extraJumps = extraJumpValue;
        defaultGravityScale = rb.gravityScale;
    }

    //  Reads player input every frame
    void Update()
    {
        ReadInput();
    }

    //  refills jumps, calls move function, calls Setanimation function and gives it moveInput
    private void FixedUpdate()
    {
        if (isGrounded() && rb.linearVelocityY <= 0.01f)
        {
            extraJumps = extraJumpValue;
        }
        Move();
        SetAnimation(moveInput);
    }

    //  Reads A/D for movement, Space to jump/doubleJump and left shift to dash(starting coroutine)
    private void ReadInput()
    {
        Keyboard kb = Keyboard.current;

        

        moveInput = 0f;
        if (kb.dKey.isPressed) moveInput = 1f;
        if (kb.aKey.isPressed) moveInput = -1f;

        if (kb.spaceKey.wasPressedThisFrame) Jump();
        if (kb.leftShiftKey.wasPressedThisFrame && canDash) StartCoroutine(Dash());
        if (moveInput != 0f) facingDirection = (int)moveInput;
    }

    //  allows the user to jump if on the ground or double jump if not
    //  cant jump while dashing
    private void Jump()
    {
        if (isDashing) return;

        if (isGrounded())
        { 
            rb.linearVelocityY = jumpForce;
        }
        else if (extraJumps > 0)
        {
            rb.linearVelocityY = jumpForce;
            extraJumps--;
        }
    }

    //  returns true if the player is on the ground
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundlayer);
    }

    //  allows the player to move if not dashing
    private void Move()
    {
        if (isDashing) return;

        rb.linearVelocityX = moveInput * moveSpeed;
    }

    //  chooses the animation, idle, run, jump, or fall
    private void SetAnimation(float moveInput)
    {
        if (isGrounded())
        {
            if (moveInput == 0f)
            {
                animator.Play("PlayerIdle");
            }
            else
            {
                animator.Play("PlayerRun");
            }
        }
        else
        {
            if(rb.linearVelocityY > 0)
            {
                animator.Play("PlayerJump");
            }
            else if(rb.linearVelocityY < 0)
            {
                animator.Play("PlayerFall");
            }

        }
    }

    //  the player will take damage when touching an object marked with "damage"(spike/enemy)
    //  calls die function when health = 0
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Damage")
        {
            health -= 1;

            if(health <= 0)
            {
                Die();
            }
        }
    }

    //  Restarts the level 
    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    //  Coroutine that allows the player to dash, turning gravity on and off
    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        rb.gravityScale = 0;
        rb.linearVelocityX = facingDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        rb.gravityScale = defaultGravityScale;
        rb.linearVelocityX *= 0.5f;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

    //coin counter and destroying coins
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
    }
}
