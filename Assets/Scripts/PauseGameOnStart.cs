using UnityEngine;
using TMPro;

public class PauseGameOnStart : MonoBehaviour
{
    public KeyCode unpauseKey = KeyCode.F; // Hotkey to unpause the game
    public TextMeshProUGUI pauseText; // Reference to the TextMeshPro for displaying pause text

    void Start()
    {
        PauseGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(unpauseKey))
        {
            UnpauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        if (pauseText != null)
        {
            pauseText.gameObject.SetActive(true); // Show the pause text
        }
    }

    void UnpauseGame()
    {
        Time.timeScale = 1f; // Unpause the game
        if (pauseText != null)
        {
            pauseText.gameObject.SetActive(false); // Hide the pause text
        }
    }
}