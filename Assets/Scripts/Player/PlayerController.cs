/*
 * PlayerController.cs
 * -------------------
 * Handles player movement, jumping, and animation for a 2D physics-based platformer.
 *
 * SETUP:
 *  1. Attach to a GameObject with Rigidbody2D and Collider2D.
 *  2. Assign an Animator with parameters: "Speed" (float), "IsGrounded" (bool), "IsShooting" (trigger).
 *  3. Set the Ground Layer mask in the Inspector to match your tilemap/ground layer.
 *  4. Gravity Scale on Rigidbody2D is handled by physics; tune MoveSpeed and JumpForce here.
 *
 * USAGE:
 *  - Left/Right arrow keys (or A/D) to move.
 *  - Space to jump (only when grounded).
 *  - Shooting is triggered by PlayerShoot on this same GameObject.
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        // Flip sprite to face movement direction
        if (horizontalInput > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            animator.SetBool("IsGrounded", isGrounded);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    // Called by PlayerShoot to trigger the shoot animation
    public void TriggerShootAnimation()
    {
        if (animator != null)
            animator.SetTrigger("IsShooting");
    }
}
