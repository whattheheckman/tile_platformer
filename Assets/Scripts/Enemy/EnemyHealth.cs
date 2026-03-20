/*
 * EnemyHealth.cs
 * --------------
 * Manages enemy health and death. Plays a hurt sound on taking damage.
 *
 * SETUP:
 *  1. Attach to the same GameObject as EnemyController.
 *  2. Assign hurtSound (AudioClip) in the Inspector.
 *  3. Requires AudioSource (auto-added).
 *
 * USAGE:
 *  - Call TakeDamage(amount) from projectile or other scripts.
 *  - Enemy is destroyed when health reaches 0.
 */

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private AudioClip hurtSound;

    private int currentHealth;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (hurtSound != null)
            audioSource.PlayOneShot(hurtSound);

        if (currentHealth <= 0)
            Destroy(gameObject);
    }
}
