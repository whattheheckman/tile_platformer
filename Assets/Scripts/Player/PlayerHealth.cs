/*
 * PlayerHealth.cs
 * ---------------
 * Manages player health, hurt flashing, sound, and death/respawn logic.
 *
 * SETUP:
 *  1. Attach to the same GameObject as PlayerController.
 *  2. Assign hurtSound (AudioClip) in the Inspector.
 *  3. Requires an AudioSource on this GameObject (auto-added).
 *  4. The GameManager.Instance.RespawnPlayer() is called on death — ensure GameManager is in the scene.
 *
 * USAGE:
 *  - Call TakeDamage(amount) from enemy/obstacle scripts to hurt the player.
 *  - Player flashes red briefly when hurt.
 *  - On death, GameManager handles restart.
 */

using System.Collections;
using UnityEngine;

//[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 1.2f;
    [SerializeField] private float flashInterval = 0.1f;
    [SerializeField] private AudioClip hurtSound;

    private int currentHealth;
    private bool isInvincible;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;


    private void OnValidate()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null )
        {
            Debug.LogError("Required SpriteRenderer component is missing from all children of " + gameObject.name + "!", gameObject);
        }
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;

        if (hurtSound != null)
            audioSource.PlayOneShot(hurtSound);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFlash());
        }
    }

    private IEnumerator InvincibilityFlash()
    {
        isInvincible = true;
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            // Flash between red and normal
            spriteRenderer.color = (spriteRenderer.color == Color.red) ? Color.white : Color.red;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.color = Color.white;
        isInvincible = false;
    }

    private void Die()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RespawnPlayer();
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
