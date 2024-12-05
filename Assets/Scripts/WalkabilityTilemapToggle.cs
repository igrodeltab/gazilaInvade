using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkabilityTilemapToggle : MonoBehaviour
{
    private TilemapRenderer _walkabilityTilemapRenderer; // Reference to the TilemapRenderer to enable/disable
    [SerializeField] private KeyCode _toggleKey = KeyCode.I; // Hotkey for toggling, default is 'I'
    [SerializeField] private bool _defaultState = true; // Default state: enabled (true) or disabled (false)

    private void Awake()
    {
        // Automatically retrieve the TilemapRenderer component
        _walkabilityTilemapRenderer = GetComponent<TilemapRenderer>();

        if (_walkabilityTilemapRenderer == null)
        {
            Debug.LogError("TilemapRenderer component is missing on this GameObject.");
            return;
        }

        // Set the initial state of the renderer
        _walkabilityTilemapRenderer.enabled = _defaultState;
    }

    private void Update()
    {
        // Check if the hotkey is pressed
        if (Input.GetKeyDown(_toggleKey))
        {
            // Toggle the renderer's enabled state
            _walkabilityTilemapRenderer.enabled = !_walkabilityTilemapRenderer.enabled;
        }
    }
}