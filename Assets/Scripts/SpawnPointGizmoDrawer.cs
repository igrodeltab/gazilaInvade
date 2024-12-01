using UnityEngine;

public class SpawnPointGizmoDrawer : MonoBehaviour
{
    [SerializeField] private float _gizmoRadius = 0.5f; // Радиус сферы
    [SerializeField] private Color _gizmoColor = Color.yellow; // Цвет сферы
    [SerializeField] private float _directionLength = 2f; // Длина стрелки (ось Y)
    [SerializeField] private float _lineThickness = 0.1f; // Толщина линии стрелки
    [SerializeField] private float _arrowHeadSize = 0.3f; // Размер головки стрелки
    [SerializeField] private float _arrowHeadAngle = 45f; // Угол головки стрелки в градусах
    [SerializeField] private Color _lineColor = Color.blue; // Цвет линии стрелки

    private void OnDrawGizmos()
    {
        // Устанавливаем цвет для сферы
        Gizmos.color = _gizmoColor;

        // Рисуем сферу в позиции объекта
        Gizmos.DrawSphere(transform.position, _gizmoRadius);

        // Рисуем стрелку оси Y с толщиной
        DrawThickArrow(transform.position, transform.up * _directionLength, _lineColor);
    }

    private void DrawThickArrow(Vector3 start, Vector3 direction, Color color)
    {
        Gizmos.color = color;

        // Конечная точка стрелки
        Vector3 end = start + direction;

        // Вектор для создания "толщины"
        Vector3 perpendicular = Vector3.Cross(direction.normalized, Vector3.forward).normalized * (_lineThickness / 2);

        // Рисуем основную линию с толщиной
        Gizmos.DrawLine(start - perpendicular, end - perpendicular);
        Gizmos.DrawLine(start + perpendicular, end + perpendicular);
        Gizmos.DrawLine(start - perpendicular, start + perpendicular);
        Gizmos.DrawLine(end - perpendicular, end + perpendicular);

        // Рисуем головку стрелки
        DrawArrowHead(end, direction, _arrowHeadAngle, _arrowHeadSize, color);
    }

    private void DrawArrowHead(Vector3 tip, Vector3 direction, float angle, float size, Color color)
    {
        Gizmos.color = color;

        // Расчет точек головки стрелки на основе угла
        Vector3 baseLeft = tip + Quaternion.Euler(0, 0, angle) * -direction.normalized * size;
        Vector3 baseRight = tip + Quaternion.Euler(0, 0, -angle) * -direction.normalized * size;

        // Рисуем треугольник головки
        Gizmos.DrawLine(tip, baseLeft);
        Gizmos.DrawLine(tip, baseRight);
        Gizmos.DrawLine(baseLeft, baseRight);
    }
}