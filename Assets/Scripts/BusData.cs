using UnityEngine;

public class BusDriver : MonoBehaviour
{
    public float _moneyCounter = 0; // Счетчик денег

    // Метод, вызываемый, когда пассажир садится на автобус
    public void CollectPaymentFromPassenger(PassengerData passengerData)
    {
        _moneyCounter += passengerData._paymentAmount;
        Debug.Log("Collected payment. Total money: " + _moneyCounter);
    }
}