using UnityEngine;

public class PassengerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _waitTimeAfterDropOff = 5f;
    [SerializeField] private float _angleRange = 30f; // Диапазон угла разброса в градусах

    private float _waitTimer;
    private bool _isReadyToBoard = true;
    private Vector2 _moveAwayDirection;

    public bool IsReadyToBoard => _isReadyToBoard;

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

        float randomAngle = Random.Range(-_angleRange / 2, _angleRange / 2);
        Vector2 rotatedDirection = Quaternion.Euler(0, 0, randomAngle) * moveAwayDirection;
        _moveAwayDirection = rotatedDirection.normalized;
    }
}