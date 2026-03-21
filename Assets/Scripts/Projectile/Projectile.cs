/*
 * Projectile.cs
 * -------------
 * A projectile fired by the player. Moves in a straight line, damages enemies,
 * and destroys itself on hitting terrain or after a lifetime.
 *
 * SETUP:
 *  1. Create a prefab with this script, a Rigidbody2D (Kinematic or Dynamic),
 *     and a Collider2D set as Trigger.
 *  2. Assign this prefab to PlayerShoot.projectilePrefab.
 *  3. Set terrainLayer to match your tilemap layer.
 *
 * USAGE:
 *  - PlayerShoot calls SetDirection() after instantiation.
 *  - Automatically destroys after lifetime or on hitting terrain/enemies.
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask terrainLayer;

    private Vector2 direction;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        // Flip sprite to match direction
        if (direction.x < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Hit enemy
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(this.gameObject);
            return;
        }

        // Hit terrain
        if (((1 << other.gameObject.layer) & terrainLayer) != 0)
        {
            Destroy(this.gameObject);
        }
    }
}
