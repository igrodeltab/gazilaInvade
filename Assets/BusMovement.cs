using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BusMovement : MonoBehaviour
{
    [SerializeField]
    private float _accelerationRate = 1.0f;
    [SerializeField]
    private float _decelerationRate = 2.0f;
    [SerializeField]
    private float _maxSpeed = 5.0f;
    [SerializeField]
    private float _turnSpeed = 200.0f;

    private Rigidbody2D _rigidbody2D;
    private InputSystem _inputSystem;
    private float _targetSpeed = 0;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputSystem = GetComponent<InputSystem>();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
    }

    private void HandleInput()
    {
        if (_inputSystem.VerticalInput > 0)
        {
            _targetSpeed = _maxSpeed;
        }
        else if (_inputSystem.VerticalInput < 0)
        {
            _targetSpeed = -_maxSpeed; // Изменено для движения назад
        }
    }

    private void HandleMovement()
    {
        float currentSpeed = _rigidbody2D.velocity.magnitude;
        Vector2 direction = transform.up;

        if (Mathf.Abs(currentSpeed) < Mathf.Abs(_targetSpeed))
        {
            Vector2 acceleration = direction * _accelerationRate * Time.deltaTime * Mathf.Sign(_targetSpeed);
            _rigidbody2D.velocity += acceleration;
        }
        else if (Mathf.Abs(currentSpeed) > Mathf.Abs(_targetSpeed))
        {
            Vector2 deceleration = -direction * _decelerationRate * Time.deltaTime * Mathf.Sign(_targetSpeed);
            _rigidbody2D.velocity += deceleration;
        }

        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxSpeed);
    }

    private void HandleRotation()
    {
        float turnAmount = 0;

        if (_inputSystem.HorizontalInput > 0)
        {
            turnAmount = 1;  // Для поворота направо
        }
        else if (_inputSystem.HorizontalInput < 0)
        {
            turnAmount = -1;  // Для поворота налево
        }

        float currentSpeed = _rigidbody2D.velocity.magnitude;
        float speedPercentage = currentSpeed / _maxSpeed;
        float currentTurnSpeed = _turnSpeed * speedPercentage;

        transform.Rotate(0, 0, turnAmount * currentTurnSpeed * Time.deltaTime);

        // Обновляем направление движения согласно новому углу поворота
        if (currentSpeed > 0)
        {
            _rigidbody2D.velocity = transform.up * currentSpeed * Mathf.Sign(_targetSpeed);
        }
    }
}