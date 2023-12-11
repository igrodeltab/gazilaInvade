using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }

    private void Update()
    {
        VerticalInput = 0;
        HorizontalInput = 0;

        if (Input.GetKeyDown(KeyCode.W))
        {
            VerticalInput = 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            VerticalInput = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            HorizontalInput = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            HorizontalInput = -1;
        }
    }
}