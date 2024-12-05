using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkabilityTilemapToggle : MonoBehaviour
{
    private TilemapRenderer _walkabilityTilemapRenderer; // Ссылка на TilemapRenderer, который будет включаться/выключаться
    [SerializeField] private KeyCode _toggleKey = KeyCode.I; // Горячая клавиша для включения/отключения, по умолчанию I
    [SerializeField] private bool _defaultState = true; // Значение по умолчанию: включено (true) или выключено (false)

    private void Awake()
    {
        // Автоматически получаем компонент TilemapRenderer
        _walkabilityTilemapRenderer = GetComponent<TilemapRenderer>();

        if (_walkabilityTilemapRenderer == null)
        {
            Debug.LogError("TilemapRenderer component is missing on this GameObject.");
            return;
        }

        // Устанавливаем начальное состояние рендерера
        _walkabilityTilemapRenderer.enabled = _defaultState;
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