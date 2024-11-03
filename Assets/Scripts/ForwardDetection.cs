using UnityEngine;

public class ForwardDetection : MonoBehaviour
{
    //public bool IsSomethingAhead { get; private set; } = false;
    public bool IsSomethingAhead = false;
    [SerializeField] private float detectionWidth = 5f;  // Width of the rectangular area
    [SerializeField] private float detectionHeight = 5f; // Height of the rectangular area
    [SerializeField] private LayerMask detectionLayer;   // Layer to filter detected objects

    private void Update()
    {
        // Define the size of the rectangular detection area
        Vector2 boxSize = new Vector2(detectionWidth, detectionHeight);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, detectionLayer);
        IsSomethingAhead = colliders.Length > 0;
    }

    private void OnDrawGizmos()
    {
        // Set the Gizmos color to magenta
        Gizmos.color = Color.magenta;
        // Draw the detection area as a wireframe rectangle
        Gizmos.DrawWireCube(transform.position, new Vector3(detectionWidth, detectionHeight, 0f));
    }
}