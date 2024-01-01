using UnityEngine;

public class PassengerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f; // Скорость движения пассажира
    [SerializeField] private float _waitTimeAfterDropOff = 5f; // Время ожидания перед тем, как снова подойти к автобусу

    private float _waitTimer; // Таймер для отслеживания времени ожидания
    private bool _isReadyToBoard = true; // Флаг, показывающий, готов ли пассажир сесть в автобус
    private Vector2 _moveAwayDirection; // Направление для движения после высадки

    public bool IsReadyToBoard => _isReadyToBoard; // Публичный геттер для _isReadyToBoard

    private void Update()
    {
        if (!_isReadyToBoard)
        {
            MoveAwayFromBus();
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _isReadyToBoard = true;
            }
        }
    }

    public void MoveTowards(Transform targetTransform)
    {
        if (_isReadyToBoard)
        {
            // Двигаемся к цели (автобусу)
            Vector2 direction = (targetTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * _moveSpeed * Time.deltaTime;
        }
    }

    private void MoveAwayFromBus()
    {
        transform.position += (Vector3)_moveAwayDirection * _moveSpeed * Time.deltaTime;
    }

    public void SetAsDroppedOff(Vector2 moveAwayDirection)
    {
        _isReadyToBoard = false;
        _waitTimer = _waitTimeAfterDropOff;
        _moveAwayDirection = moveAwayDirection;
    }
}