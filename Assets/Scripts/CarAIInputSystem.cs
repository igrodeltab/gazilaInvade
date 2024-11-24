using UnityEngine;
using UnityEngine.Tilemaps;

public class CarAIInputSystem : MonoBehaviour
{
    [SerializeField] private TileCenterChecker _check01; // Tile center checker 1
    [SerializeField] private TileCenterChecker _check02; // Tile center checker 2
    [SerializeField] private TileCenterChecker _check03; // Tile center checker 3
    [SerializeField] private TileCenterChecker _check04; // Tile center checker 4
    [SerializeField] private TileCenterChecker _check05; // Tile center checker 5
    [SerializeField] private TileCenterChecker _check06; // Tile center checker 6
    [SerializeField] private CarMovement _carMovement;    // Reference to CarMovement component
    [SerializeField] private Tilemap _roadTilemap;        // Reference to the road Tilemap

    [SerializeField] private float _rectangleWidth = 2f;  // Width of the car detection rectangle
    [SerializeField] private float _rectangleHeight = 3f; // Height of the car detection rectangle

    private enum CarState
    {
        Forward,
        Right,
        Left
    }

    private CarState _currentState = CarState.Forward; // Default state
    private float _targetRotation = 0f;               // Target rotation in degrees

    private Vector3Int _currentTile;                  // Current tile position

    private void Start()
    {
        // Initialize the current tile
        _currentTile = GetTilePosition(transform.position);
        SetState(); // Set initial state
    }

    private void Update()
    {
        // Always move forward
        _carMovement.MoveForward();

        // Handle state-specific logic
        if (_currentState == CarState.Forward)
        {
            // Check if the car entered a new tile
            Vector3Int newTile = GetTilePosition(transform.position);
            if (newTile != _currentTile)
            {
                _currentTile = newTile;
                SetState(); // Recalculate state
            }
        }
        else if (_currentState == CarState.Right || _currentState == CarState.Left)
        {
            // Check if the rotation matches the target
            if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, _targetRotation)) < 0.1f)
            {
                SetState();
            }
        }

        switch (_currentState)
        {
            case CarState.Forward:
                _targetRotation = 0f;
                break;
            case CarState.Right:
                _carMovement.TurnRight();
                _targetRotation = -90f;
                break;
            case CarState.Left:
                _carMovement.TurnLeft();
                _targetRotation = 90f;
                break;
        }
    }

    /// <summary>
    /// Sets the current state based on road checks.
    /// </summary>
    public void SetState()
    {
        // Update state based on new conditions
        if (_check01.IsRoad && _check02.IsRoad)
        {
            _currentState = CarState.Forward;
        }
        
        if (_check05.IsRoad && !_check06.IsRoad)
        {
            _currentState = CarState.Right;
        }
        
        if (!_check01.IsRoad && !_check02.IsRoad && !_check05.IsRoad && !_check06.IsRoad && _check03.IsRoad && _check04.IsRoad)
        {
            _currentState = CarState.Left;
        }
    }

    /// <summary>
    /// Gets the tile position from the world position.
    /// </summary>
    private Vector3Int GetTilePosition(Vector3 worldPosition)
    {
        return _roadTilemap.WorldToCell(worldPosition);
    }

    private void OnDrawGizmos()
    {
        // Apply local-to-world transformation for correct rotation
        Gizmos.matrix = transform.localToWorldMatrix;

        // Define rectangle size
        Vector3 rectangleCenter = Vector3.zero; // Center in local space
        Vector3 rectangleSize = new Vector3(_rectangleWidth, _rectangleHeight, 0);

        // Draw the rectangle in local space
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(rectangleCenter, rectangleSize);

        // Reset Gizmos matrix
        Gizmos.matrix = Matrix4x4.identity;

#if UNITY_EDITOR
        // Draw state name in the center of the object
        GUIStyle style = new GUIStyle
        {
            fontSize = 16,
            normal = { textColor = Color.white },
            alignment = TextAnchor.MiddleCenter
        };

        UnityEditor.Handles.Label(transform.position, _currentState.ToString(), style);
#endif
    }
}