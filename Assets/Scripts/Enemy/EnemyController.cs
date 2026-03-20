/*
 * EnemyController.cs
 * ------------------
 * Patrol enemy that walks back and forth on a platform without falling off.
 * Uses 2D raycasts to detect platform edges and walls.
 *
 * SETUP:
 *  1. Attach to an enemy GameObject with Rigidbody2D and Collider2D.
 *  2. Set groundLayer to match your tilemap/ground layer.
 *  3. Tune moveSpeed, edgeCheckDistance, and wallCheckDistance in the Inspector.
 *
 * USAGE:
 *  - Enemy automatically patrols. It flips direction at platform edges or walls.
 *  - Deals damage to the player on contact (calls PlayerHealth.TakeDamage).
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float edgeCheckDistance = 0.5f;
    [SerializeField] private float wallCheckDistance = 0.3f;
    [SerializeField] private int damageToPlayer = 1;

    private Rigidbody2D rb;
    private int direction = 1; // 1 = right, -1 = left
    private float colliderHalfWidth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        colliderHalfWidth = GetComponent<Collider2D>().bounds.extents.x;
    }

    private void FixedUpdate()
    {
        MoveAndPatrol();
    }

    private void MoveAndPatrol()
    {
        // Edge detection: cast down from the forward-bottom corner
        Vector2 edgeCheckOrigin = new Vector2(
            transform.position.x + direction * (colliderHalfWidth + 0.05f),
            transform.position.y
        );

        bool groundAhead = Physics2D.Raycast(edgeCheckOrigin, Vector2.down, edgeCheckDistance, groundLayer);

        // Wall detection: cast forward from center
        Vector2 wallCheckOrigin = (Vector2)transform.position;
        bool wallAhead = Physics2D.Raycast(wallCheckOrigin, new Vector2(direction, 0f), wallCheckDistance + colliderHalfWidth, groundLayer);

        if (!groundAhead || wallAhead)
            Flip();

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // Face movement direction
        transform.localScale = new Vector3(direction, 1f, 1f);
    }

    private void Flip()
    {
        direction *= -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(damageToPlayer);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize raycasts in Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * edgeCheckDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(direction * wallCheckDistance, 0f, 0f));
    }
}
