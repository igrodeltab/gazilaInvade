using UnityEngine;

[CreateAssetMenu(fileName = "PassengerData", menuName = "Game/Passenger Data", order = 0)]
public class PassengerData : ScriptableObject
{
    public int _paymentAmount; // Количество денег, которое пассажир платит за поездку

    // Можно добавить другие атрибуты, характерные для пассажира
}