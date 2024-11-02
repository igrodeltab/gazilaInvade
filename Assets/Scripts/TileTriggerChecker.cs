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

    private List<Vector3> _tileCenters = new List<Vector3>(); // List of quarter points in the area

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

            if (_roadTilemap.HasTile(tilePosition))
            {
                // Calculate the positions of the 4 quarter points within the tile
                float halfTileSizeX = _roadTilemap.cellSize.x / 2;
                float halfTileSizeY = _roadTilemap.cellSize.y / 2;

                // Calculate only the 4 quarter points
                Vector3[] quarterPoints = new Vector3[]
                {
                    tileCenter + new Vector3(-halfTileSizeX / 2, -halfTileSizeY / 2, 0),
                    tileCenter + new Vector3(halfTileSizeX / 2, -halfTileSizeY / 2, 0),
                    tileCenter + new Vector3(-halfTileSizeX / 2, halfTileSizeY / 2, 0),
                    tileCenter + new Vector3(halfTileSizeX / 2, halfTileSizeY / 2, 0)
                };

                // Check each quarter point and count it if within bounds
                foreach (var point in quarterPoints)
                {
                    if (IsTileInRectangle(point, centerPoint))
                    {
                        _tileCenters.Add(point);
                        TileCountInArea++;
                    }
                }
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

            // Draw quarter points inside the tiles
            Gizmos.color = Color.green;
            foreach (var tileCenter in _tileCenters)
            {
                Gizmos.DrawSphere(tileCenter, 0.05f);
            }
        }
    }
}