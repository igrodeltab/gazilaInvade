using System.IO;
using UnityEngine;

public class ConsoleToText : MonoBehaviour
{
    private StreamWriter logWriter;

    void OnEnable()
    {
        // Установить путь к файлу для сохранения логов
        string logPath = Application.dataPath + "/console_output.txt";

        // Создаем файловый поток и настраиваем его на добавление в файл, а не на перезапись файла
        logWriter = new StreamWriter(logPath, true);

        // Перенаправление вывода консоли в файл
        Application.logMessageReceived += LogToFile;
    }

    void OnDisable()
    {
        // Отмена перенаправления вывода консоли
        Application.logMessageReceived -= LogToFile;

        // Закрыть StreamWriter
        logWriter.Close();
    }

    void LogToFile(string logString, string stackTrace, LogType type)
    {
        // Формируем строку с информацией о логе
        string logEntry = string.Format("[{0}] {1}\n{2}\n", type, logString, stackTrace);

        // Пишем в файл
        logWriter.WriteLine(logEntry);
        logWriter.Flush(); // Очистка буфера, чтобы записать данные немедленно
    }
}