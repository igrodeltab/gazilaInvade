using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapProvider : MonoBehaviour
{
    [SerializeField] private Tilemap _roadTilemap;

    public Tilemap RoadTilemap => _roadTilemap;

    // Метод для установки Tilemap
    public void SetRoadTilemap(Tilemap tilemap)
    {
        _roadTilemap = tilemap;
    }
}