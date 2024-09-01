using UnityEngine;

public class MovementToTargetByAxis : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f; // Скорость перемещения, настраиваемая в инспекторе

    private Vector3 targetPosition;
    private bool hasTarget = false;
    private bool movingAlongX = true;

    void Update()
    {
        // Проверяем нажатие мыши
        if (Input.GetMouseButtonDown(0))
        {
            // Преобразуем позицию клика в мировые координаты
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane; // Задаём расстояние от камеры до точки клика
            targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            targetPosition.z = 0; // Убираем глубину, если работаем в 2D
            hasTarget = true;

            // Определяем, по какой оси будем двигаться сначала
            movingAlongX = Mathf.Abs(targetPosition.x - transform.position.x) > Mathf.Abs(targetPosition.y - transform.position.y);
        }

        if (hasTarget)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        // Получаем текущие координаты объекта
        Vector3 currentPosition = transform.position;

        if (movingAlongX)
        {
            // Двигаемся по оси X с заданной скоростью
            currentPosition.x = Mathf.MoveTowards(currentPosition.x, targetPosition.x, Time.deltaTime * movementSpeed);

            // Проверяем, достигли ли мы нужной позиции по X
            if (Mathf.Abs(currentPosition.x - targetPosition.x) < 0.01f)
            {
                movingAlongX = false; // Переключаемся на ось Y
            }
        }

        if (!movingAlongX && hasTarget)  // Если движение по X завершено
        {
            // Двигаемся по оси Y с заданной скоростью
            currentPosition.y = Mathf.MoveTowards(currentPosition.y, targetPosition.y, Time.deltaTime * movementSpeed);

            // Проверяем, достигли ли мы нужной позиции по Y
            if (Mathf.Abs(currentPosition.y - targetPosition.y) < 0.01f)
            {
                hasTarget = false; // Движение завершено
            }
        }

        // Обновляем позицию объекта
        transform.position = currentPosition;

        // Дополнительная проверка для случаев, когда объект может остановиться раньше
        if (!hasTarget)
        {
            if (Mathf.Abs(transform.position.x - targetPosition.x) > 0.01f || Mathf.Abs(transform.position.y - targetPosition.y) > 0.01f)
            {
                hasTarget = true;
                movingAlongX = Mathf.Abs(targetPosition.x - transform.position.x) > Mathf.Abs(targetPosition.y - transform.position.y);
            }
        }
    }
}