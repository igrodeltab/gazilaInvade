using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float _rotationStep = 10f; // Шаг поворота в градусах
    [SerializeField] private float _rotationDelay = 0.5f; // Задержка перед началом непрерывного поворота при удержании
    [SerializeField] private float _continuousRotationInterval = 0.1f; // Интервал между поворотами при удержании
    [SerializeField] private float _rotationSpeed = 1f; // Множитель скорости поворота

    private float _rotationTimer;
    private bool _isHoldingKey = false; // Флаг для удержания клавиши

    private void Update()
    {
        HandleRotation(KeyCode.A, _rotationStep);
        HandleRotation(KeyCode.D, -_rotationStep);
    }

    private void HandleRotation(KeyCode key, float step)
    {
        if (Input.GetKeyDown(key))
        {
            RotateObjectByStep(step);
            _rotationTimer = _rotationDelay; // Начинаем отсчёт задержки для удержания
            _isHoldingKey = true; // Устанавливаем флаг удержания
        }

        if (Input.GetKey(key))
        {
            _rotationTimer -= Time.deltaTime;

            if (_rotationTimer <= 0 && _isHoldingKey)
            {
                RotateObjectByStep(step * _rotationSpeed); // Непрерывный поворот
                _rotationTimer = _continuousRotationInterval; // Интервал между поворотами
            }
        }

        if (Input.GetKeyUp(key))
        {
            _isHoldingKey = false; // Сбрасываем флаг удержания
        }
    }

    private void RotateObjectByStep(float step)
    {
        transform.Rotate(0, 0, step);
    }
}