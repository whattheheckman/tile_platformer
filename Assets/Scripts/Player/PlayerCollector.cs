/*
 * PlayerCollector.cs
 * ------------------
 * Handles the player picking up collectables (coins, etc.).
 *
 * SETUP:
 *  1. Attach to the same GameObject as PlayerController.
 *  2. Ensure your collectable GameObjects have a Collider2D set as Trigger and the Collectable script.
 *
 * USAGE:
 *  - Automatically collects any object with the Collectable script on overlap.
 */

using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            other.GetComponent<Collectable>().Collect();

        }
    }
}
