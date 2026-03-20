/*
 * WinScreen.cs
 * ------------
 * Displays a win screen overlay when the player reaches the win zone.
 * Shows elapsed time and coin count.
 *
 * SETUP:
 *  1. Create a Canvas with a win panel containing:
 *     - A Text (TMP) for "Level Complete!"
 *     - A Text (TMP) for time display
 *     - A Text (TMP) for coin display
 *     - A "Next Level" or "Restart" button
 *  2. Assign winPanel, timeText, coinText in the Inspector.
 *  3. GameManager.TriggerWin() calls Show() automatically.
 *
 * USAGE:
 *  - Show(time, coins) is called by GameManager.
 *  - Wire the restart button to WinScreen.Restart().
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text coinText;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void Show(float time, int coins)
    {
        Time.timeScale = 0f;
        if (winPanel != null)
            winPanel.SetActive(true);
        if (timeText != null)
            timeText.text = $"Time: {time:F2}s";
        if (coinText != null)
            coinText.text = $"Coins: {coins}";
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene(0); // loop back to first level
    }
}
