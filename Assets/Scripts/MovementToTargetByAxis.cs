using UnityEngine;

public class MovementToTargetByAxis : MonoBehaviour
{
    public enum MovementState
    {
        Idle,       // Стоим на месте
        MovingRight,// Двигаемся вправо
        MovingLeft, // Двигаемся влево
        MovingUp,   // Двигаемся вверх
        MovingDown  // Двигаемся вниз
    }

    [SerializeField] private float _movementSpeed = 5f; // Скорость перемещения, настраиваемая в инспекторе

    private Vector3 _intermediateTarget; // Промежуточная цель
    private Vector3 _targetPosition; // Конечная цель
    private bool _hasTarget = false;
    private bool _isIntermediateTargetReached = false; // Достигнута ли промежуточная цель
    private bool _movingAlongX = true; // Двигаемся по оси X сначала
    private bool _isTargetSet = false; // Установлены ли цели

    public bool HasTarget => _hasTarget; // Свойство только для чтения
    public MovementState CurrentState { get; private set; } = MovementState.Idle;

    // Установка промежуточной и конечной целей
    public void SetTargets(Vector3 intermediateTarget, Vector3 finalTarget)
    {
        _intermediateTarget = intermediateTarget;
        _targetPosition = finalTarget;
        _isIntermediateTargetReached = false; // Сбрасываем статус
        _hasTarget = true;
        _isTargetSet = true;

        // Начинаем с промежуточной цели
        _movingAlongX = Mathf.Abs(_intermediateTarget.x - transform.position.x) > Mathf.Abs(_intermediateTarget.y - transform.position.y);

        Debug.Log($"Intermediate target set to: {_intermediateTarget}, final target set to: {_targetPosition}");
    }

    // Сброс цели
    public void ResetTarget()
    {
        _hasTarget = false;
        _isIntermediateTargetReached = false;
        _isTargetSet = false;
        CurrentState = MovementState.Idle;
        Debug.Log("Target reset, passenger is idle.");
    }

    private void Update()
    {
        if (_hasTarget)
        {
            MoveTowardsTarget();
        }
        else
        {
            CurrentState = MovementState.Idle; // Если нет цели, стоим на месте
        }

        // Проверка, если цель была установлена, но ещё не достигнута
        if (!_hasTarget && _isTargetSet)
        {
            if (Mathf.Abs(transform.position.x - _targetPosition.x) > 0.01f || Mathf.Abs(transform.position.y - _targetPosition.y) > 0.01f)
            {
                _hasTarget = true;
                _movingAlongX = Mathf.Abs(_targetPosition.x - transform.position.x) > Mathf.Abs(_targetPosition.y - transform.position.y);
            }
        }
    }

    // Метод для перемещения к цели
    private void MoveTowardsTarget()
    {
        // Проверяем, достигли ли промежуточной цели
        if (!_isIntermediateTargetReached)
        {
            MoveTowards(_intermediateTarget);

            if (HasReachedTarget(_intermediateTarget))
            {
                _isIntermediateTargetReached = true;
                _movingAlongX = Mathf.Abs(_targetPosition.x - transform.position.x) > Mathf.Abs(_targetPosition.y - transform.position.y);
                Debug.Log("Intermediate target reached, moving to final target.");
            }
        }
        else
        {
            MoveTowards(_targetPosition);

            if (HasReachedTarget(_targetPosition))
            {
                _hasTarget = false; // Конечная цель достигнута
                Debug.Log("Final target reached.");
            }
        }
    }

    // Метод для перемещения к указанной цели
    private void MoveTowards(Vector3 target)
    {
        Vector3 currentPosition = transform.position;

        if (_movingAlongX)
        {
            // Движение по оси X
            currentPosition.x = Mathf.MoveTowards(currentPosition.x, target.x, Time.deltaTime * _movementSpeed);
            CurrentState = target.x > transform.position.x ? MovementState.MovingRight : MovementState.MovingLeft;

            if (Mathf.Abs(currentPosition.x - target.x) < 0.01f)
            {
                _movingAlongX = false; // Переключаемся на ось Y
            }
        }
        else
        {
            // Движение по оси Y
            currentPosition.y = Mathf.MoveTowards(currentPosition.y, target.y, Time.deltaTime * _movementSpeed);
            CurrentState = target.y > transform.position.y ? MovementState.MovingUp : MovementState.MovingDown;

            if (Mathf.Abs(currentPosition.y - target.y) < 0.01f)
            {
                _movingAlongX = true; // Переключаемся на ось X
            }
        }

        transform.position = currentPosition;
    }

    // Проверка достижения цели
    private bool HasReachedTarget(Vector3 target)
    {
        return Mathf.Abs(transform.position.x - target.x) < 0.01f && Mathf.Abs(transform.position.y - target.y) < 0.01f;
    }

    private void OnDrawGizmos()
    {
        if (_hasTarget)
        {
            Vector3 passengerPosition = transform.position;

            // Всегда отрисовываем конечную цель
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_targetPosition, 0.3f);

            if (!_isIntermediateTargetReached)
            {
                // Отрисовка промежуточной цели (если она не достигнута)
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_intermediateTarget, 0.3f);

                // Линия от текущей позиции до промежуточной цели (толстая)
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(
                            passengerPosition + new Vector3(i * 0.05f, j * 0.05f, 0),
                            _intermediateTarget + new Vector3(i * 0.05f, j * 0.05f, 0));
                    }
                }

                // Линия от промежуточной до конечной цели (толстая)
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(
                            _intermediateTarget + new Vector3(i * 0.05f, j * 0.05f, 0),
                            _targetPosition + new Vector3(i * 0.05f, j * 0.05f, 0));
                    }
                }
            }
            else
            {
                // Если промежуточная цель достигнута, рисуем линию от текущей позиции до конечной цели (толстая)
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(
                            passengerPosition + new Vector3(i * 0.05f, j * 0.05f, 0),
                            _targetPosition + new Vector3(i * 0.05f, j * 0.05f, 0));
                    }
                }
            }
        }
    }
}