using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Для перезапуска уровня

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float _countdownTime = 60f; // Countdown time in seconds
    [SerializeField] private TextMeshProUGUI _timerText; // Reference to the TextMeshProUGUI component
    [SerializeField] private TextMeshProUGUI _defeatText; // Reference to the TextMeshProUGUI for defeat screen
    [SerializeField] private KeyCode _restartKey = KeyCode.R; // Key to restart the level

    private float _currentTime;
    private bool _isDefeated = false; // Flag for defeat state

    void Start()
    {
        _currentTime = _countdownTime;
        if (_defeatText != null) _defeatText.gameObject.SetActive(false); // Hide defeat text at start
    }

    void Update()
    {
        if (_isDefeated && Input.GetKeyDown(_restartKey))
        {
            RestartLevel(); // Restart the level when the restart key is pressed
        }
        else if (!_isDefeated && _currentTime > 0)
        {
            _currentTime -= Time.deltaTime; // Decrease time
            UpdateTimerText(_currentTime); // Update timer text in minutes:seconds format
        }
        else if (_currentTime <= 0 && !_isDefeated)
        {
            TimerFinished(); // Call method when timer finishes
        }
    }

    private void UpdateTimerText(float time)
    {
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(time);
        _timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds); // Format as MM:SS
    }

    private void TimerFinished()
    {
        _isDefeated = true; // Set defeat state
        Time.timeScale = 0f; // Stop the game
        if (_defeatText != null)
        {
            _defeatText.gameObject.SetActive(true); // Show defeat text
        }
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f; // Resume the game before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}