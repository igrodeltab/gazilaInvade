using UnityEngine;
using System.Collections.Generic; // Необходимо для использования List

public class BusStop : MonoBehaviour
{
    [SerializeField] private GameObject _passengerPrefab; // Префаб пассажира
    private const float _minVelocityToUnload = 0.01f; // Минимальная скорость для высадки пассажиров
    private BusPickingUpPassenger _bus;
    private Rigidbody2D _busRigidbody;
    private bool _passengersUnloaded = false; // Флаг, отслеживающий высадку пассажиров
    private List<PassengerMovement> _droppedOffPassengers = new List<PassengerMovement>(); // Список высаженных пассажиров

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
        // Убедитесь, что есть пассажиры для высадки
        if (bus.CurrentPassengers > 0)
        {
            // Определение количества пассажиров для высадки
            int passengersToUnload = Random.Range(1, bus.CurrentPassengers + 1);

            for (int i = 0; i < passengersToUnload; i++)
            {
                // Создание экземпляра префаба пассажира
                GameObject passengerObj = Instantiate(_passengerPrefab, dropOffPoint.position, Quaternion.identity);
                PassengerMovement passenger = passengerObj.GetComponent<PassengerMovement>();

                // Настройка состояния пассажира как "не готов сесть в автобус"
                if (passenger != null)
                {
                    passenger.SetAsDroppedOff();
                    _droppedOffPassengers.Add(passenger); // Если используется список для отслеживания высаженных пассажиров
                }

                // Уменьшение числа пассажиров в автобусе
                bus.RemovePassenger();
            }
        }
    }
}