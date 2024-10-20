using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // For restarting the level

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float _countdownTime = 60f; // Countdown time in seconds
    [SerializeField] private TextMeshProUGUI _timerText; // Reference to the TextMeshProUGUI component
    [SerializeField] private TextMeshProUGUI _defeatText; // Reference to the TextMeshProUGUI for defeat screen
    [SerializeField] private TextMeshProUGUI _victoryText; // Reference to the TextMeshProUGUI for victory screen
    [SerializeField] private KeyCode _restartKey = KeyCode.R; // Key to restart the level

    private float _currentTime;
    private bool _isGameOver = false; // Flag to check if the game is over (victory or defeat)

    void Start()
    {
        _currentTime = _countdownTime;
        if (_defeatText != null) _defeatText.gameObject.SetActive(false); // Hide defeat text at start
        if (_victoryText != null) _victoryText.gameObject.SetActive(false); // Hide victory text at start
    }

    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(_restartKey))
        {
            RestartLevel(); // Restart the level when the restart key is pressed
        }
        else if (!_isGameOver && _currentTime > 0)
        {
            _currentTime -= Time.deltaTime; // Decrease time
            UpdateTimerText(_currentTime); // Update timer text in minutes:seconds format
        }
        else if (_currentTime <= 0 && !_isGameOver)
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
        _isGameOver = true; // Set game over state
        Time.timeScale = 0f; // Stop the game
        if (_defeatText != null)
        {
            _defeatText.gameObject.SetActive(true); // Show defeat text
        }
    }

    public void Victory() // Сделали метод публичным
    {
        _isGameOver = true; // Set game over state
        Time.timeScale = 0f; // Stop the game
        if (_victoryText != null)
        {
            _victoryText.gameObject.SetActive(true); // Show victory text
        }
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f; // Resume the game before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}