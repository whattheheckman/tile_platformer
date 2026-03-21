/*
 * PlayerShoot.cs
 * --------------
 * Handles player projectile firing.
 *
 * SETUP:
 *  1. Attach to the same GameObject as PlayerController.
 *  2. Assign projectilePrefab (a prefab with the Projectile script) in the Inspector.
 *  3. Assign firePoint (a child Transform that marks where projectiles spawn).
 *  4. Assign shootSound (AudioClip) in the Inspector. Requires AudioSource on this GameObject.
 *
 * USAGE:
 *  - Press Left Mouse Button or Z to fire.
 *  - Projectile spawns at firePoint facing the direction the player is facing.
 */

using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private AudioClip shootSound;

    private PlayerController playerController;
    private AudioSource audioSource;
    private float nextFireTime;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Fire()
    {
        if (projectilePrefab == null || firePoint == null) return;

        // Direction based on player facing (scale x)
        float direction = Mathf.Sign(transform.localScale.x);

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projScript = proj.GetComponent<Projectile>();
        if (projScript != null)
            projScript.SetDirection(new Vector2(direction, 0f));

        if (shootSound != null) audioSource.PlayOneShot(shootSound);

        if (playerController != null)
            playerController.TriggerShootAnimation();
    }
}
