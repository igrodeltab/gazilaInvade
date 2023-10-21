using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BusPassengerDetection : MonoBehaviour
{
    private List<PassengerBehaviour> passengersNearby = new List<PassengerBehaviour>();

    private void Update()
    {
        if (GetComponentInParent<Rigidbody2D>().velocity.magnitude < 0.1f)
        {
            foreach (var passenger in passengersNearby)
            {
                passenger.MoveTowards(this.transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PassengerBehaviour passenger = collision.GetComponent<PassengerBehaviour>();
        if (passenger)
        {
            passengersNearby.Add(passenger);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PassengerBehaviour passenger = collision.GetComponent<PassengerBehaviour>();
        if (passenger)
        {
            passengersNearby.Remove(passenger);
        }
    }
}