/*
 * BoundaryAudioZone.cs
 * --------------------
 * Changes the background music when the player enters a Cinemachine confiner zone area.
 * Attach one per "room" or boundary zone alongside your Cinemachine confiner collider.
 *
 * SETUP:
 *  1. Attach to the same GameObject as your zone's Polygon/Box Collider2D (set as Trigger).
 *  2. Assign zoneMusic (AudioClip) — the music that plays in this zone.
 *  3. Assign the musicSource (an AudioSource in the scene designated for background music).
 *     Tip: put AudioSource on a persistent "MusicPlayer" GameObject and reference it here,
 *     or use a singleton MusicManager.
 *  4. Set crossfadeTime to 0 for instant switch, or > 0 for a simple volume fade.
 *
 * USAGE:
 *  - When the player enters this trigger, the music switches to zoneMusic.
 *  - Each separate area gets its own BoundaryAudioZone with a different clip.
 */

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BoundaryAudioZone : MonoBehaviour
{
    [SerializeField] private AudioClip zoneMusic;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float crossfadeTime = 0.5f;

    private Coroutine crossfadeRoutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (musicSource == null || zoneMusic == null) return;
        if (musicSource.clip == zoneMusic) return; // already playing

        if (crossfadeRoutine != null)
            StopCoroutine(crossfadeRoutine);

        if (crossfadeTime > 0f)
            crossfadeRoutine = StartCoroutine(Crossfade());
        else
        {
            musicSource.clip = zoneMusic;
            musicSource.Play();
        }
    }

    private IEnumerator Crossfade()
    {
        float startVolume = musicSource.volume;

        // Fade out
        float t = 0f;
        while (t < crossfadeTime / 2f)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / (crossfadeTime / 2f));
            yield return null;
        }

        // Swap clip
        musicSource.clip = zoneMusic;
        musicSource.Play();

        // Fade in
        t = 0f;
        while (t < crossfadeTime / 2f)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(0f, startVolume, t / (crossfadeTime / 2f));
            yield return null;
        }

        musicSource.volume = startVolume;
    }
}
