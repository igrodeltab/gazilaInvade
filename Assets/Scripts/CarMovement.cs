using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarMovement : MonoBehaviour
{
    [SerializeField] private float _accelerationRate = 1.0f; // Rate of acceleration
    [SerializeField] private float _decelerationRate = 2.0f; // Rate of deceleration
    [SerializeField] private float _maxSpeed = 5.0f;         // Maximum speed of the bus
    [SerializeField] private float _turnSpeed = 200.0f;      // Speed of turning

    private Rigidbody2D _rigidbody2D;
    private float _targetSpeed = 0;
    private bool _isBraking = false;
    private float _turnDirection = 0; // Variable to store turn direction (0 for straight, 1 for right, -1 for left)

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    public void MoveForward()
    {
        // Set target speed to max and turn off braking
        _targetSpeed = _maxSpeed;
        _isBraking = false;
    }

    public void Stop()
    {
        // Set target speed to zero and enable braking
        _targetSpeed = 0;
        _isBraking = true;
    }

    public void TurnRight()
    {
        _turnDirection = 1; // Set turn direction to right
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
            }
        }
        else
        {
            // Accelerate to reach the target speed
            if (currentSpeed < Mathf.Abs(_targetSpeed))
            {
                Vector2 acceleration = forwardDirection * _accelerationRate * Time.deltaTime;
                _rigidbody2D.velocity += acceleration;
            }
        }

        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxSpeed);
    }

    private void HandleRotation()
    {
        float currentSpeed = _rigidbody2D.velocity.magnitude;
        float speedPercentage = currentSpeed / _maxSpeed;
        float currentTurnSpeed = _turnSpeed * speedPercentage;

        // Apply rotation based on turn direction
        transform.Rotate(0, 0, -_turnDirection * currentTurnSpeed * Time.deltaTime);

        // Reset turn direction after applying rotation
        _turnDirection = 0;

        // Update movement direction based on the new rotation angle
        if (currentSpeed > 0)
        {
            _rigidbody2D.velocity = transform.up * currentSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset the current speed of the car on collision
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0;
        _isBraking = false;
        _targetSpeed = 0;
    }
}