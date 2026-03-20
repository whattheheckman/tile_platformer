/*
 * Coin.cs
 * -------
 * Extends Collectable. Awards a point to GameManager when collected.
 *
 * SETUP:
 *  1. Attach to a coin GameObject alongside Collectable (or replace Collectable with this).
 *     This script inherits all Collectable setup requirements.
 *  2. GameManager.Instance must be present in the scene.
 *
 * USAGE:
 *  - Automatically awards 1 coin to GameManager.AddCoin() on collection.
 */

using UnityEngine;

public class Coin : Collectable
{
    protected override void OnCollected()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.AddCoin();
    }
}
