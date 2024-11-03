using UnityEngine;

public class CarAIInputSystem : MonoBehaviour
{
    [SerializeField] private TileTriggerChecker _frontTileTriggerChecker; // Reference to front TileTriggerChecker
    [SerializeField] private TileTriggerChecker _rightTileTriggerChecker; // Reference to right TileTriggerChecker
    [SerializeField] private TileTriggerChecker _leftTileTriggerChecker;  // Reference to left TileTriggerChecker
    [SerializeField] private CarMovement _carMovement; // Reference to CarMovement component
    [SerializeField] private ForwardDetection _forwardDetection; // Reference to ForwardDetection component

    private enum TurnDirection { None, Right, Left }
    private TurnDirection _currentTurnDirection = TurnDirection.None;

    private void Update()
    {
        // If there is something ahead, stop the car
        if (_forwardDetection.IsSomethingAhead)
        {
            _carMovement.Stop();
        }
        else
        {
            // Check if there are fewer than 8 tiles in front
            if (_frontTileTriggerChecker.TileCountInArea < 6)
            {
                // If no turn direction is set, determine the better turn direction
                if (_currentTurnDirection == TurnDirection.None)
                {
                    if (_rightTileTriggerChecker.TileCountInArea > _leftTileTriggerChecker.TileCountInArea)
                    {
                        _currentTurnDirection = TurnDirection.Right;
                    }
                    else if (_leftTileTriggerChecker.TileCountInArea > _rightTileTriggerChecker.TileCountInArea)
                    {
                        _currentTurnDirection = TurnDirection.Left;
                    }
                    else
                    {
                        _currentTurnDirection = TurnDirection.Right; // Default turn
                    }
                }

                // Execute the current turn direction
                if (_currentTurnDirection == TurnDirection.Right)
                {
                    _carMovement.TurnRight();
                }
                else if (_currentTurnDirection == TurnDirection.Left)
                {
                    _carMovement.TurnLeft();
                }
            }
            else
            {
                // If there are 8 or more tiles in front, reset the turn direction
                _currentTurnDirection = TurnDirection.None;
            }

            // Move forward only if there is no obstacle
            _carMovement.MoveForward();
        }
    }
}