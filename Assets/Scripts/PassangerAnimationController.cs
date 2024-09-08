using UnityEngine;

public class PassengerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator; // Ссылка на компонент Animator
    [SerializeField] private MovementToTargetByAxis _movementScript; // Ссылка на скрипт движения

    private void Update()
    {
        // Получаем текущее состояние движения
        MovementToTargetByAxis.MovementState currentState = _movementScript.CurrentState;

        // Определяем значения для параметров анимации в зависимости от состояния
        float horizontal = 0f;
        float vertical = 0f;
        float speed = 0f;

        switch (currentState)
        {
            case MovementToTargetByAxis.MovementState.MovingRight:
                horizontal = 1f;
                vertical = 0f;
                speed = 1f;
                break;

            case MovementToTargetByAxis.MovementState.MovingLeft:
                horizontal = -1f;
                vertical = 0f;
                speed = 1f;
                break;

            case MovementToTargetByAxis.MovementState.MovingUp:
                horizontal = 0f;
                vertical = 1f;
                speed = 1f;
                break;

            case MovementToTargetByAxis.MovementState.MovingDown:
                horizontal = 0f;
                vertical = -1f;
                speed = 1f;
                break;

            case MovementToTargetByAxis.MovementState.Idle:
            default:
                horizontal = 0f;
                vertical = 0f;
                speed = 0f;
                break;
        }

        // Устанавливаем значения для параметров анимации
        _animator.SetFloat("Horizontal", horizontal);
        _animator.SetFloat("Vertical", vertical);
        _animator.SetFloat("Speed", speed);
    }
}