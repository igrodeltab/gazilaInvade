using UnityEngine;

public class CarAIInputSystem : MonoBehaviour
{
    [SerializeField] private TileTriggerChecker _frontTileTriggerChecker; // Reference to TileTriggerChecker
    [SerializeField] private TileTriggerChecker _rightTileTriggerChecker;
    [SerializeField] private TileTriggerChecker _leftTileTriggerChecker;
    [SerializeField] private CarMovement _carMovement; // Reference to CarMovement component

    private void Update()
    {
        // Check the area for tiles and send commands
        if (_frontTileTriggerChecker.TileCountInArea >= 2)
        {
            _carMovement.MoveForward(); // Command to move forward
        }
        else if (_rightTileTriggerChecker.TileCountInArea >= 2)
        {
            _carMovement.MoveForward();
            _carMovement.TurnRight();
        }
        else if (_leftTileTriggerChecker.TileCountInArea >= 2)
        {
            _carMovement.MoveForward();
            _carMovement.TurnLeft();
        }
        else
        {
            _carMovement.Stop(); // Command to stop
        }
    }
}