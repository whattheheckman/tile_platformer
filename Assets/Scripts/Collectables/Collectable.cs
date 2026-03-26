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
 *  2. Assign collectSound (FMOD EventReference) in the Inspector.
 *  3. Optionally assign collectParticles (a ParticleSystem) in the Inspector.
 *  4. The PlayerCollector script on the Player calls Collect() on contact.
 *  5. In the "Collect" animation clip, add Animation Events:
 *     - Call PlayCollectParticles() at the frame particles should burst
 *     - Call DestroyCollectable() at the last frame to destroy the GameObject
 *
 * USAGE:
 *  - Collect() is called automatically by PlayerCollector when the player overlaps.
 *  - Override OnCollected() in a subclass to add score, etc.
 */

using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private EventReference collectSound;
    [SerializeField] private ParticleSystem collectParticles;

    private Animator animator;
    private bool collected;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Collect()
    {
        if (collected) return;
        collected = true;

        // Disable trigger so it can't be collected again
        GetComponent<Collider2D>().enabled = false;

        if (!collectSound.IsNull)
            RuntimeManager.PlayOneShot(collectSound, transform.position);

        animator.SetTrigger("Collect");

        OnCollected();
    }

    // Override in subclass (e.g. Coin) to add score, etc.
    protected virtual void OnCollected() { }

    // Called by Animation Event on the "Collect" clip
    public void PlayCollectParticles()
    {
        if (collectParticles != null)
            collectParticles.Play();
    }

    // Called by Animation Event at the end of the "Collect" clip
    public void DestroyCollectable()
    {
        Destroy(gameObject);
    }
}
