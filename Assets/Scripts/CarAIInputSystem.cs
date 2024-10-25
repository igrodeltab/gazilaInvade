using UnityEngine;

public class CarAIInputSystem : MonoBehaviour
{
    [SerializeField] private TileTriggerChecker _tileTriggerChecker; // Reference to TileTriggerChecker
    [SerializeField] private CarMovement _carMovement; // Reference to CarMovement component

    private void Update()
    {
        // Check the area for tiles and send commands
        if (_tileTriggerChecker.TileCountInArea >= 2)
        {
            _carMovement.MoveForward(); // Command to move forward
        }
        else
        {
            _carMovement.Stop(); // Command to stop
        }
    }
}