using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private Vector2 _zoneSize = new Vector2(5f, 3f);

    public bool IsObjectDetected { get; private set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _zoneSize);
    }

    private void Update()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapBoxAll(transform.position, _zoneSize, transform.eulerAngles.z);
        IsObjectDetected = detectedObjects.Length > 0;
    }
}