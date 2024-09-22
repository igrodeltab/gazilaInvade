using UnityEngine;

public class SetPassengerTargetForWalking : MonoBehaviour
{
    [SerializeField] private float minTargetRadius = 4f; // Минимальное значение радиуса
    [SerializeField] private float maxTargetRadius = 6f; // Максимальное значение радиуса
    private MovementToTargetByAxis movementComponent; // Ссылка на компонент движения пассажира

    private void Start()
    {
        movementComponent = GetComponent<MovementToTargetByAxis>();

        if (movementComponent == null)
        {
            throw new MissingComponentException("MovementToTargetByAxis component is missing on the passenger.");
        }
    }

    private void Update()
    {
        // Если у пассажира не задана цель, задаем новую случайную цель
        if (!movementComponent.HasTarget) // Проверяем свойство только для чтения
        {
            AssignRandomTarget();
        }
    }

    // Метод для задания случайной цели в радиусе
    private void AssignRandomTarget()
    {
        // Генерируем случайный радиус между minTargetRadius и maxTargetRadius
        float targetRadius = Random.Range(minTargetRadius, maxTargetRadius);
        Vector3 randomDirection = Random.insideUnitCircle * targetRadius; // Выбираем случайную точку в радиусе
        Vector3 randomTarget = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0); // Убираем глубину для 2D

        movementComponent.SetTarget(randomTarget); // Задаем новую цель через компонент движения
    }
}