using UnityEngine;
using UnityEngine.UI;

public class RouteLineVisualizer : MonoBehaviour
{
    [SerializeField] private BusRoute _busRoute; // Reference to the existing BusRoute script
    [SerializeField] private Camera _mainCamera; // Camera to define screen boundaries
    [SerializeField] private RectTransform _spriteRect; // UI element (Image) for displaying at the intersection point
    private Canvas _canvas; // Reference to the UI canvas

    private void Start()
    {
        // Find the Canvas in the scene
        _canvas = _spriteRect.GetComponentInParent<Canvas>();
        _spriteRect.gameObject.SetActive(false); // Hide the sprite initially
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

        // Convert the route point position to viewport coordinates (0 to 1)
        Vector3 viewportPoint = _mainCamera.WorldToViewportPoint(pointPosition);

        // If the point is within the screen bounds, hide the sprite
        if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
        {
            _spriteRect.gameObject.SetActive(false); // Hide sprite if the point is inside the screen
        }
        else
        {
            _spriteRect.gameObject.SetActive(true); // Show the sprite if the point is outside the screen

            // Clamp the viewport point to the edges (0 to 1)
            viewportPoint.x = Mathf.Clamp01(viewportPoint.x);
            viewportPoint.y = Mathf.Clamp01(viewportPoint.y);

            // Calculate the position on the screen (use the canvas size as reference)
            Vector2 canvasSize = _canvas.GetComponent<RectTransform>().sizeDelta;

            // Convert viewport coordinates to canvas coordinates
            Vector2 canvasPos = new Vector2(viewportPoint.x * canvasSize.x - canvasSize.x / 2,
                                            viewportPoint.y * canvasSize.y - canvasSize.y / 2);

            // Adjust the position so that the sprite does not overlap the screen edge
            Vector2 spriteSize = _spriteRect.sizeDelta;

            // Adjust based on position near the edges
            if (viewportPoint.x == 0) canvasPos.x += spriteSize.x / 2; // Left edge
            if (viewportPoint.x == 1) canvasPos.x -= spriteSize.x / 2; // Right edge
            if (viewportPoint.y == 0) canvasPos.y += spriteSize.y / 2; // Bottom edge
            if (viewportPoint.y == 1) canvasPos.y -= spriteSize.y / 2; // Top edge

            // Set the position of the UI element on the canvas
            _spriteRect.anchoredPosition = canvasPos;
        }
    }
}