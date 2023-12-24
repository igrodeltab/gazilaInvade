using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField] private GameObject passengerPrefab; // Префаб пассажира

    private void OnTriggerEnter2D(Collider2D collider)
    {
        BusPickingUpPassenger bus = collider.GetComponent<BusPickingUpPassenger>();
        if (bus != null && bus.DropOffPassengerPoint != null)
        {
            UnloadPassengers(bus, bus.DropOffPassengerPoint);
        }
    }

    private void UnloadPassengers(BusPickingUpPassenger bus, Transform dropOffPoint)
    {
        int passengersToUnload = Random.Range(0, bus.CurrentPassengers + 1);
        for (int i = 0; i < passengersToUnload; i++)
        {
            Instantiate(passengerPrefab, dropOffPoint.position, Quaternion.identity);
            bus.RemovePassenger();
        }
    }
}