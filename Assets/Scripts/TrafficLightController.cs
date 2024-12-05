using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    [System.Serializable]
    private class TrafficLightState
    {
        [SerializeField] private Sprite _sprite;  // Sprite for the state
        [SerializeField] private float _duration; // Duration of the state

        public Sprite Sprite => _sprite;
        public float Duration => _duration;
    }

    [SerializeField] private TrafficLightState _greenState;
    [SerializeField] private TrafficLightState _yellowState;
    [SerializeField] private TrafficLightState _redState;
    [SerializeField] private LightState _defaultState = LightState.Green; // Default initial state

    private enum LightState { Green, Yellow, Red }
    private LightState _currentState;

    private float _stateTimer;
    private int _direction = 1; // Direction of the cycle (1 - forward, -1 - backward)

    private SpriteRenderer[] _lights;

    private void Start()
    {
        // Get all child objects with SpriteRenderer components
        _lights = GetComponentsInChildren<SpriteRenderer>();

        if (_lights.Length == 0)
        {
            Debug.LogError("No child objects with SpriteRenderer found!");
            return;
        }

        // Initialize the starting state
        InitializeState();
    }

    private void Update()
    {
        if (_lights.Length == 0) return;

        // Update the timer
        _stateTimer -= Time.deltaTime;

        if (_stateTimer <= 0)
        {
            // Change the traffic light state
            ChangeState();
        }
    }

    private void InitializeState()
    {
        _currentState = _defaultState;
        SetState(GetTrafficLightState(_currentState));
    }

    private void ChangeState()
    {
        switch (_currentState)
        {
            case LightState.Green:
                _currentState = LightState.Yellow;
                SetState(_yellowState);
                break;

            case LightState.Yellow:
                if (_direction == 1)
                {
                    _currentState = LightState.Red;
                    SetState(_redState);
                }
                else
                {
                    _currentState = LightState.Green;
                    SetState(_greenState);
                }
                break;

            case LightState.Red:
                _currentState = LightState.Yellow;
                SetState(_yellowState);
                _direction = -1; // Change direction after the red light
                break;
        }

        // Reset direction to forward when returning to green light
        if (_currentState == LightState.Green)
        {
            _direction = 1;
        }
    }

    private void SetState(TrafficLightState state)
    {
        foreach (var light in _lights)
        {
            light.sprite = state.Sprite;
        }

        _stateTimer = state.Duration;
    }

    private TrafficLightState GetTrafficLightState(LightState state)
    {
        switch (state)
        {
            case LightState.Green: return _greenState;
            case LightState.Yellow: return _yellowState;
            case LightState.Red: return _redState;
            default: return null;
        }
    }
}