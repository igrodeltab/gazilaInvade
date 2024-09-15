using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BusPassengerDetection : MonoBehaviour
{
    private List<PassengerMovement> _passengersNearby = new List<PassengerMovement>();
    private BusPickingUpPassenger _busPickingUpPassenger; // Ссылка на скрипт управления автобусом
    private Rigidbody2D _busRigidbody;

    private void Awake()
    {
        _busPickingUpPassenger = GetComponentInParent<BusPickingUpPassenger>();
        _busRigidbody = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        // Если автобус остановился и есть свободные места, пассажиры начинают садиться
        if (_busRigidbody.velocity.magnitude < 0.1f && HasFreeSeats())
        {
            foreach (var passenger in _passengersNearby)
            {
                if (passenger.IsReadyToBoard) // Проверка, готов ли пассажир к посадке
                {
                    passenger.MoveTowards(this.transform);
                }
            }
        }
        // Если автобус начинает движение, сбрасываем цель для всех пассажиров, которые не успели сесть
        else if (_busRigidbody.velocity.magnitude > 0.1f)
        {
            foreach (var passenger in _passengersNearby)
            {
                passenger.ResetTarget(); // Сбрасываем цель пассажиров
                Debug.Log("Bus started moving, resetting passenger targets.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PassengerMovement passenger = collision.GetComponent<PassengerMovement>();
        if (passenger)
        {
            _passengersNearby.Add(passenger);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PassengerMovement passenger = collision.GetComponent<PassengerMovement>();
        if (passenger)
        {
            _passengersNearby.Remove(passenger);
        }
    }

    private bool HasFreeSeats()
    {
        return _busPickingUpPassenger != null && _busPickingUpPassenger.CurrentPassengers < _busPickingUpPassenger.MaxPassengers;
    }
}