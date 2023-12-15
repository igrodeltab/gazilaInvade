using UnityEngine;

public class MoveUpText : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1.0f; // Скорость движения

    void Update()
    {
        // Перемещение объекта вверх
        transform.position += Vector3.up * _moveSpeed * Time.deltaTime;

        // Проверка выхода за границы экрана
        if (IsOffScreen())
        {
            Destroy(gameObject); // Уничтожить объект, если он за пределами экрана
        }
    }

    private bool IsOffScreen()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return screenPosition.y > Screen.height; // Проверка, находится ли объект выше верхней границы экрана
    }
}