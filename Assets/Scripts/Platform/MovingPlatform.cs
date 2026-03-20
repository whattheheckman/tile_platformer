/*
 * MovingPlatform.cs
 * -----------------
 * Moves a platform back and forth between two points defined in the Scene editor.
 * Players standing on the platform are carried automatically via parent transform.
 *
 * SETUP:
 *  1. Attach to a platform GameObject.
 *  2. In the Inspector, set pointA and pointB (world positions) using the Scene handles,
 *     OR just set them as Vector2 fields — they are the left and right endpoints.
 *  3. Tune moveSpeed.
 *  4. The platform must have a Collider2D. Player Rigidbody2D interpolation should be on
 *     for smooth riding.
 *
 * USAGE:
 *  - Platform oscillates automatically between pointA and pointB.
 *  - When a player (or any Rigidbody2D object tagged "Player") lands on it,
 *    it becomes a child so they are carried.
 */

using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector2 pointA;
    [SerializeField] private Vector2 pointB;
    [SerializeField] private float moveSpeed = 3f;

    private Vector2 target;

    private void Start()
    {
        // Start at pointA in world space (offset from starting position if desired)
        transform.position = pointA;
        target = pointB;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.05f)
            target = (target == pointA) ? pointB : pointA;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Carry anything that lands on top
        if (collision.transform.CompareTag("Player"))
            collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            collision.transform.SetParent(null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(pointA, 0.15f);
        Gizmos.DrawSphere(pointB, 0.15f);
        Gizmos.DrawLine(pointA, pointB);
    }
}
