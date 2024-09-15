using UnityEngine;
using TMPro;

public class PauseGameOnStart : MonoBehaviour
{
    public KeyCode unpauseKey = KeyCode.F; // Горячая клавиша для отжатия паузы
    public TextMeshProUGUI pauseText; // Ссылка на TextMeshPro для отображения текста

    void Start()
    {
        PauseGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(unpauseKey))
        {
            UnpauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Ставим игру на паузу
        if (pauseText != null)
        {
            pauseText.gameObject.SetActive(true); // Показываем текст паузы
        }
    }

    void UnpauseGame()
    {
        Time.timeScale = 1f; // Снимаем паузу
        if (pauseText != null)
        {
            pauseText.gameObject.SetActive(false); // Отключаем текст паузы
        }
    }
}