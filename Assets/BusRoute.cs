using UnityEngine;
using System.Collections.Generic;

public class BusRoute : MonoBehaviour
{
    [SerializeField] private List<GameObject> _routePoints; // Список точек маршрута
    private int _currentPointIndex = 0; // Текущий индекс в списке точек маршрута

    private void Start()
    {
        InitializeRoute(); // Инициализация маршрута
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _routePoints[_currentPointIndex])
        {
            _routePoints[_currentPointIndex].SetActive(false); // Деактивируем текущую точку маршрута

            _currentPointIndex++; // Переходим к следующей точке
            if (_currentPointIndex >= _routePoints.Count)
            {
                _currentPointIndex = 0; // Начать маршрут заново или остановиться
            }

            if (_currentPointIndex < _routePoints.Count)
            {
                _routePoints[_currentPointIndex].SetActive(true); // Активируем следующую точку маршрута
            }
        }
    }

    private void InitializeRoute()
    {
        foreach (var point in _routePoints)
        {
            point.SetActive(false); // Деактивируем все точки маршрута, кроме первой
        }

        if (_routePoints.Count > 0)
        {
            _routePoints[0].SetActive(true); // Активируем первую точку маршрута
        }
    }
}