using UnityEngine;
using System.Collections.Generic;

public class BusRoute : MonoBehaviour
{
    [SerializeField] private List<GameObject> _routePoints; // List of route points
    private int _currentPointIndex = 0; // Current index in the list of route points

    private void Start()
    {
        InitializeRoute(); // Initialize the route
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _routePoints[_currentPointIndex])
        {
            _routePoints[_currentPointIndex].SetActive(false); // Deactivate the current route point

            _currentPointIndex++; // Move to the next point
            if (_currentPointIndex >= _routePoints.Count)
            {
                _currentPointIndex = 0; // Restart the route or stop
            }

            if (_currentPointIndex < _routePoints.Count)
            {
                _routePoints[_currentPointIndex].SetActive(true); // Activate the next route point
            }
        }
    }

    private void InitializeRoute()
    {
        foreach (var point in _routePoints)
        {
            point.SetActive(false); // Deactivate all route points except the first one
        }

        if (_routePoints.Count > 0)
        {
            _routePoints[0].SetActive(true); // Activate the first route point
        }
    }
}