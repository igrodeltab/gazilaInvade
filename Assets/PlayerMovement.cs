using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _accelerationRate = 1.0f; // Скорость набора скорости
    [SerializeField]
    private float _decelerationRate = 2.0f; // Скорость торможения
    [SerializeField]
    private float _maxSpeed = 5.0f;         // Максимальная скорость

    private Rigidbody2D _rigidbody2D;
    private float _targetSpeed = 0;         // Желаемая скорость

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
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
        if (_rigidbody2D.velocity.y < _targetSpeed)
        {
            _rigidbody2D.velocity += new Vector2(0, _accelerationRate * Time.deltaTime);
        }
        else if (_rigidbody2D.velocity.y > _targetSpeed)
        {
            _rigidbody2D.velocity -= new Vector2(0, _decelerationRate * Time.deltaTime);
        }

        // Ограничиваем максимальную скорость
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxSpeed);
    }
}
