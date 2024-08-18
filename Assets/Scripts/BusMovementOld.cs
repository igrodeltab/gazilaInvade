using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BusMovementOld : MonoBehaviour
{
    [SerializeField]
    private float _accelerationRate = 1.0f;
    [SerializeField]
    private float _decelerationRate = 2.0f;
    [SerializeField]
    private float _maxSpeed = 5.0f;
    [SerializeField]
    private float _turnSpeed = 200.0f;  // Скорость поворота

    private Rigidbody2D _rigidbody2D;
    private float _targetSpeed = 0;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _targetSpeed = _maxSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _targetSpeed = 0;
        }
    }

    private void HandleMovement()
    {
        float currentSpeed = _rigidbody2D.velocity.magnitude;

        Vector2 direction = transform.up;  // Направление движения вперед

        if (currentSpeed < _targetSpeed)
        {
            Vector2 acceleration = direction * _accelerationRate * Time.deltaTime;
            _rigidbody2D.velocity += acceleration;
        }
        else if (currentSpeed > _targetSpeed)
        {
            Vector2 deceleration = -direction * _decelerationRate * Time.deltaTime;
            _rigidbody2D.velocity += deceleration;
        }

        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxSpeed);
    }

    private void HandleRotation()
    {
        float turnAmount = 0;

        if (Input.GetKey(KeyCode.A))
        {
            turnAmount = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnAmount = -1;
        }

        // Вычисляем скорость поворота на основе текущей скорости
        float currentSpeed = _rigidbody2D.velocity.magnitude;
        float speedPercentage = currentSpeed / _maxSpeed;
        float currentTurnSpeed = _turnSpeed * speedPercentage;

        // Применяем поворот
        transform.Rotate(0, 0, turnAmount * currentTurnSpeed * Time.deltaTime);

        // Обновляем направление движения согласно новому углу поворота
        _rigidbody2D.velocity = transform.up * currentSpeed;
    }
}