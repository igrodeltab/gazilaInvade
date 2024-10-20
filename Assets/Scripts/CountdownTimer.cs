using UnityEngine;
using TMPro;
using System;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float _countdownTime = 60f; // Countdown time in seconds
    [SerializeField] private TextMeshProUGUI _timerText; // Reference to the TextMeshProUGUI component

    private float _currentTime;

    void Start()
    {
        _currentTime = _countdownTime;
    }

    void Update()
    {
        if (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime; // Decrease time
            UpdateTimerText(_currentTime); // Update timer text in minutes:seconds format
        }
        else
        {
            _timerText.text = "00:00"; // When time is up
            TimerFinished(); // Call method when timer finishes
        }
    }

    private void UpdateTimerText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        _timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds); // Format as MM:SS
    }

    private void TimerFinished()
    {
        // Actions when the timer finishes
        Debug.Log("Time is up!");
    }
}