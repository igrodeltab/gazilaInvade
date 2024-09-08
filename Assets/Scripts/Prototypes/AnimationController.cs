using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator; // Ссылка на компонент Animator
    [SerializeField] private MovementToTargetByAxisDraft _movementScript; // Ссылка на скрипт движения

    private void Update()
    {
        // Получаем текущее состояние движения
        MovementToTargetByAxisDraft.MovementState currentState = _movementScript.CurrentState;

        // Определяем значения для параметров анимации в зависимости от состояния
        float horizontal = 0f;
        float vertical = 0f;
        float speed = 0f;

        switch (currentState)
        {
            case MovementToTargetByAxisDraft.MovementState.MovingRight:
                horizontal = 1f;
                vertical = 0f;
                speed = 1f;
                break;

            case MovementToTargetByAxisDraft.MovementState.MovingLeft:
                horizontal = -1f;
                vertical = 0f;
                speed = 1f;
                break;

            case MovementToTargetByAxisDraft.MovementState.MovingUp:
                horizontal = 0f;
                vertical = 1f;
                speed = 1f;
                break;

            case MovementToTargetByAxisDraft.MovementState.MovingDown:
                horizontal = 0f;
                vertical = -1f;
                speed = 1f;
                break;

            case MovementToTargetByAxisDraft.MovementState.Idle:
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