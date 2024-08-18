using UnityEngine;
using System.Collections.Generic;

public class BusStop : MonoBehaviour
{
    [SerializeField] private GameObject _passengerPrefab;
    private const float _minVelocityToUnload = 0.01f;
    private BusPickingUpPassenger _bus;
    private Rigidbody2D _busRigidbody;
    private bool _passengersUnloaded = false;
    private List<PassengerMovement> _droppedOffPassengers = new List<PassengerMovement>();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _bus = collider.GetComponent<BusPickingUpPassenger>();
        _busRigidbody = collider.GetComponent<Rigidbody2D>();
        _passengersUnloaded = false;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!_passengersUnloaded && _bus != null && _bus.DropOffPassengerPoint != null && _busRigidbody != null)
        {
            if (_busRigidbody.velocity.magnitude < _minVelocityToUnload)
            {
                UnloadPassengers(_bus, _bus.DropOffPassengerPoint);
                _passengersUnloaded = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (_bus != null && collider.GetComponent<BusPickingUpPassenger>() == _bus)
        {
            _bus = null;
            _busRigidbody = null;
            _passengersUnloaded = false;
        }
    }

    private void UnloadPassengers(BusPickingUpPassenger bus, Transform dropOffPoint)
    {
        if (bus.CurrentPassengers > 0)
        {
            int passengersToUnload = Random.Range(1, bus.CurrentPassengers + 1);

            for (int i = 0; i < passengersToUnload; i++)
            {
                GameObject passengerObj = Instantiate(_passengerPrefab, dropOffPoint.position, Quaternion.identity);
                PassengerMovement passenger = passengerObj.GetComponent<PassengerMovement>();

                if (passenger != null)
                {
                    Vector2 moveAwayDirection = (passenger.transform.position - bus.transform.position).normalized;
                    passenger.SetAsDroppedOff(moveAwayDirection);
                    _droppedOffPassengers.Add(passenger);
                }

                bus.RemovePassenger();
            }
        }
    }
}