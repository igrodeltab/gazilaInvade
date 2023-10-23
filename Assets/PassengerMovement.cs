using UnityEngine;

public class PassengerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 1f;          // Скорость движения пассажира

    public void MoveTowards(Transform targetTransform)
    {
        // Двигаемся к автобусу
        Vector2 direction = (targetTransform.position - transform.position).normalized;
        transform.position += (Vector3)direction * _moveSpeed * Time.deltaTime;
    }
}