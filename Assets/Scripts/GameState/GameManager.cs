/*
 * GameManager.cs
 * --------------
 * Singleton managing overall game state: coins, respawn, win condition, and level timer.
 *
 * SETUP:
 *  1. Place a single GameManager GameObject in the scene.
 *  2. Assign playerSpawnPoint (a Transform marking where the player respawns).
 *  3. Assign the player GameObject reference.
 *  4. The PauseMenu and WinScreen scripts call into this.
 *
 * USAGE:
 *  - GameManager.Instance.RespawnPlayer() — reload current level.
 *  - GameManager.Instance.AddCoin() — called by Coin on collection.
 *  - GameManager.Instance.TriggerWin() — called by WinZone.
 *  - GameManager.Instance.GetElapsedTime() — query level timer.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject player;

    private int coinCount;
    private float levelStartTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        levelStartTime = Time.time;
    }

    public void RespawnPlayer()
    {
        // Reload the current scene to restart the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TriggerWin()
    {
        float elapsed = GetElapsedTime();
        Debug.Log($"Level complete! Time: {elapsed:F2}s  Coins: {coinCount}");
        // Show win screen via PauseMenu or a separate WinScreen component
        WinScreen winScreen = FindFirstObjectByType<WinScreen>();
        if (winScreen != null)
            winScreen.Show(elapsed, coinCount);
        else
            Time.timeScale = 0f; // fallback: freeze game
    }

    public void AddCoin()
    {
        coinCount++;
        Debug.Log($"Coins: {coinCount}");
    }

    public float GetElapsedTime() => Time.time - levelStartTime;
    public int GetCoinCount() => coinCount;
}
