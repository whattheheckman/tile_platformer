/*
 * HazardZone.cs
 * -------------
 * Damages the player when they enter a hazard trigger (e.g. spike pit, lava, fall-out-of-bounds).
 *
 * SETUP:
 *  1. Attach to any GameObject with a Collider2D set as Trigger.
 *  2. Set damageAmount in the Inspector.
 *  3. Place over spike tiles, below the level, etc.
 *
 * USAGE:
 *  - Calls PlayerHealth.TakeDamage() on player entry.
 */

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HazardZone : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(damageAmount);
    }
}
