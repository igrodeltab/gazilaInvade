using UnityEngine;

public class CameraFollowBus : MonoBehaviour
{
    [SerializeField] private Transform _busTransform; // Ссылка на трансформ автобуса

    private void Update()
    {
        // Обновляем позицию камеры, следуя за автобусом, но сохраняя исходную Z-координату камеры
        if (_busTransform != null)
        {
            transform.position = new Vector3(_busTransform.position.x, _busTransform.position.y, transform.position.z);
        }
    }
}