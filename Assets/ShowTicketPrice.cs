using UnityEngine;
using TMPro;

public class ShowTicketPrice : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // The prefab to spawn, should be a UI element
    [SerializeField] private Canvas canvas; // The canvas where the prefab will be spawned

    // Public method to spawn the prefab on the canvas at a specified position
    public void SpawnPrefabOnCanvas(Vector2 position, int textTicketPrice)
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas is not assigned.");
            return;
        }

        // Instantiate the prefab as a child of the canvas
        GameObject spawnedObject = Instantiate(prefab, canvas.transform, false);

        // Convert the world position to canvas space
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(position);
        Vector2 canvasPosition = new Vector2(
            (viewportPosition.x * canvas.GetComponent<RectTransform>().sizeDelta.x) - (canvas.GetComponent<RectTransform>().sizeDelta.x * 0.5f),
            (viewportPosition.y * canvas.GetComponent<RectTransform>().sizeDelta.y) - (canvas.GetComponent<RectTransform>().sizeDelta.y * 0.5f)
        );

        // Set the position of the prefab
        spawnedObject.GetComponent<RectTransform>().anchoredPosition = canvasPosition;

        // Find the TextMeshProUGUI component in the instantiated prefab
        TextMeshProUGUI tmp = spawnedObject.GetComponentInChildren<TextMeshProUGUI>();

        // If TextMeshProUGUI component is found, set the message
        if (tmp != null)
        {
            tmp.text = textTicketPrice.ToString();
        }
    }
}