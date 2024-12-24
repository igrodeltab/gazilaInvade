using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private Vector2 _zoneSize = new Vector2(5f, 3f);

    public bool IsObjectDetected { get; private set; }
    [SerializeField] private bool _isDetected;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _zoneSize);
    }

    private void Update()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapBoxAll(transform.position, _zoneSize, 0f);
        IsObjectDetected = detectedObjects.Length > 0;
        _isDetected = IsObjectDetected;
    }
}