using UnityEngine;

public class BusPickingUpPassenger : MonoBehaviour
{
    [SerializeField] private int _ticketPrice;
    [ReadOnly] [SerializeField] private int _totalEarnings;
    [SerializeField] private int _maxPassengers = 10; // Максимальное количество пассажиров
    [ReadOnly] [SerializeField] private int _currentPassengers = 0; // Текущее количество пассажиров
    [SerializeField] private Transform _dropOffPassengerPoint; // Приватное поле для ссылки на точку высадки
    [SerializeField] private GameObject _crushedPassenger; // GameObject that holds the sprite of a crushed passenger
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private ShowTicketPrice _showTicketPrice;

    public Transform DropOffPassengerPoint => _dropOffPassengerPoint; // Публичное свойство только для чтения
    public int CurrentPassengers => _currentPassengers; // Только для чтения
    public int MaxPassengers => _maxPassengers; // Только для чтения

    private void Awake()
    {
        _totalEarnings = 0;
        _currentPassengers = 0;
        PassengerCounter.Instance?.UpdatePassengerCount(CurrentPassengers, MaxPassengers);
        TotalEarningsCounter.Instance?.UpdateTotalEarningsCount(_totalEarnings);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PassengerMovement passengerMovement = collider.GetComponent<PassengerMovement>();

        if (passengerMovement != null)
        {
            if (_rigidbody2D.velocity.magnitude > 0.01f)
            {
                CrushPassenger(collider.gameObject);
            }
            else if (passengerMovement.IsReadyToBoard)
            {
                PickUpPassenger(collider.gameObject);
            }
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

    private void CrushPassenger(GameObject passenger)
    {
        // Spawn the crushed passenger sprite at the passenger's position
        Instantiate(_crushedPassenger, passenger.transform.position, Quaternion.identity);
        Destroy(passenger);
    }

    public void RemovePassenger()
    {
        if (_currentPassengers > 0)
        {
            _currentPassengers--;
            PassengerCounter.Instance?.UpdatePassengerCount(_currentPassengers, _maxPassengers);
        }
    }
}