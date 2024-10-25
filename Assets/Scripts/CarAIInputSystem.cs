using UnityEngine;

public class CarAIInputSystem : MonoBehaviour
{
    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }

    [SerializeField] private TileTriggerChecker _tileTriggerChecker; // Reference to TileTriggerChecker

    private void Update()
    {
        // Reset VerticalInput and HorizontalInput
        VerticalInput = 0;
        HorizontalInput = 0;

        // Check if there are 2 or more tiles in the area
        if (_tileTriggerChecker.TileCountInArea >= 2)
        {
            VerticalInput = 1; // Move forward
        }
        else
        {
            VerticalInput = -1; // Stop moving forward
        }

        // Additional logic for HorizontalInput if needed
        // HorizontalInput can be set based on other conditions if required
    }
}