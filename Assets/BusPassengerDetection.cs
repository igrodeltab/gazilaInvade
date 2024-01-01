using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BusPassengerDetection : MonoBehaviour
{
    private List<PassengerMovement> _passengersNearby = new List<PassengerMovement>();
    private BusPickingUpPassenger _busPickingUpPassenger; // Ссылка на скрипт управления автобусом

    private void Awake()
    {
        _busPickingUpPassenger = GetComponentInParent<BusPickingUpPassenger>();
    }

    private void Update()
    {
        if (GetComponentInParent<Rigidbody2D>().velocity.magnitude < 0.1f && HasFreeSeats())
        {
            foreach (var passenger in _passengersNearby)
            {
                if (passenger.IsReadyToBoard) // Проверка, готов ли пассажир к посадке
                {
                    passenger.MoveTowards(this.transform);
                }
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