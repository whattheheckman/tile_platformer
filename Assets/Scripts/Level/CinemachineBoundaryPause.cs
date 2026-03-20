/*
 * CinemachineBoundaryPause.cs
 * ----------------------------
 * Pauses the game briefly when the Cinemachine camera transitions between confiner zones.
 * Attach this to the CinemachineCamera (Virtual Camera) GameObject.
 *
 * SETUP:
 *  1. Attach to your CinemachineCamera (or a manager GameObject).
 *  2. Set pauseDuration — how long to freeze on transition (e.g. 0.5s).
 *  3. Call TriggerBoundaryPause() from a custom CinemachineConfiner zone enter event,
 *     OR call it from BoundaryAudioZone.OnTriggerEnter2D by adding a FindObjectOfType call,
 *     OR wire a UnityEvent from your boundary trigger.
 *
 * USAGE:
 *  - TriggerBoundaryPause() freezes gameplay for pauseDuration seconds (unscaled).
 *  - Camera blend still plays; gameplay objects freeze.
 */

using System.Collections;
using UnityEngine;

public class CinemachineBoundaryPause : MonoBehaviour
{
    [SerializeField] private float pauseDuration = 0.6f;

    private bool isPausing;

    public void TriggerBoundaryPause()
    {
        if (!isPausing)
            StartCoroutine(DoPause());
    }

    private IEnumerator DoPause()
    {
        isPausing = true;
        float previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(pauseDuration);
        Time.timeScale = previousTimeScale;
        isPausing = false;
    }
}
