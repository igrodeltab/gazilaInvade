using UnityEngine;

public class TicketPrice : MonoBehaviour
{
    [SerializeField]
    private PassengerData _passengerData; // Ссылка на данные пассажира

    // Метод для получения стоимости билета
    public int GetFareAmount()
    {
        return _passengerData._paymentAmount;
    }
}