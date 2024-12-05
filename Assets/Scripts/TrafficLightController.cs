using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    [System.Serializable]
    private class TrafficLightState
    {
        [SerializeField] private Sprite _sprite;  // Спрайт для состояния
        [SerializeField] private float _duration; // Длительность состояния

        public Sprite Sprite => _sprite;
        public float Duration => _duration;
    }

    [SerializeField] private TrafficLightState _greenState;
    [SerializeField] private TrafficLightState _yellowState;
    [SerializeField] private TrafficLightState _redState;

    private enum LightState { Green, Yellow, Red }
    private LightState _currentState = LightState.Green;

    private float _stateTimer;
    private int _direction = 1; // Направление (1 - вперед, -1 - назад)

    private SpriteRenderer[] _lights;

    private void Start()
    {
        // Получаем все дочерние объекты со спрайтами
        _lights = GetComponentsInChildren<SpriteRenderer>();

        if (_lights.Length == 0)
        {
            Debug.LogError("No child objects with SpriteRenderer found!");
            return;
        }

        // Устанавливаем начальное состояние
        UpdateLights(_greenState);
        _stateTimer = _greenState.Duration;
    }

    private void Update()
    {
        if (_lights.Length == 0) return;

        // Обновляем таймер
        _stateTimer -= Time.deltaTime;

        if (_stateTimer <= 0)
        {
            // Меняем состояние
            ChangeState();
        }
    }

    private void ChangeState()
    {
        switch (_currentState)
        {
            case LightState.Green:
                _currentState = LightState.Yellow;
                UpdateLights(_yellowState);
                _stateTimer = _yellowState.Duration;
                break;
            case LightState.Yellow:
                _currentState = _direction == 1 ? LightState.Red : LightState.Green;
                UpdateLights(_currentState == LightState.Red ? _redState : _greenState);
                _stateTimer = _currentState == LightState.Red ? _redState.Duration : _greenState.Duration;
                break;
            case LightState.Red:
                _currentState = LightState.Yellow;
                UpdateLights(_yellowState);
                _stateTimer = _yellowState.Duration;
                _direction = -1; // Меняем направление
                break;
        }
    }

    private void UpdateLights(TrafficLightState state)
    {
        foreach (var light in _lights)
        {
            light.sprite = state.Sprite;
        }
    }
}