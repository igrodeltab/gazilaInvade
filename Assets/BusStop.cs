using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField] private GameObject _passengerPrefab; // Префаб пассажира
    private const float _minVelocityToUnload = 0.01f; // Минимальная скорость для высадки пассажиров
    private BusPickingUpPassenger _bus;
    private Rigidbody2D _busRigidbody;
    private bool _passengersUnloaded = false; // Флаг, отслеживающий высадку пассажиров

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _bus = collider.GetComponent<BusPickingUpPassenger>();
        _busRigidbody = collider.GetComponent<Rigidbody2D>();
        _passengersUnloaded = false; // Сбросить флаг при входе автобуса в остановку
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!_passengersUnloaded && _bus != null && _bus.DropOffPassengerPoint != null && _busRigidbody != null)
        {
            if (_busRigidbody.velocity.magnitude < _minVelocityToUnload)
            {
                UnloadPassengers(_bus, _bus.DropOffPassengerPoint);
                _passengersUnloaded = true; // Установить флаг, чтобы избежать повторной высадки
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (_bus != null && collider.GetComponent<BusPickingUpPassenger>() == _bus)
        {
            _bus = null;
            _busRigidbody = null;
            _passengersUnloaded = false; // Сбросить флаг при выходе автобуса из остановки
        }
    }

    private void UnloadPassengers(BusPickingUpPassenger bus, Transform dropOffPoint)
    {
        int passengersToUnload = Random.Range(0, bus.CurrentPassengers + 1);
        for (int i = 0; i < passengersToUnload; i++)
        {
            Instantiate(_passengerPrefab, dropOffPoint.position, Quaternion.identity);
            bus.RemovePassenger();
        }
    }
}