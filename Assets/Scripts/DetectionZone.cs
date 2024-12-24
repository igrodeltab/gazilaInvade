using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private Vector2 _zoneSize = new Vector2(5f, 3f);

    public bool IsObjectDetected { get; private set; }

    private void OnDrawGizmos()
    {
        // Установка цвета в зависимости от детекции
        Gizmos.color = IsObjectDetected ? new Color(1f, 0f, 0f, 0.5f) : new Color(0f, 1f, 0f, 0.5f);

        // Отрисовка заполненной области
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, _zoneSize);

        // Отрисовка рамки
        Gizmos.color = IsObjectDetected ? Color.red : Color.green;
        Gizmos.DrawWireCube(Vector3.zero, _zoneSize);
    }

    private void Update()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapBoxAll(transform.position, _zoneSize, transform.eulerAngles.z);
        IsObjectDetected = detectedObjects.Length > 0;
    }
}