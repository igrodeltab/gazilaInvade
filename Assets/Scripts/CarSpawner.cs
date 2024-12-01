using UnityEngine;
using UnityEngine.Tilemaps;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap _roadTilemap; // Ссылка на Tilemap
    [SerializeField] private GameObject _carPrefab; // Префаб автомобиля
    [SerializeField] private Transform _spawnPoint; // Точка спавна с позицией и направлением

    private void Awake()
    {
        SpawnCar();
    }


    public void SpawnCar()
    {
        if (_carPrefab == null || _roadTilemap == null || _spawnPoint == null)
        {
            Debug.LogError("Не все поля заданы в CarSpawner!");
            return;
        }

        // Спавн объекта в точке спавна с поворотом
        GameObject spawnedCar = Instantiate(_carPrefab, _spawnPoint.position, _spawnPoint.rotation);

        // Передача ссылки на Tilemap в TilemapProvider
        TilemapProvider provider = spawnedCar.GetComponent<TilemapProvider>();
        if (provider != null)
        {
            // Устанавливаем Tilemap из спавнера
            var providerField = typeof(TilemapProvider)
                                .GetField("_roadTilemap",
                                System.Reflection.BindingFlags.NonPublic
                                | System.Reflection.BindingFlags.Instance);

            providerField?.SetValue(provider, _roadTilemap);
        }
        else
        {
            Debug.LogError("У префаба отсутствует компонент TilemapProvider!");
        }
    }
}