/*
 * PlayerController.cs
 * -------------------
 * Handles player movement, jumping, and animation for a 2D physics-based platformer.
 *
 * SETUP:
 *  1. Attach to a GameObject with Rigidbody2D and Collider2D.
 *  2. Assign an Animator with the following parameters:
 *       - "Speed"      (Float)   — magnitude of horizontal input; 0 = idle, >0 = running
 *       - "IsGrounded" (Bool)    — true while the player is on the ground
 *       - "IsJumping"  (Bool)    — true while ascending in the air (velocity.y > 0)
 *       - "IsFalling"  (Bool)    — true while descending in the air (velocity.y < 0)
 *       - "IsShooting" (Trigger) — fired by PlayerShoot via TriggerShootAnimation()
 *  3. Set the Ground Layer mask in the Inspector to match your tilemap/ground layer.
 *  4. Assign a groundCheck child Transform positioned just below the player's feet.
 *  5. Assign jumpSound and doubleJumpSound (AudioClips) in the Inspector. Requires AudioSource (auto-added).
 *
 * ANIMATOR SETUP GUIDE:
 *  Idle   — default state; transition out when Speed > 0.1 (→ Run) or IsGrounded=false (→ Jump)
 *  Run    — transition out when Speed < 0.1 (→ Idle) or IsGrounded=false (→ Jump)
 *  Jump   — plays while IsJumping=true; transition to Fall when IsJumping=false & IsGrounded=false
 *  Fall   — plays while IsFalling=true; transition back to Idle/Run when IsGrounded=true
 *  Shoot  — any-state transition on IsShooting trigger; return to previous state after clip
 *
 * USAGE:
 *  - Left/Right arrow keys (or A/D) to move.
 *  - Space to jump. Press again in the air for a double jump.
 *  - Shooting is triggered by PlayerShoot on this same GameObject.
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip doubleJumpSound;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private bool isGrounded;
    private float horizontalInput;
    private int jumpsRemaining;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
            jumpsRemaining = 2;

        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            AudioClip sound = (jumpsRemaining == 2) ? jumpSound : doubleJumpSound;
            if (sound != null) audioSource.PlayOneShot(sound);

            jumpsRemaining--;
        }

        // Flip sprite to face movement direction
        if (horizontalInput > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetBool("IsGrounded", isGrounded);

        // IsJumping: true only while ascending in the air
        animator.SetBool("IsJumping", !isGrounded && rb.linearVelocity.y > 0.05f);

        // IsFalling: true while descending in the air
        animator.SetBool("IsFalling", !isGrounded && rb.linearVelocity.y < -0.05f);
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
