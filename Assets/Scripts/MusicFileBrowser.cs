using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using NativeFilePickerNamespace;
using UnityEngine;
using SFB;

public class MusicFileBrowser : MonoBehaviour
{
    
    public void OpenFileBrowserToAddAudioToAssets()
    {
        
        


        //не работает на андроид 10+
        //FileBrowser.SetFilters( true, new FileBrowser.Filter( "Audio", ".mp3", ".wav", ".ogg" ));
        StartCoroutine(ShowLoadDialogCoroutine());


        //не работает 
        /*StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", "", false, (string[] paths) =>
        {
            GetAudio(paths);
        });*/
    }
    
    IEnumerator ShowLoadDialogCoroutine()
    {
        /*NativeFilePicker.Permission callback =  NativeFilePicker.PickFile(null, new string[] { ".mp3", ".wav", ".ogg"});
        yield return callback;
        
        if (callback == NativeFilePicker.Permission.Granted)
        {
            UIManager.instance.SetAudioInfo();
        }*/
        
        
        //не работает на 10+
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, true, null, null, "Select Files", "Load" );

        
        if (FileBrowser.Success)
        {
            GetAudio( FileBrowser.Result );
            UIManager.instance.SetAudioInfo();
        } 
    }
	
    void OnFilesSelected( string[] filePaths )
    {
        string filePath = filePaths[0];

        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( filePath );

        string destinationPath = Path.Combine( Application.persistentDataPath, FileBrowserHelpers.GetFilename( filePath ) );
        FileBrowserHelpers.CopyFile( filePath, destinationPath );
    }
    

    private void GetAudio(string[] paths)
    {
        for (int i = 0; i < paths.Length; i++)
        {
            if (!GameManager.instance.IsAudioFile(paths[i]))
            {
                Debug.LogError("File is not an audio file: " + paths[i]);
                //показать что выбран не аудио файл
                return;
            }

            AddAudioFile(paths[i]);
        }
    }

    private void AddAudioFile(string filePath)
    {
        string destinationPath = Path.Combine(Application.persistentDataPath, "Audio", Path.GetFileName(filePath));

        if (!FileBrowserHelpers.DirectoryExists(Application.persistentDataPath + "Audio"))
        {
            FileBrowserHelpers.CreateFolderInDirectory(Application.persistentDataPath, "Audio");
        }
        FileBrowserHelpers.CopyFile(filePath,Application.persistentDataPath+"Audio");
        Debug.Log(Application.persistentDataPath);
        //Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
        //File.Copy(filePath, destinationPath, true);

        
        /*string destinationFolderPath = Path.Combine(Application.dataPath, "StreamingAssets/Audio");
        string fileName = Path.GetFileName(filePath);
        string destinationFilePath = Path.Combine(destinationFolderPath, fileName);
        File.Copy(filePath, destinationFilePath, true);
        Resources.UnloadUnusedAssets();*/
    }
}
