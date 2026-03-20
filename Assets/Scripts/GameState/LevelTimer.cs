/*
 * LevelTimer.cs
 * -------------
 * Displays the elapsed level time on a UI Text element in real time.
 *
 * SETUP:
 *  1. Attach to a persistent GameObject (GameManager or HUD manager).
 *  2. Assign timerText (TMP_Text UI element) in the Inspector.
 *  3. GameManager tracks the actual start time; this just reads and displays it.
 *
 * USAGE:
 *  - Automatically updates every frame while Time.timeScale > 0.
 */

using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private void Update()
    {
        if (GameManager.Instance == null || timerText == null) return;

        float elapsed = GameManager.Instance.GetElapsedTime();
        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
