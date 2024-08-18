using UnityEngine;
using TMPro;

public class TotalEarningsCounter : MonoBehaviour
{
    private TextMeshProUGUI _totalEarningsCounterText;

    public static TotalEarningsCounter Instance { get; private set; }

    private void Awake()
    {
        _totalEarningsCounterText = GetComponent<TextMeshProUGUI>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTotalEarningsCount(int currentMoney)
    {
        if (_totalEarningsCounterText != null)
        {
            _totalEarningsCounterText.text = $"{currentMoney}";
        }
    }
}