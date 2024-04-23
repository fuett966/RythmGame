using System.Collections;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;

public class MusicFileBrowser : MonoBehaviour
{

    public void OpenFileBrowserToAddAudioToAssets()
    {
        FileBrowser.SetFilters( true, new FileBrowser.Filter( "Audio", ".mp3", ".wav", ".ogg" ));
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Select Files",
            "Load");
        if (FileBrowser.Success)
        {
            GetAudio(FileBrowser.Result);
            UIManager.instance.SetAudioInfo();
        }
    }

    private void GetAudio(string[] paths)
    {
        for (int i = 0; i < paths.Length; i++)
        {
            AddAudioFile(paths[i]);
        }
    }

    public void AddAudioFile(string filePath)
    {
        if (!GameManager.instance.IsAudioFile(filePath))
        {
            Debug.LogError("File is not an audio file: " + filePath);
            return;
        }
        string destinationPath = Path.Combine(Application.persistentDataPath, "Audio", Path.GetFileName(filePath));
        FileBrowserHelpers.CopyFile(filePath, Application.persistentDataPath + "Audio");
        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
        File.Copy(filePath, destinationPath, true);
    }
}