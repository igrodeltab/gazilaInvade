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
    private TurnDirection _lastTurnDirection = TurnDirection.Right; // Track last turn direction

    private void Update()
    {
        // Get the number of tiles ahead, to the right, and to the left
        int tilesAhead = _frontTileTriggerChecker.TileCountInArea;
        int tilesRight = _rightTileTriggerChecker.TileCountInArea;
        int tilesLeft = _leftTileTriggerChecker.TileCountInArea;

        // If something is ahead, stop the car
        if (_forwardDetection.IsSomethingAhead)
        {
            _carMovement.Stop();
        }
        else
        {
            // Check if there are fewer than 6 tiles in front
            if (tilesAhead < 5)
            {
                // If no turn direction is set, decide the best turn direction
                if (_currentTurnDirection == TurnDirection.None)
                {
                    if (tilesRight > tilesLeft)
                    {
                        _currentTurnDirection = TurnDirection.Right;
                    }
                    else if (tilesLeft > tilesRight)
                    {
                        if (tilesRight < 3)
                        {
                            _currentTurnDirection = TurnDirection.Left;
                        }
                    }
                    else
                    {
                        _carMovement.Stop();
                    }
                }
            }
            else
            {
                if (Mathf.Abs(tilesAhead - tilesRight) <= 1)
                {
                    // If tiles are equal, choose randomly between Right and Forward
                    //_currentTurnDirection = (Random.value > 0.5f) ? TurnDirection.Right : TurnDirection.None;

                    // Alternate between Right and Forward based on the last turn direction
                    if (_lastTurnDirection == TurnDirection.Right)
                    {
                        _currentTurnDirection = TurnDirection.None; // Move Forward
                        _lastTurnDirection = TurnDirection.None;
                    }
                    else if (_lastTurnDirection == TurnDirection.None)
                    {
                        _currentTurnDirection = TurnDirection.Right;
                        _lastTurnDirection = TurnDirection.Right;
                    }
                }
                else
                {
                    // If there are 6 or more tiles in front, reset the turn direction
                    _currentTurnDirection = TurnDirection.None;
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

            // Move forward only if there is no obstacle
            _carMovement.MoveForward();
        }
    }
}