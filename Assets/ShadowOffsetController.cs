using UnityEngine;

public class ShadowOffsetController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform childObject;

    void Start()
    {
        if (childObject == null)
        {
            Debug.LogError("Child object is not assigned.");
            return;
        }

        // Set initial offset
        UpdateChildPosition();
    }

    void Update()
    {
        // Constantly update child object's position in global coordinates
        UpdateChildPosition();
    }

    private void UpdateChildPosition()
    {
        childObject.position = transform.position + offset;
    }
}