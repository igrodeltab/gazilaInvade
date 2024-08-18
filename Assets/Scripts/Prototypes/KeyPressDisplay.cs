using UnityEngine;
using TMPro;

public class KeyPressDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI aText; // Ссылка на TextMeshPro для A
    [SerializeField] private TextMeshProUGUI dText; // Ссылка на TextMeshPro для D

    private void Update()
    {
        // Обработка нажатия клавиши A
        if (Input.GetKey(KeyCode.A))
        {
            aText.enabled = true;
        }
        else
        {
            aText.enabled = false; // Скрыть текст, если клавиша A не нажата
        }

        // Обработка нажатия клавиши D
        if (Input.GetKey(KeyCode.D))
        {
            dText.enabled = true;
        }
        else
        {
            dText.enabled = false; // Скрыть текст, если клавиша D не нажата
        }
    }
}