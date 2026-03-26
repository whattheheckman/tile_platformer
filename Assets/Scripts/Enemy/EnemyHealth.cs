/*
 * EnemyHealth.cs
 * --------------
 * Manages enemy health and death. Plays a hurt sound on taking damage.
 *
 * SETUP:
 *  1. Attach to the same GameObject as EnemyController.
 *  2. Assign hurtSound (FMOD EventReference) in the Inspector.
 *
 * USAGE:
 *  - Call TakeDamage(amount) from projectile or other scripts.
 *  - Enemy is destroyed when health reaches 0.
 */

using FMODUnity;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private EventReference hurtSound;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (!hurtSound.IsNull)
            RuntimeManager.PlayOneShot(hurtSound, transform.position);

        if (currentHealth <= 0)
            Destroy(gameObject);
    }
}
