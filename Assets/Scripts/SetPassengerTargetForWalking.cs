using UnityEngine;
using UnityEngine.Tilemaps;

public class SetPassengerTargetForWalking : MonoBehaviour
{
    [TagSelector] [SerializeField] private string walkableTilemapTag = "WalkabilityTilemap"; // Tag to search for Tilemap
    [SerializeField] private float minTargetRadius = 4f; // Minimum radius for target search
    [SerializeField] private float maxTargetRadius = 6f; // Maximum radius for target search
    private Tilemap walkableTilemap; // Reference to the Tilemap found by the tag
    private MovementToTargetByAxis movementComponent; // Passenger's movement component
    private Vector3 _currentTargetPosition; // Passenger's current target position
    private Vector3 _lastPassengerPosition; // Позиция пассажира при выборе цели

    private void Start()
    {
        // Find the movement component on the passenger
        movementComponent = GetComponent<MovementToTargetByAxis>();

        if (movementComponent == null)
        {
            throw new MissingComponentException("MovementToTargetByAxis component is missing on the passenger.");
        }

        // Find the Tilemap with the specified tag
        GameObject tilemapObject = GameObject.FindWithTag(walkableTilemapTag);
        if (tilemapObject != null)
        {
            walkableTilemap = tilemapObject.GetComponent<Tilemap>();
            Debug.Log("Tilemap found: " + walkableTilemap.name);
        }

        if (walkableTilemap == null)
        {
            Debug.LogError("Tilemap with the specified tag not found or does not contain a Tilemap component.");
        }
    }

    private void Update()
    {
        // If the passenger doesn't have a target, assign a new random target
        if (!movementComponent.HasTarget)
        {
            AssignRandomTileTarget();
        }
    }

    private void AssignRandomTileTarget()
    {
        _lastPassengerPosition = transform.position; // Сохраняем текущую позицию пассажира

        Vector3Int randomTilePosition = GetRandomWalkableTileWithinRadius();

        if (randomTilePosition != Vector3Int.zero) // Если найден подходящий тайл
        {
            Vector3 finalTarget = walkableTilemap.CellToWorld(randomTilePosition) + walkableTilemap.cellSize / 2;

            Vector3 intermediateTarget = Vector3.zero;
            bool isIntermediateValid = false;

            for (int attempts = 0; attempts < 10; attempts++) // Максимум 10 попыток выбрать промежуточную цель
            {
                if (Random.value > 0.5f)
                {
                    intermediateTarget = new Vector3(finalTarget.x, transform.position.y, 0);
                }
                else
                {
                    intermediateTarget = new Vector3(transform.position.x, finalTarget.y, 0);
                }

                Vector3Int intermediateTilePosition = walkableTilemap.WorldToCell(intermediateTarget);

                if (IsWalkableTile(intermediateTilePosition) &&
                    intermediateTarget != finalTarget &&
                    IsPathValid(transform.position, intermediateTarget) &&
                    IsPathValid(intermediateTarget, finalTarget))
                {
                    isIntermediateValid = true;
                    break;
                }
            }

            if (isIntermediateValid)
            {
                movementComponent.SetTargets(intermediateTarget, finalTarget);
                Debug.Log($"New intermediate target: {intermediateTarget}, final target: {finalTarget}");
            }
            else
            {
                Debug.LogError("Failed to assign a valid intermediate target.");
            }
        }
        else
        {
            Debug.LogError("Failed to find a suitable tile for the target.");
        }
    }

    // Проверка, что путь от start до end проходит только по тайлам
    private bool IsPathValid(Vector3 start, Vector3 end)
    {
        Vector3 direction = (end - start).normalized;
        float distance = Vector3.Distance(start, end);

        // Проверяем каждую точку вдоль пути
        for (float i = 0; i <= distance; i += walkableTilemap.cellSize.x / 2) // Шаг равен половине размера тайла
        {
            Vector3 point = start + direction * i;
            Vector3Int tilePosition = walkableTilemap.WorldToCell(point);

            if (!IsWalkableTile(tilePosition))
            {
                Debug.Log($"Path invalid at point {point}, not on a valid tile.");
                return false;
            }
        }

        return true;
    }

    // Method to search for a random walkable tile within the radius
    private Vector3Int GetRandomWalkableTileWithinRadius()
    {
        Vector3Int passengerPosition = walkableTilemap.WorldToCell(transform.position);
        Vector3Int randomTilePosition = Vector3Int.zero;

        for (int i = 0; i < 100; i++) // Максимум 100 попыток найти подходящий тайл
        {
            float randomRadius = Random.Range(minTargetRadius, maxTargetRadius);
            Vector3 randomDirection = Random.insideUnitCircle.normalized * randomRadius; // Нормализуем вектор, чтобы учитывать только расстояние
            Vector3 targetWorldPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0);

            float distanceToTarget = Vector3.Distance(transform.position, targetWorldPosition);

            // Проверяем, что точка находится в нужном диапазоне
            if (distanceToTarget >= minTargetRadius && distanceToTarget <= maxTargetRadius)
            {
                randomTilePosition = walkableTilemap.WorldToCell(targetWorldPosition);

                // Проверяем, что тайл валиден
                if (IsWalkableTile(randomTilePosition))
                {
                    return randomTilePosition;
                }
            }
        }

        return Vector3Int.zero; // Если ничего не найдено, возвращаем пустую точку
    }

    // Check if the tile is walkable
    private bool IsWalkableTile(Vector3Int tilePosition)
    {
        TileBase tile = walkableTilemap.GetTile(tilePosition);
        return tile != null; // If the tile exists
    }

    private void OnDrawGizmos()
    {
        if (_lastPassengerPosition != Vector3.zero) // Если позиция пассажира сохранена
        {
            // Отрисовка радиусов поиска вокруг позиции пассажира при выборе цели
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_lastPassengerPosition, minTargetRadius); // Минимальный радиус

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_lastPassengerPosition, maxTargetRadius); // Максимальный радиус
        }

        // Отрисовка текущей цели, если есть
        if (movementComponent != null && movementComponent.HasTarget)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_currentTargetPosition, 0.3f);
        }
    }
}