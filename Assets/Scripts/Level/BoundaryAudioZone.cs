/*
 * BoundaryAudioZone.cs
 * --------------------
 * Changes the background music when the player enters a Cinemachine confiner zone area.
 * Attach one per "room" or boundary zone alongside your Cinemachine confiner collider.
 *
 * SETUP:
 *  1. Attach to the same GameObject as your zone's Polygon/Box Collider2D (set as Trigger).
 *  2. Assign zoneMusic (FMOD EventReference) — the music event that plays in this zone.
 *  3. Set crossfadeTime to 0 for instant switch, or > 0 for a volume crossfade.
 *
 * USAGE:
 *  - When the player enters this trigger, the music switches to zoneMusic.
 *  - Each separate area gets its own BoundaryAudioZone with a different event.
 *  - All zones share a single static FMOD EventInstance so only one track plays at a time.
 */

using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BoundaryAudioZone : MonoBehaviour
{
    [SerializeField] private EventReference zoneMusic;
    [SerializeField] private float crossfadeTime = 0.5f;

    // Shared across all zones so only one music instance is active at a time
    private static EventInstance currentMusicInstance;
    private static FMOD.GUID currentMusicGuid;

    private Coroutine crossfadeRoutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (zoneMusic.IsNull) return;
        if (currentMusicGuid.Equals(zoneMusic.Guid)) return; // already playing this event

        if (crossfadeRoutine != null)
            StopCoroutine(crossfadeRoutine);

        if (crossfadeTime > 0f)
            crossfadeRoutine = StartCoroutine(Crossfade());
        else
            SwitchMusic();
    }

    private void SwitchMusic()
    {
        if (currentMusicInstance.isValid())
        {
            currentMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentMusicInstance.release();
        }

        currentMusicGuid = zoneMusic.Guid;
        currentMusicInstance = RuntimeManager.CreateInstance(zoneMusic);
        currentMusicInstance.start();
    }

    private IEnumerator Crossfade()
    {
        EventInstance oldInstance = currentMusicInstance;

        // Start new track at zero volume
        currentMusicGuid = zoneMusic.Guid;
        currentMusicInstance = RuntimeManager.CreateInstance(zoneMusic);
        currentMusicInstance.setVolume(0f);
        currentMusicInstance.start();

        // Simultaneously fade old out and new in
        float t = 0f;
        while (t < crossfadeTime)
        {
            t += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(t / crossfadeTime);
            if (oldInstance.isValid())
                oldInstance.setVolume(1f - progress);
            currentMusicInstance.setVolume(progress);
            yield return null;
        }

        currentMusicInstance.setVolume(1f);

        if (oldInstance.isValid())
        {
            oldInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            oldInstance.release();
        }
    }
}
