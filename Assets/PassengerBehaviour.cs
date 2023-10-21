using UnityEngine;

public class PassengerBehaviour : MonoBehaviour
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