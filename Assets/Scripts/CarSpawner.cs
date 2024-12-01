using UnityEngine;
using UnityEngine.Tilemaps;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap _roadTilemap; // Ссылка на Tilemap
    [SerializeField] private GameObject _carPrefab; // Префаб автомобиля

    private Transform[] _spawnPoints; // Список точек спавна

    private void Awake()
    {
        // Сбор дочерних объектов как точек спавна
        CollectSpawnPoints();
        SpawnCars();
    }

    private void CollectSpawnPoints()
    {
        // Получаем все дочерние объекты
        int childCount = transform.childCount;
        _spawnPoints = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            _spawnPoints[i] = transform.GetChild(i);
        }
    }

    private void SpawnCars()
    {
        if (_carPrefab == null || _roadTilemap == null)
        {
            Debug.LogError("Не все поля заданы в CarSpawner!");
            return;
        }

        if (_spawnPoints.Length == 0)
        {
            Debug.LogWarning("Нет точек спавна! Добавьте дочерние объекты.");
            return;
        }

        foreach (Transform spawnPoint in _spawnPoints)
        {
            SpawnCar(spawnPoint);
        }
    }

    private void SpawnCar(Transform spawnPoint)
    {
        // Спавн объекта в точке спавна с её позицией и поворотом
        GameObject spawnedCar = Instantiate(_carPrefab, spawnPoint.position, spawnPoint.rotation);

        // Передача ссылки на Tilemap в TilemapProvider
        TilemapProvider provider = spawnedCar.GetComponent<TilemapProvider>();
        if (provider != null)
        {
            provider.SetRoadTilemap(_roadTilemap); // Устанавливаем Tilemap
        }
        else
        {
            Debug.LogError("У префаба отсутствует компонент TilemapProvider!");
        }
    }
}