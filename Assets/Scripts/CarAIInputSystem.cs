using UnityEngine;

public class CarAIInputSystem : MonoBehaviour
{
    [SerializeField] private TileTriggerChecker _frontTileTriggerChecker; // Reference to front TileTriggerChecker
    [SerializeField] private TileTriggerChecker _rightTileTriggerChecker; // Reference to right TileTriggerChecker
    [SerializeField] private TileTriggerChecker _leftTileTriggerChecker;  // Reference to left TileTriggerChecker
    [SerializeField] private CarMovement _carMovement; // Reference to CarMovement component

    private void Update()
    {
        // Check if there are fewer than 8 tiles in front
        if (_frontTileTriggerChecker.TileCountInArea < 8)
        {
            // Compare right and left tile counts to determine the better turn direction
            if (_rightTileTriggerChecker.TileCountInArea > _leftTileTriggerChecker.TileCountInArea)
            {
                _carMovement.TurnRight();
            }
            else if (_leftTileTriggerChecker.TileCountInArea > _rightTileTriggerChecker.TileCountInArea)
            {
                _carMovement.TurnLeft();
            }
            else
            {
                // If both sides are equal or no tiles, choose a default turn
                _carMovement.TurnRight(); // Default turn
            }
        }

        // Always move forward
        _carMovement.MoveForward();
    }
}