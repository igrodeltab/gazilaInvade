using UnityEngine;

public class SetPassengerTargetForWalking : MonoBehaviour
{
    [SerializeField] private float targetRadius = 5f; // Радиус для выбора случайной цели
    private MovementToTargetByAxis movementComponent; // Ссылка на компонент движения пассажира

    private void Start()
    {
        if (movementComponent == null)
        {
            movementComponent = GetComponent<MovementToTargetByAxis>();
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
        Vector3 randomDirection = Random.insideUnitCircle * targetRadius; // Выбираем случайную точку в радиусе
        Vector3 randomTarget = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0); // Убираем глубину для 2D

        movementComponent.SetTarget(randomTarget); // Задаем новую цель через компонент движения
        Debug.Log($"New random target assigned: {randomTarget}");
    }
}