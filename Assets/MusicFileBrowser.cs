using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SFB;

public class MusicFileBrowser : MonoBehaviour
{
    public void OpenFileBrowserToFindAudio()
    {
        StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", "", false, (string[] paths) =>
        {
            Debug.Log("Гетаудио");
            GetAudio(paths);
        });
    }

    private void GetAudio(string[] paths)
    {
        Debug.Log(paths.Length);
        //проверка на файл расширения mp3
        //сохранение только аудио
        for (int i = 0; i < paths.Length; i++)
        {
            if (!IsAudioFile(paths[i]))
            {
                Debug.LogError("File is not an audio file: " + paths[i]);
                return;
            }
            Debug.Log("ЭТО АУДИО");

            AddAudioFile(paths[i]);
        }
    }

    private void AddAudioFile(string filePath)
    {
        // Получаем путь к папке Assets/Audio
        Debug.Log("Получаем путь к папке Assets/Audio");
        string destinationFolderPath = Path.Combine(Application.dataPath, "Audio");
        Debug.Log("Получаем имя файла из исходного пути");
        // Получаем имя файла из исходного пути
        string fileName = Path.GetFileName(filePath);
        Debug.Log("Получаем путь к конечному файлу");
        // Получаем путь к конечному файлу
        string destinationFilePath = Path.Combine(destinationFolderPath, fileName);
        Debug.Log("Копируем файл в папку Assets/Audio");
        // Копируем файл в папку Assets/Audio
        File.Copy(filePath, destinationFilePath, true);
        Debug.Log("Перезагружаем ресурсы Unity, чтобы обновить список аудиофайлов");
        // Перезагружаем ресурсы Unity, чтобы обновить список аудиофайлов
        Resources.UnloadUnusedAssets();
    }
    private bool IsAudioFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".mp3":
            case ".wav":
            case ".ogg":
                return true;
            default:
                return false;
        }
    }
}
