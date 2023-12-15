using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BusPickingUpPassenger : MonoBehaviour
{
    [SerializeField] private int _ticketPrice; // Фиксированная цена за проезд, настраиваемая в инспекторе
    [ReadOnly] [SerializeField] private int _totalEarnings; // Общая сумма денег, заработанная от пассажиров
    
    private Rigidbody2D _rigidbody2D;
    private ShowTicketPrice _showTicketPrice; // Ссылка на скрипт ShowTicketPrice

    private void Awake()
    {
        _totalEarnings = 0;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _showTicketPrice = GetComponent<ShowTicketPrice>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Проверяем, вошел ли в триггер объект с тегом "Passenger"
        if (collider.CompareTag("Passenger"))
        {
            // Проверяем, стоит ли автобус на месте
            if (_rigidbody2D.velocity.magnitude < 0.01f) // автобус практически не движется
            {
                // "Подбираем" пассажира
                PickUpPassenger(collider.gameObject);
            }
        }
    }

    private void PickUpPassenger(GameObject passenger)
    {
        // Добавляем стоимость проезда к общей сумме заработка
        _totalEarnings += _ticketPrice;

        //prefabSpawner.SpawnPrefabOnCanvas(passenger.transform.position);

        if (_showTicketPrice != null)
        {
            _showTicketPrice.SpawnPrefabOnCanvas(passenger.transform.position, _ticketPrice);
        }

        // Логика по обработке пассажира, например, удаление объекта или другие действия
        Destroy(passenger);
    }
}