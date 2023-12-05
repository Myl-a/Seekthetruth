using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveX;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int maxJumps;
    [SerializeField] private float idleThreshold = 0.1f;

    private bool IsGrounded
    {
        get { return Physics2D.OverlapCircle(transform.position, 0.2f, LayerMask.GetMask("Ground")); }
    }

    private int availableJumps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        availableJumps = maxJumps;

        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        HandleInput();
        CheckGrounded();
        CheckJumpInput();
    }

    void FixedUpdate()
    {
        Move();
        Attack();
    }

    void HandleInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
    }

    void CheckGrounded()
    {
        if (IsGrounded)
        {
            availableJumps = maxJumps;
        }
    }

    void CheckJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

        if (Mathf.Abs(moveX) > idleThreshold)
        {
            spriteRenderer.flipX = (moveX < 0);
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    }

    void Jump()
    {
        if (IsGrounded || availableJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            availableJumps--;
            anim.SetBool("isJump", true);
        }
        else
        {
            anim.SetBool("isJump", false);  // Reset the isJump parameter when not jumping
        }
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack");  // Use SetTrigger instead of Play for attacks
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isJump", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // You may choose to add additional logic here if needed
        }
    }
}
