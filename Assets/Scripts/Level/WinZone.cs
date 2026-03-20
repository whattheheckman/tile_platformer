/*
 * WinZone.cs
 * ----------
 * Triggers the win condition when the player enters this zone.
 *
 * SETUP:
 *  1. Attach to a GameObject with a Collider2D set as Trigger (e.g., the doorway/exit sprite).
 *  2. No extra Inspector setup needed — calls GameManager.Instance.TriggerWin().
 *
 * USAGE:
 *  - When the player walks into the trigger, the game registers a win.
 */

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.TriggerWin();
        }
    }
}
