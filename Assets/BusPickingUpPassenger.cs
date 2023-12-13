using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BusPickingUpPassenger : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Проверяем, вошел ли в триггер объект с тегом "Passenger"
        if (collider.CompareTag("Passenger"))
        {
            // Проверяем, стоит ли автобус на месте
            if (_rigidbody2D.velocity.magnitude < 0.01f) // автобус практически не движется
            {
                // "Подбираем" пассажира (здесь можно добавить логику по добавлению пассажира в автобус)
                PickUpPassenger(collider.gameObject);
            }
        }
    }

    private void PickUpPassenger(GameObject passenger)
    {
        // Логика по обработке пассажира, например, удаление объекта или другие действия
        Destroy(passenger);
    }
}