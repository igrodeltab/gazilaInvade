using UnityEngine;

public class BlinkingPoint : MonoBehaviour
{
    [SerializeField] private float _blinkInterval = 1f; // Интервал мигания в секундах
    private Renderer _objectRenderer;
    private float _timer;

    private void Awake()
    {
        _objectRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _blinkInterval)
        {
            // Переключаем видимость объекта
            _objectRenderer.enabled = !_objectRenderer.enabled;
            _timer = 0f;
        }
    }
}