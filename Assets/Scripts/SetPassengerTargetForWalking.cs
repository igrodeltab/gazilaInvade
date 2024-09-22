using UnityEngine;
using UnityEngine.Tilemaps;

public class SetPassengerTargetForWalking : MonoBehaviour
{
    [TagSelector] [SerializeField] private string walkableTilemapTag = "WalkabilityTilemap"; // Тег для поиска Tilemap
    [SerializeField] private float minTargetRadius = 4f; // Минимальный радиус для поиска цели
    [SerializeField] private float maxTargetRadius = 6f; // Максимальный радиус для поиска цели
    private Tilemap walkableTilemap; // Ссылка на Tilemap, найденный по тегу
    private MovementToTargetByAxis movementComponent; // Компонент движения пассажира

    private void Start()
    {
        // Ищем компонент движения у пассажира
        movementComponent = GetComponent<MovementToTargetByAxis>();

        if (movementComponent == null)
        {
            throw new MissingComponentException("MovementToTargetByAxis component is missing on the passenger.");
        }

        // Ищем Tilemap с тегом, заданным в инспекторе
        GameObject tilemapObject = GameObject.FindWithTag(walkableTilemapTag);
        if (tilemapObject != null)
        {
            walkableTilemap = tilemapObject.GetComponent<Tilemap>();
            Debug.Log("Tilemap найден: " + walkableTilemap.name);
        }

        if (walkableTilemap == null)
        {
            Debug.LogError("Tilemap с заданным тегом не найден или не содержит компонента Tilemap.");
        }
    }

    private void Update()
    {
        // Если у пассажира не задана цель, задаем новую случайную цель
        if (!movementComponent.HasTarget)
        {
            AssignRandomTileTarget();
        }
    }

    // Метод для назначения случайной цели на доступном тайле в радиусе
    private void AssignRandomTileTarget()
    {
        Vector3Int randomTilePosition = GetRandomWalkableTileWithinRadius();
        if (randomTilePosition != Vector3Int.zero) // Если найден подходящий тайл
        {
            Vector3 targetPosition = walkableTilemap.CellToWorld(randomTilePosition) + walkableTilemap.cellSize / 2; // Центрируем цель
            movementComponent.SetTarget(targetPosition); // Назначаем цель пассажиру
            Debug.Log("Назначена новая цель для пассажира: " + targetPosition);
        }
        else
        {
            Debug.LogError("Не удалось найти подходящий тайл для цели.");
        }
    }

    // Метод для поиска случайного доступного тайла в радиусе
    private Vector3Int GetRandomWalkableTileWithinRadius()
    {
        Vector3Int passengerPosition = walkableTilemap.WorldToCell(transform.position);
        Vector3Int randomTilePosition = Vector3Int.zero;

        // Пытаемся найти случайный доступный тайл
        for (int i = 0; i < 100; i++) // Максимум 100 попыток найти подходящий тайл
        {
            float randomRadius = Random.Range(minTargetRadius, maxTargetRadius);
            Vector3 randomDirection = Random.insideUnitCircle * randomRadius;
            Vector3Int targetTilePosition = walkableTilemap.WorldToCell(transform.position + new Vector3(randomDirection.x, randomDirection.y, 0));

            // Проверяем, является ли тайл подходящим для движения
            if (IsWalkableTile(targetTilePosition))
            {
                randomTilePosition = targetTilePosition;
                break;
            }
        }

        return randomTilePosition;
    }

    // Проверка, является ли тайл доступным для движения
    private bool IsWalkableTile(Vector3Int tilePosition)
    {
        TileBase tile = walkableTilemap.GetTile(tilePosition);
        return tile != null; // Если тайл существует
    }
}