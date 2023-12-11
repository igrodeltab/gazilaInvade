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
    private bool _isBraking = false;

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
            if (IsMovingForward() || IsStationary())
            {
                _targetSpeed = _maxSpeed;
                _isBraking = false;
            }
            else
            {
                _isBraking = true;
            }
        }
        else if (_inputSystem.VerticalInput < 0)
        {
            if (IsMovingBackward() || IsStationary())
            {
                _targetSpeed = -_maxSpeed;
                _isBraking = false;
            }
            else
            {
                _isBraking = true;
            }
        }
    }

    private bool IsMovingForward()
    {
        return Vector2.Dot(_rigidbody2D.velocity, transform.up) > 0;
    }

    private bool IsMovingBackward()
    {
        return Vector2.Dot(_rigidbody2D.velocity, transform.up) < 0;
    }

    private bool IsStationary()
    {
        return _rigidbody2D.velocity.magnitude < 0.01f;
    }

    private void HandleMovement()
    {
        float currentSpeed = _rigidbody2D.velocity.magnitude;
        Vector2 currentDirection = _rigidbody2D.velocity.normalized;
        Vector2 forwardDirection = transform.up;

        if (_isBraking)
        {
            // Торможение: применяем силу замедления в направлении, противоположном текущему движению
            Vector2 deceleration = -currentDirection * _decelerationRate * Time.deltaTime;
            _rigidbody2D.velocity += deceleration;

            if (currentSpeed < 0.01f)
            {
                _rigidbody2D.velocity = Vector2.zero;
                _isBraking = false;
                _targetSpeed = 0;
            }
        }
        else
        {
            // Ускорение или замедление до целевой скорости
            if (currentSpeed < Mathf.Abs(_targetSpeed))
            {
                Vector2 acceleration = forwardDirection * _accelerationRate * Time.deltaTime * Mathf.Sign(_targetSpeed);
                _rigidbody2D.velocity += acceleration;
            }
            else if (currentSpeed > Mathf.Abs(_targetSpeed))
            {
                Vector2 deceleration = -forwardDirection * _decelerationRate * Time.deltaTime * Mathf.Sign(_targetSpeed);
                _rigidbody2D.velocity += deceleration;
            }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Вычисление относительной скорости столкновения
        Vector2 relativeVelocity = collision.relativeVelocity;

        // Сброс текущей скорости автобуса
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0;
        _isBraking = false;
        _targetSpeed = 0;

        // Применение силы для отталкивания автобуса
        _rigidbody2D.AddForce(-relativeVelocity * _rigidbody2D.mass, ForceMode2D.Impulse);
    }
}