using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapProvider : MonoBehaviour
{
    [SerializeField] private Tilemap _roadTilemap;

    public Tilemap RoadTilemap => _roadTilemap;
}