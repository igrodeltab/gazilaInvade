using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BusPickingUpPassenger : MonoBehaviour
{
    [SerializeField] private int _ticketPrice;
    [ReadOnly] [SerializeField] private int _totalEarnings;
    [SerializeField] private int _maxPassengers = 10; // Максимальное количество пассажиров
    [ReadOnly] [SerializeField] private int _currentPassengers = 0; // Текущее количество пассажиров

    public int CurrentPassengers => _currentPassengers; // Только для чтения
    public int MaxPassengers => _maxPassengers; // Только для чтения

    private Rigidbody2D _rigidbody2D;
    private ShowTicketPrice _showTicketPrice;

    private void Awake()
    {
        _totalEarnings = 0;
        _currentPassengers = 0;
        PassengerCounter.Instance?.UpdatePassengerCount(CurrentPassengers, MaxPassengers);
        TotalEarningsCounter.Instance?.UpdateTotalEarningsCount(_totalEarnings);

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _showTicketPrice = GetComponent<ShowTicketPrice>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Passenger") && _rigidbody2D.velocity.magnitude < 0.01f)
        {
            PickUpPassenger(collider.gameObject);
        }
    }

    private void PickUpPassenger(GameObject passenger)
    {
        if (_currentPassengers < _maxPassengers)
        {
            _totalEarnings += _ticketPrice;
            _currentPassengers++;
            PassengerCounter.Instance?.UpdatePassengerCount(CurrentPassengers, MaxPassengers);
            TotalEarningsCounter.Instance?.UpdateTotalEarningsCount(_totalEarnings);

            if (_showTicketPrice != null)
            {
                _showTicketPrice.SpawnPrefabOnCanvas(passenger.transform.position, _ticketPrice);
            }

            Destroy(passenger);
        }
    }
}