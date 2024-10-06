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

    // Method to assign a random target on a walkable tile within the radius
    private void AssignRandomTileTarget()
    {
        Vector3Int randomTilePosition = GetRandomWalkableTileWithinRadius();
        if (randomTilePosition != Vector3Int.zero) // If a valid tile is found
        {
            _currentTargetPosition = walkableTilemap.CellToWorld(randomTilePosition) + walkableTilemap.cellSize / 2; // Center the target
            movementComponent.SetTarget(_currentTargetPosition); // Set the target for the passenger
            Debug.Log("New target assigned for the passenger: " + _currentTargetPosition);
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
        if (movementComponent != null && movementComponent.HasTarget)
        {
            // Draw a sphere at the target position
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_currentTargetPosition, 0.3f);

            // Draw a line from the passenger's X coordinate to the target's X coordinate
            Vector3 passengerPosition = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(passengerPosition.x, passengerPosition.y, 0), new Vector3(_currentTargetPosition.x, passengerPosition.y, 0));

            // Draw a line from the passenger's Y coordinate to the target's Y coordinate
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(passengerPosition.x, passengerPosition.y, 0), new Vector3(passengerPosition.x, _currentTargetPosition.y, 0));
        }

        // Draw the min and max target search radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minTargetRadius); // Draw the minimum radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxTargetRadius); // Draw the maximum radius
    }
}