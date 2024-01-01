using UnityEngine;

public class PassengerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f; // Скорость движения пассажира
    [SerializeField] private float _waitTimeAfterDropOff = 5f; // Время ожидания перед тем, как снова подойти к автобусу

    private float _waitTimer;
    private bool _isReadyToBoard = true; // Состояние готовности сесть в автобус
    public bool IsReadyToBoard => _isReadyToBoard; // Публичный геттер для _isReadyToBoard

    private void Update()
    {
        if (!_isReadyToBoard)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _isReadyToBoard = true; // Пассажир готов снова садиться в автобус
            }
        }
    }

    public void MoveTowards(Transform targetTransform)
    {
        if (_isReadyToBoard)
        {
            // Двигаемся к автобусу
            Vector2 direction = (targetTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * _moveSpeed * Time.deltaTime;
        }
    }

    public void SetAsDroppedOff()
    {
        _isReadyToBoard = false;
        _waitTimer = _waitTimeAfterDropOff;
    }
}