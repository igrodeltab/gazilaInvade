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

    private Vector3 _targetPosition;
    private bool _hasTarget = false;
    private bool _movingAlongX = true;

    public MovementState CurrentState { get; private set; } = MovementState.Idle;

    void Update()
    {
        // Проверяем нажатие мыши
        if (Input.GetMouseButtonDown(0))
        {
            // Преобразуем позицию клика в мировые координаты
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane; // Задаём расстояние от камеры до точки клика
            _targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _targetPosition.z = 0; // Убираем глубину, если работаем в 2D
            _hasTarget = true;

            // Определяем, по какой оси будем двигаться сначала
            _movingAlongX = Mathf.Abs(_targetPosition.x - transform.position.x) > Mathf.Abs(_targetPosition.y - transform.position.y);
        }

        if (_hasTarget)
        {
            MoveTowardsTarget();
        }
        else
        {
            CurrentState = MovementState.Idle; // Если нет цели, стоим на месте
        }

        // Дополнительная проверка для случаев, когда объект может остановиться раньше
        if (!_hasTarget)
        {
            if (Mathf.Abs(transform.position.x - _targetPosition.x) > 0.01f || Mathf.Abs(transform.position.y - _targetPosition.y) > 0.01f)
            {
                _hasTarget = true;
                _movingAlongX = Mathf.Abs(_targetPosition.x - transform.position.x) > Mathf.Abs(_targetPosition.y - transform.position.y);
            }
        }
    }

    private void MoveTowardsTarget()
    {
        // Получаем текущие координаты объекта
        Vector3 currentPosition = transform.position;

        if (_movingAlongX)
        {
            // Двигаемся по оси X с заданной скоростью
            currentPosition.x = Mathf.MoveTowards(currentPosition.x, _targetPosition.x, Time.deltaTime * _movementSpeed);

            // Определяем направление движения по X
            if (Mathf.Sign(_targetPosition.x - transform.position.x) > 0)
            {
                CurrentState = MovementState.MovingRight;
            }
            else if (Mathf.Sign(_targetPosition.x - transform.position.x) < 0)
            {
                CurrentState = MovementState.MovingLeft;
            }

            // Проверяем, достигли ли мы нужной позиции по X
            if (Mathf.Abs(currentPosition.x - _targetPosition.x) < 0.01f)
            {
                _movingAlongX = false; // Переключаемся на ось Y
            }
        }

        if (!_movingAlongX && _hasTarget)  // Если движение по X завершено
        {
            // Двигаемся по оси Y с заданной скоростью
            currentPosition.y = Mathf.MoveTowards(currentPosition.y, _targetPosition.y, Time.deltaTime * _movementSpeed);

            // Определяем направление движения по Y
            if (Mathf.Sign(_targetPosition.y - transform.position.y) > 0)
            {
                CurrentState = MovementState.MovingUp;
            }
            else if (Mathf.Sign(_targetPosition.y - transform.position.y) < 0)
            {
                CurrentState = MovementState.MovingDown;
            }

            // Проверяем, достигли ли мы нужной позиции по Y
            if (Mathf.Abs(currentPosition.y - _targetPosition.y) < 0.01f)
            {
                _hasTarget = false; // Движение завершено
            }
        }

        // Обновляем позицию объекта
        transform.position = currentPosition;
    }
}