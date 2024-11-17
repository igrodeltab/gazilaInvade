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
        Vector3Int randomTilePosition = GetRandomWalkableTileWithinRadius();

        if (randomTilePosition != Vector3Int.zero) // Если найден подходящий тайл
        {
            // Определяем конечную цель
            Vector3 finalTarget = walkableTilemap.CellToWorld(randomTilePosition) + walkableTilemap.cellSize / 2;

            // Пробуем выбрать промежуточную цель
            Vector3 intermediateTarget = Vector3.zero; // Задаём значение по умолчанию
            bool isIntermediateValid = false;

            for (int attempts = 0; attempts < 10; attempts++) // Максимум 10 попыток выбрать промежуточную цель
            {
                if (Random.value > 0.5f) // Сначала двигаемся по X, затем по Y
                {
                    intermediateTarget = new Vector3(finalTarget.x, transform.position.y, 0);
                }
                else // Сначала двигаемся по Y, затем по X
                {
                    intermediateTarget = new Vector3(transform.position.x, finalTarget.y, 0);
                }

                // Проверяем, находится ли промежуточная цель на тайле
                Vector3Int intermediateTilePosition = walkableTilemap.WorldToCell(intermediateTarget);

                if (IsWalkableTile(intermediateTilePosition) && intermediateTarget != finalTarget)
                {
                    isIntermediateValid = true;
                    break;
                }
            }

            // Если промежуточная цель найдена, назначаем обе цели
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

    // Method to search for a random walkable tile within the radius
    private Vector3Int GetRandomWalkableTileWithinRadius()
    {
        Vector3Int passengerPosition = walkableTilemap.WorldToCell(transform.position);
        Vector3Int randomTilePosition = Vector3Int.zero;

        // Try to find a random walkable tile
        for (int i = 0; i < 100; i++) // Maximum of 100 attempts to find a suitable tile
        {
            float randomRadius = Random.Range(minTargetRadius, maxTargetRadius);
            Vector3 randomDirection = Random.insideUnitCircle * randomRadius;
            Vector3Int targetTilePosition = walkableTilemap.WorldToCell(transform.position + new Vector3(randomDirection.x, randomDirection.y, 0));

            // Check if the tile is walkable
            if (IsWalkableTile(targetTilePosition))
            {
                randomTilePosition = targetTilePosition;
                break;
            }
        }

        return randomTilePosition;
    }

    // Check if the tile is walkable
    private bool IsWalkableTile(Vector3Int tilePosition)
    {
        TileBase tile = walkableTilemap.GetTile(tilePosition);
        return tile != null; // If the tile exists
    }

    private void OnDrawGizmos()
    {
        // Отрисовка радиусов поиска целей
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minTargetRadius); // Минимальный радиус
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxTargetRadius); // Максимальный радиус
    }
}