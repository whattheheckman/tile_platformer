/*
 * Collectable.cs
 * --------------
 * Base class for collectables (coins, power-ups, etc.).
 * Plays an idle animation, triggers a collect animation + sound, then destroys itself.
 *
 * SETUP:
 *  1. Attach to a collectable GameObject with:
 *     - Collider2D set as Trigger
 *     - Animator with states: "Idle" and "Collect" (set "Collect" trigger in transitions)
 *     - AudioSource (auto-added)
 *  2. Assign collectSound (AudioClip) in the Inspector.
 *  3. The PlayerCollector script on the Player calls Collect() on contact.
 *
 * USAGE:
 *  - Collect() is called automatically by PlayerCollector when the player overlaps.
 *  - Override OnCollected() in a subclass to add score, etc.
 */

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private float destroyDelay = 0.5f; // time after collect anim before destroy

    private Animator animator;
    private AudioSource audioSource;
    private bool collected;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Collect()
    {
        if (collected) return;
        collected = true;

        // Disable trigger so it can't be collected again
        GetComponent<Collider2D>().enabled = false;

        if (collectSound != null)
            audioSource.PlayOneShot(collectSound);

        animator.SetTrigger("Collect");

        OnCollected();

        StartCoroutine(DestroyAfterDelay());
    }

    // Override in subclass (e.g. Coin) to add score, etc.
    protected virtual void OnCollected() { }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
