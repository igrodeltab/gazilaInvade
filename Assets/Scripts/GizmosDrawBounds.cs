using UnityEngine;

[ExecuteAlways]
public class GizmosDrawBounds : MonoBehaviour
{
    [SerializeField] private Color _fillColor = new Color(0f, 1f, 0f, 0.5f); // Цвет заливки
    [SerializeField] private Color _borderColor = Color.green; // Цвет границы

    private void OnDrawGizmos()
    {
        // Получаем размеры объекта через его локальный Scale
        Vector3 size = transform.localScale;

        // Устанавливаем матрицу для учета поворота и масштаба
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        // Отрисовка заливки
        Gizmos.color = _fillColor;
        Gizmos.DrawCube(Vector3.zero, size);

        // Отрисовка рамки
        Gizmos.color = _borderColor;
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}