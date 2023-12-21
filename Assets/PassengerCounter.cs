using UnityEngine;
using TMPro;

public class PassengerCounter : MonoBehaviour
{
    private TextMeshProUGUI _passengerCounterText;

    public static PassengerCounter Instance { get; private set; }

    private void Awake()
    {
        _passengerCounterText = GetComponent<TextMeshProUGUI>();

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

    public void UpdatePassengerCount(int current, int max)
    {
        if (_passengerCounterText != null)
        {
            _passengerCounterText.text = $"{current}/{max}";
        }
    }
}