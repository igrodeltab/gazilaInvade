using UnityEngine;

public class CarAIInputSystem : MonoBehaviour
{
    [SerializeField] private TileTriggerChecker _frontTileTriggerChecker; // Reference to front TileTriggerChecker
    [SerializeField] private TileTriggerChecker _rightTileTriggerChecker; // Reference to right TileTriggerChecker
    [SerializeField] private TileTriggerChecker _leftTileTriggerChecker;  // Reference to left TileTriggerChecker
    [SerializeField] private CarMovement _carMovement; // Reference to CarMovement component

    private void Update()
    {
        // Check the area for tiles and send commands
        if (_frontTileTriggerChecker.TileCountInArea >= 2)
        {
            _carMovement.MoveForward(); // Command to move forward
        }
        else
        {
            // Compare right and left tile counts to determine the better turn direction
            if (_rightTileTriggerChecker.TileCountInArea > _leftTileTriggerChecker.TileCountInArea)
            {
                _carMovement.MoveForward();
                _carMovement.TurnRight();
            }
            else if (_leftTileTriggerChecker.TileCountInArea > _rightTileTriggerChecker.TileCountInArea)
            {
                _carMovement.MoveForward();
                _carMovement.TurnLeft();
            }
            else
            {
                // If both sides are equal or no tiles, choose a default turn or stop
                _carMovement.Stop(); // or _carMovement.TurnRight() if you prefer a default turn
            }
        }
    }
}