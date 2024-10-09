using UnityEngine;

public class RouteLineVisualizer : MonoBehaviour
{
    [SerializeField] private BusRoute _busRoute; // Reference to the existing BusRoute script
    [SerializeField] private Camera _mainCamera; // Camera to define screen boundaries
    [SerializeField] private GameObject _spritePrefab; // Prefab for displaying at the intersection point
    private GameObject _intersectionObject; // Instance of the prefab at the screen edge intersection

    private void Start()
    {
        // Instantiate the prefab for displaying the sprite
        _intersectionObject = Instantiate(_spritePrefab);
        _intersectionObject.SetActive(false); // Hide the sprite initially
    }

    private void Update()
    {
        DrawLineAndCheckIntersection();
    }

    private void DrawLineAndCheckIntersection()
    {
        if (_busRoute == null || _busRoute.GetCurrentRoutePoint() == null)
            return;

        Vector3 busPosition = transform.position;
        Vector3 pointPosition = _busRoute.GetCurrentRoutePoint().transform.position;

        // Visualize the line
        Debug.DrawLine(busPosition, pointPosition, Color.red);

        // Get screen boundaries
        Vector3 screenMin = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        Vector3 screenMax = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));

        // Check for intersection with screen boundaries
        Vector3 intersectionPoint;
        if (CheckLineScreenIntersection(busPosition, pointPosition, screenMin, screenMax, out intersectionPoint))
        {
            _intersectionObject.SetActive(true);
            _intersectionObject.transform.position = intersectionPoint;
        }
        else
        {
            _intersectionObject.SetActive(false); // Hide the sprite if no intersection
        }
    }

    private bool CheckLineScreenIntersection(Vector3 start, Vector3 end, Vector3 screenMin, Vector3 screenMax, out Vector3 intersection)
    {
        // Initialize the intersection point
        intersection = Vector3.zero;

        // Check intersections with screen boundaries (left, bottom, right, and top edges)
        if (LineIntersect(start, end, screenMin, new Vector3(screenMin.x, screenMax.y, start.z), out intersection)) return true; // Left edge
        if (LineIntersect(start, end, screenMin, new Vector3(screenMax.x, screenMin.y, start.z), out intersection)) return true; // Bottom edge
        if (LineIntersect(start, end, new Vector3(screenMax.x, screenMin.y, start.z), screenMax, out intersection)) return true; // Right edge
        if (LineIntersect(start, end, new Vector3(screenMin.x, screenMax.y, start.z), screenMax, out intersection)) return true; // Top edge

        return false; // No intersection
    }

    private bool LineIntersect(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, out Vector3 intersection)
    {
        // Calculate the intersection point of the lines
        intersection = Vector3.zero;

        float a1 = p2.y - p1.y;
        float b1 = p1.x - p2.x;
        float c1 = a1 * p1.x + b1 * p1.y;

        float a2 = p4.y - p3.y;
        float b2 = p3.x - p4.x;
        float c2 = a2 * p3.x + b2 * p3.y;

        float delta = a1 * b2 - a2 * b1;

        if (delta == 0)
        {
            return false; // Lines are parallel
        }

        intersection = new Vector3(
            (b2 * c1 - b1 * c2) / delta,
            (a1 * c2 - a2 * c1) / delta,
            0
        );

        // Check if the intersection point is within the line segments
        return IsBetween(p1, p2, intersection) && IsBetween(p3, p4, intersection);
    }

    private bool IsBetween(Vector3 a, Vector3 b, Vector3 c)
    {
        return Mathf.Min(a.x, b.x) <= c.x && c.x <= Mathf.Max(a.x, b.x) &&
               Mathf.Min(a.y, b.y) <= c.y && c.y <= Mathf.Max(a.y, b.y);
    }
}