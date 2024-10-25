using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileTriggerChecker : MonoBehaviour
{
    [SerializeField] private Tilemap _roadTilemap; // Reference to the road tilemap
    [SerializeField] private Transform _frontCheckPoint; // Point in front of the car
    [SerializeField] private float _rectangleWidth = 2.0f; // Width of the rectangle
    [SerializeField] private float _rectangleHeight = 1.0f; // Height of the rectangle

    public int TileCountInArea { get; private set; } // Public count of tiles in the area

    private List<Vector3> _tileCenters = new List<Vector3>(); // List of tile centers in the area

    private void Update()
    {
        // Clear previous tile data
        _tileCenters.Clear();
        TileCountInArea = 0;

        // Get the center point of the rectangle
        Vector3 centerPoint = _frontCheckPoint.position;

        // Iterate through all tile positions within tilemap bounds
        foreach (var position in _roadTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, position.z);
            Vector3 tileCenter = _roadTilemap.GetCellCenterWorld(tilePosition);

            // Check if the tile center is within the rectangle bounds
            if (_roadTilemap.HasTile(tilePosition) && IsTileInRectangle(tileCenter, centerPoint))
            {
                _tileCenters.Add(tileCenter);
                TileCountInArea++;
            }
        }
    }

    private bool IsTileInRectangle(Vector3 tileCenter, Vector3 centerPoint)
    {
        // Calculate local position relative to front checkpoint
        Vector3 localPosition = _frontCheckPoint.InverseTransformPoint(tileCenter);

        // Check if within width and height bounds
        return Mathf.Abs(localPosition.x) <= _rectangleWidth / 2 &&
               Mathf.Abs(localPosition.y) <= _rectangleHeight / 2;
    }

    private void OnDrawGizmos()
    {
        if (_frontCheckPoint != null)
        {
            Gizmos.color = Color.red;

            // Draw rectangle bounds
            Vector3 topLeft = _frontCheckPoint.position + _frontCheckPoint.right * -_rectangleWidth / 2 + _frontCheckPoint.up * _rectangleHeight / 2;
            Vector3 topRight = _frontCheckPoint.position + _frontCheckPoint.right * _rectangleWidth / 2 + _frontCheckPoint.up * _rectangleHeight / 2;
            Vector3 bottomLeft = _frontCheckPoint.position + _frontCheckPoint.right * -_rectangleWidth / 2 - _frontCheckPoint.up * _rectangleHeight / 2;
            Vector3 bottomRight = _frontCheckPoint.position + _frontCheckPoint.right * _rectangleWidth / 2 - _frontCheckPoint.up * _rectangleHeight / 2;

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);

            Gizmos.color = Color.green;
            // Draw spheres at the center of each tile inside the rectangle
            foreach (var tileCenter in _tileCenters)
            {
                Gizmos.DrawSphere(tileCenter, 0.1f);
            }
        }
    }
}