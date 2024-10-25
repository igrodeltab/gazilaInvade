using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BusMovement : MonoBehaviour
{
    [SerializeField] private float _accelerationRate = 1.0f; // Rate of acceleration
    [SerializeField] private float _decelerationRate = 2.0f; // Rate of deceleration
    [SerializeField] private float _maxSpeed = 5.0f;         // Maximum speed of the bus
    [SerializeField] private float _turnSpeed = 200.0f;      // Speed of turning

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
            // Apply braking force in the opposite direction of current movement
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
            // Accelerate or decelerate to reach the target speed
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
            turnAmount = 1;  // Turn right
        }
        else if (_inputSystem.HorizontalInput < 0)
        {
            turnAmount = -1;  // Turn left
        }

        float currentSpeed = _rigidbody2D.velocity.magnitude;
        float speedPercentage = currentSpeed / _maxSpeed;
        float currentTurnSpeed = _turnSpeed * speedPercentage;

        transform.Rotate(0, 0, turnAmount * currentTurnSpeed * Time.deltaTime);

        // Update movement direction based on the new rotation angle
        if (currentSpeed > 0)
        {
            _rigidbody2D.velocity = transform.up * currentSpeed * Mathf.Sign(_targetSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate relative velocity of the collision
        Vector2 relativeVelocity = collision.relativeVelocity;

        // Reset the current speed of the bus
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0;
        _isBraking = false;
        _targetSpeed = 0;

        // Apply a force to push the bus back
        _rigidbody2D.AddForce(-relativeVelocity * _rigidbody2D.mass, ForceMode2D.Impulse);
    }
}