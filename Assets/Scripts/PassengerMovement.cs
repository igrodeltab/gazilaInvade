using UnityEngine;

public class PassengerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _waitTimeAfterDropOff = 5f;
    [SerializeField] private float _angleRange = 30f; // Диапазон угла разброса в градусах
    private MovementToTargetByAxis _movementToTargetScript; // Ссылка на скрипт перемещения

    private float _waitTimer;
    private bool _isReadyToBoard = true;
    private Vector2 _moveAwayDirection;
    private bool _isTargetSet = false; // Добавляем переменную для отслеживания установки цели

    public bool IsReadyToBoard => _isReadyToBoard;
    public bool IsBoardingBus = false;

    private void Awake()
    {
        _movementToTargetScript = GetComponent<MovementToTargetByAxis>();
        if (_movementToTargetScript == null)
        {
            Debug.LogError("MovementToTargetByAxis не найден на объекте PassengerMovement.");
        }

        IsBoardingBus = false;
    }

    private void Update()
    {
        if (!_isReadyToBoard)
        {
            MoveAwayFromBus();
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _isReadyToBoard = true;
                _isTargetSet = false; // Сбрасываем флаг при готовности к посадке
            }
        }
    }

    public void MoveTowards(Transform targetTransform)
    {
        // Проверяем, была ли уже установлена цель
        if (_isReadyToBoard && !_isTargetSet)
        {
            // Определяем промежуточную цель
            Vector3 intermediateTarget = new Vector3(
                targetTransform.position.x,
                transform.position.y,
                0); // Сначала двигаемся по X, затем к Y

            // Устанавливаем цели в компонент движения
            _movementToTargetScript.SetTargets(intermediateTarget, targetTransform.position);
            Debug.Log("MoveTowards called, setting intermediate and final targets");
            _isTargetSet = true; // Цель установлена, больше не задаём её
        }
    }

    public void ResetTarget()
    {
        // Вызываем сброс цели через скрипт MovementToTargetByAxis
        _movementToTargetScript.ResetTarget();
        _isTargetSet = false; // Также сбрасываем этот флаг
        Debug.Log("Passenger target reset.");
    }

    private void MoveAwayFromBus()
    {
        transform.position += (Vector3)_moveAwayDirection * _moveSpeed * Time.deltaTime;
    }

    public void SetAsDroppedOff(Vector2 moveAwayDirection)
    {
        _isReadyToBoard = false;
        _waitTimer = _waitTimeAfterDropOff;

        float randomAngle = Random.Range(-_angleRange / 2, _angleRange / 2);
        Vector2 rotatedDirection = Quaternion.Euler(0, 0, randomAngle) * moveAwayDirection;
        _moveAwayDirection = rotatedDirection.normalized;
    }
}