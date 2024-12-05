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
    [SerializeField] private LightState _defaultState = LightState.Green; // Значение по умолчанию

    private enum LightState { Green, Yellow, Red }
    private LightState _currentState;

    private float _stateTimer;
    private int _direction = 1; // Направление смены (1 - вперед, -1 - назад)

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

        // Инициализация начального состояния
        InitializeState();
    }

    private void Update()
    {
        if (_lights.Length == 0) return;

        // Обновляем таймер
        _stateTimer -= Time.deltaTime;

        if (_stateTimer <= 0)
        {
            // Смена состояния
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
                _direction = -1; // Меняем направление после красного света
                break;
        }

        // Если вернулись к зеленому свету, меняем направление на вперед
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