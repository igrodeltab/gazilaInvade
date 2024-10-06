using UnityEngine;
using System.Collections;

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
    [SerializeField] private GameObject _bloodPrefab; // Prefab for blood
    [SerializeField] private float _bloodSpawnInterval = 0.5f; // Interval between blood spawns (in seconds)
    [SerializeField] private float _bloodSpawnDuration = 3f; // Duration for blood spawns (in seconds)

    public Transform DropOffPassengerPoint => _dropOffPassengerPoint; // Public read-only property for drop-off point
    public int CurrentPassengers => _currentPassengers; // Public read-only property for current passengers
    public int MaxPassengers => _maxPassengers; // Public read-only property for max passengers

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

        // Start spawning blood after crushing the passenger
        StartCoroutine(SpawnBloodCoroutine(passenger.transform.position));

        Destroy(passenger);
    }

    private IEnumerator SpawnBloodCoroutine(Vector3 collisionPoint)
    {
        float timeElapsed = 0f;

        while (timeElapsed < _bloodSpawnDuration)
        {
            // Spawn blood at the current bus position
            Instantiate(_bloodPrefab, transform.position, Quaternion.identity);

            // Wait for the next interval
            yield return new WaitForSeconds(_bloodSpawnInterval);

            // Update the elapsed time
            timeElapsed += _bloodSpawnInterval;
        }
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