using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCenterChecker : MonoBehaviour
{
    [SerializeField] private Tilemap _roadTilemap; // Reference to the road tilemap

    public bool IsRoad { get; private set; } // True if at least one road tile is found in the area

    private const float FixedRectangleWidth = 1.0f; // Fixed width of the rectangle
    private const float FixedRectangleHeight = 1.0f; // Fixed height of the rectangle

    private void Update()
    {
        IsRoad = false;

        // Get the center point of the rectangle from the transform position
        Vector3 centerPoint = transform.position;

        // Iterate through all tile positions within tilemap bounds
        foreach (var position in _roadTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, position.z);
            Vector3 tileCenter = _roadTilemap.GetCellCenterWorld(tilePosition);

            if (_roadTilemap.HasTile(tilePosition))
            {
                if (IsTileInRectangle(tileCenter, centerPoint))
                {
                    IsRoad = true; // At least one road tile is detected
                    return; // Exit early, since we only need to know if there is a road
                }
            }
        }
    }

    private bool IsTileInRectangle(Vector3 tileCenter, Vector3 centerPoint)
    {
        // Calculate local position relative to the transform position
        Vector3 localPosition = transform.InverseTransformPoint(tileCenter);

        // Check if within width and height bounds
        return Mathf.Abs(localPosition.x) <= FixedRectangleWidth / 2 &&
               Mathf.Abs(localPosition.y) <= FixedRectangleHeight / 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix; // Apply local-to-world transformation

        // Define the rectangle's center and dimensions in local space
        Vector3 center = Vector3.zero; // Local center is at the origin
        Vector3 size = new Vector3(FixedRectangleWidth, FixedRectangleHeight, 0);

        // Set the Gizmos color based on IsRoad
        Gizmos.color = IsRoad ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);

        // Draw a filled rectangle in local space
        Gizmos.DrawCube(center, size);

        // Draw the outline of the rectangle in local space
        Gizmos.color = IsRoad ? Color.green : Color.red;
        Gizmos.DrawWireCube(center, size);

        Gizmos.matrix = Matrix4x4.identity; // Reset Gizmos matrix to default
    }
}