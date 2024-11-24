using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic; // Для работы с List<>

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

    [SerializeField] private float _tolerance = 3f;       // Допустимая погрешность угла

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

        // Текущий угол машины
        float currentAngle = transform.eulerAngles.z;

        // Обработка текущего состояния
        switch (_currentState)
        {
            case CarState.Forward:
                _targetRotation = 0f;

                // Проверка входа в новый тайл
                Vector3Int newTile = GetTilePosition(transform.position);
                if (newTile != _currentTile)
                {
                    _currentTile = newTile;
                    SetState(); // Обновляем состояние
                }
                break;

            case CarState.Right:
                _carMovement.TurnRight();

                // Проверяем, достиг ли угол целевого значения
                if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, _targetRotation)) < _tolerance)
                {
                    SetState(); // Угол достигнут
                }
                break;

            case CarState.Left:
                _carMovement.TurnLeft();

                // Проверяем, достиг ли угол целевого значения
                if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, _targetRotation)) < _tolerance)
                {
                    SetState(); // Угол достигнут
                }
                break;
        }
    }

    private void SetState()
    {
        // Список возможных стейтов
        List<CarState> possibleStates = new List<CarState>();

        // Проверка условий и добавление подходящих стейтов в список
        if (_check01.IsRoad && _check02.IsRoad)
        {
            possibleStates.Add(CarState.Forward);
        }
        if (_check05.IsRoad && !_check06.IsRoad)
        {
            possibleStates.Add(CarState.Right);
        }
        if (!_check01.IsRoad && !_check02.IsRoad && !_check05.IsRoad && !_check06.IsRoad && _check03.IsRoad && _check04.IsRoad)
        {
            possibleStates.Add(CarState.Left);
        }

        // Если есть несколько подходящих стейтов, выбираем случайный
        if (possibleStates.Count > 0)
        {
            _currentState = possibleStates[Random.Range(0, possibleStates.Count)];
        }
        else
        {
            _currentState = CarState.Forward; // По умолчанию
        }

        // Устанавливаем целевые углы с учётом текущего угла машины
        float currentAngle = RoundToNearest90(transform.eulerAngles.z);

        switch (_currentState)
        {
            case CarState.Forward:
                _targetRotation = currentAngle; // Движение вперёд без изменения угла
                break;
            case CarState.Right:
                _targetRotation = Mathf.Repeat(currentAngle - 90f, 360f); // Направо
                break;
            case CarState.Left:
                _targetRotation = Mathf.Repeat(currentAngle + 90f, 360f); // Налево
                break;
        }
    }

    private float RoundToNearest90(float angle)
    {
        return Mathf.Round(angle / 90f) * 90f;
    }


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

        // Текущий угол машины
        float currentAngle = transform.eulerAngles.z;

        // Draw state name, current angle, and target angle in the center of the object
#if UNITY_EDITOR
        GUIStyle style = new GUIStyle
        {
            fontSize = 16,
            normal = { textColor = Color.white },
            alignment = TextAnchor.MiddleCenter
        };

        string label = $"State: {_currentState}\n" +
                       $"Current Angle: {currentAngle:F1}°\n" +
                       $"Target: {_targetRotation:F1}°";

        UnityEditor.Handles.Label(transform.position, label, style);
#endif
    }
}