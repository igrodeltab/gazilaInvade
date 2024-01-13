using UnityEngine;

public class CameraFollowBus : MonoBehaviour
{
    [SerializeField] private Transform _busTransform; // Ссылка на трансформ автобуса
    [SerializeField] private Transform _frontPointTransform; // Точка перед автобусом
    [SerializeField] private Transform _backPointTransform; // Точка сзади автобуса
    [SerializeField] private float _transitionSpeed = 1f; // Скорость перехода между целями

    private Rigidbody2D _busRigidbody;

    private void Start()
    {
        _busRigidbody = _busTransform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Transform targetTransform = DetermineTargetTransform();
        Vector3 targetPosition = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _transitionSpeed * Time.deltaTime);
    }

    private Transform DetermineTargetTransform()
    {
        if (_busRigidbody.velocity.magnitude < 0.1f) // Если автобус практически остановился
        {
            return _busTransform;
        }

        // Определение направления движения автобуса (вперед или назад)
        Vector2 busDirection = _busRigidbody.velocity.normalized;
        Vector2 busForward = _busTransform.up; // Предполагаем, что 'up' трансформа автобуса направлено вперед
        bool movingForward = Vector2.Dot(busDirection, busForward) > 0;

        return movingForward ? _frontPointTransform : _backPointTransform;
    }
}