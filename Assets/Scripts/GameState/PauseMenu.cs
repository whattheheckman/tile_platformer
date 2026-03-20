/*
 * PauseMenu.cs
 * ------------
 * Pauses the game and shows an overlay UI when the player presses Escape.
 *
 * SETUP:
 *  1. Create a Canvas with a pause panel (e.g. a dark overlay with "Resume" and "Restart" buttons).
 *  2. Attach this script to any persistent GameObject (e.g. the GameManager or a UI Manager).
 *  3. Assign pausePanel (the root panel GameObject) in the Inspector.
 *  4. Wire the Resume button to PauseMenu.Resume() and Restart button to PauseMenu.Restart().
 *
 * USAGE:
 *  - Press Escape to toggle pause.
 *  - Pause sets Time.timeScale = 0; resume sets it back to 1.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private bool isPaused;

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        if (pausePanel != null)
            pausePanel.SetActive(isPaused);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
