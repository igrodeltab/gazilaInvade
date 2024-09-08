using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _targetPrefab; // Префаб цели для спавна
    [SerializeField] private MovementToTargetByAxisDraft _movingObject; // Ссылка на объект, который движется

    private GameObject _currentTargetObject;

    void Update()
    {
        // Проверяем нажатие мыши
        if (Input.GetMouseButtonDown(0))
        {
            // Преобразуем позицию клика в мировые координаты
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane; // Задаём расстояние от камеры до точки клика
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            targetPosition.z = 0; // Убираем глубину, если работаем в 2D

            // Удаляем предыдущий объект цели, если он существует
            if (_currentTargetObject != null)
            {
                Destroy(_currentTargetObject);
            }

            // Спавним новый объект цели на позиции клика
            _currentTargetObject = Instantiate(_targetPrefab, targetPosition, Quaternion.identity);
        }

        // Проверяем, совпадают ли координаты объекта и цели
        if (_currentTargetObject != null && _movingObject != null)
        {
            if (Mathf.Abs(_movingObject.transform.position.x - _currentTargetObject.transform.position.x) < 0.01f &&
                Mathf.Abs(_movingObject.transform.position.y - _currentTargetObject.transform.position.y) < 0.01f)
            {
                Destroy(_currentTargetObject); // Удаляем объект цели
            }
        }
    }
}