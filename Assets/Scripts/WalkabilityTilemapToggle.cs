using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkabilityTilemapToggle : MonoBehaviour
{
    private TilemapRenderer _walkabilityTilemapRenderer; // Ссылка на TilemapRenderer, который будет включаться/выключаться
    [SerializeField] private KeyCode _toggleKey = KeyCode.I; // Горячая клавиша для включения/отключения, по умолчанию I

    private void Awake()
    {
        // Автоматически получаем компонент TilemapRenderer
        _walkabilityTilemapRenderer = GetComponent<TilemapRenderer>();

        if (_walkabilityTilemapRenderer == null)
        {
            Debug.LogError("TilemapRenderer component is missing on this GameObject.");
            return;
        }

        // Отключаем рендерер при старте
        _walkabilityTilemapRenderer.enabled = false;
    }

    private void Update()
    {
        // Проверяем, нажата ли горячая клавиша
        if (Input.GetKeyDown(_toggleKey))
        {
            // Переключаем активность рендерера
            _walkabilityTilemapRenderer.enabled = !_walkabilityTilemapRenderer.enabled;
        }
    }
}