using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float _rotationStep = 10f; // Шаг поворота в градусах
    [SerializeField] private float _rotationDelay = 0.5f; // Задержка перед началом непрерывного поворота при удержании
    [SerializeField] private float _continuousRotationInterval = 0.1f; // Интервал между поворотами при удержании

    private float _rotationTimer;

    private void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        // Если нажата клавиша A
        if (Input.GetKeyDown(KeyCode.A))
        {
            RotateObjectByStep(_rotationStep); // Поворот на шаг
            _rotationTimer = _rotationDelay; // Устанавливаем таймер на задержку перед удержанием
        }

        // Если нажата клавиша D
        if (Input.GetKeyDown(KeyCode.D))
        {
            RotateObjectByStep(-_rotationStep); // Поворот на шаг
            _rotationTimer = _rotationDelay; // Устанавливаем таймер на задержку перед удержанием
        }

        // Если удерживается клавиша A
        if (Input.GetKey(KeyCode.A))
        {
            _rotationTimer -= Time.deltaTime;
            if (_rotationTimer <= 0)
            {
                RotateObjectByStep(_rotationStep); // Поворот на шаг
                _rotationTimer = _continuousRotationInterval; // Устанавливаем таймер на интервал
            }
        }

        // Если удерживается клавиша D
        if (Input.GetKey(KeyCode.D))
        {
            _rotationTimer -= Time.deltaTime;
            if (_rotationTimer <= 0)
            {
                RotateObjectByStep(-_rotationStep); // Поворот на шаг
                _rotationTimer = _continuousRotationInterval; // Устанавливаем таймер на интервал
            }
        }
    }

    private void RotateObjectByStep(float step)
    {
        // Поворачиваем объект
        transform.Rotate(0, 0, step);
    }
}