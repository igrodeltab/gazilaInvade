using UnityEngine;

public class CameraFollowBus : MonoBehaviour
{
    [SerializeField] private Transform _busTransform; // Ссылка на трансформ автобуса
    [SerializeField] private Transform _frontPointTransform; // Точка перед автобусом
    [SerializeField] private Transform _backPointTransform; // Точка сзади автобуса
    [SerializeField] private float _centerToFrontSpeed = 1f; // Скорость перехода от центра к переду
    [SerializeField] private float _frontToCenterSpeed = 1f; // Скорость перехода от переда к центру
    [SerializeField] private float _centerToBackSpeed = 1f; // Скорость перехода от центра к заду
    [SerializeField] private float _backToCenterSpeed = 1f; // Скорость перехода от зада к центру
    [SerializeField] private float _accelerationFactor = 1f; // Фактор ускорения камеры
    [SerializeField] private float _maxAcceleration = 10f; // Максимальное ускорение автобуса

    private Rigidbody2D _busRigidbody;
    private Transform _currentTarget;
    private float _currentTransitionSpeed;

    private void Start()
    {
        _busRigidbody = _busTransform.GetComponent<Rigidbody2D>();
        _currentTarget = _busTransform;
    }

    private void FixedUpdate()
    {
        Transform newTarget = DetermineTargetTransform();
        if (_currentTarget != newTarget)
        {
            UpdateTransitionSpeed(_currentTarget, newTarget);
            _currentTarget = newTarget;
        }

        Vector3 targetPosition = new Vector3(_currentTarget.position.x, _currentTarget.position.y, transform.position.z);
        float cameraSpeed = _currentTransitionSpeed;

        // Учитываем ускорение автобуса
        float acceleration = Mathf.Clamp(_busRigidbody.velocity.magnitude, 0f, _maxAcceleration) * _accelerationFactor;
        cameraSpeed += acceleration;

        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.fixedDeltaTime);
    }

    private Transform DetermineTargetTransform()
    {
        if (_busRigidbody.velocity.magnitude < 0.1f)
            return _busTransform;

        Vector2 busDirection = _busRigidbody.velocity.normalized;
        Vector2 busForward = _busTransform.up;
        bool movingForward = Vector2.Dot(busDirection, busForward) > 0;

        return movingForward ? _frontPointTransform : _backPointTransform;
    }

    private void UpdateTransitionSpeed(Transform current, Transform newTarget)
    {
        if (current == _busTransform && newTarget == _frontPointTransform)
            _currentTransitionSpeed = _centerToFrontSpeed;
        else if (current == _frontPointTransform && newTarget == _busTransform)
            _currentTransitionSpeed = _frontToCenterSpeed;
        else if (current == _busTransform && newTarget == _backPointTransform)
            _currentTransitionSpeed = _centerToBackSpeed;
        else if (current == _backPointTransform && newTarget == _busTransform)
            _currentTransitionSpeed = _backToCenterSpeed;
    }
}