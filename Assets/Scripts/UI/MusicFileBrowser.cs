using System;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;

public class MusicFileBrowser : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void OpenFileBrowserToAddAudioToAssets()
    {

        //ImportFromIntent(Application.persistentDataPath+"Audio");
        //UIManager.instance.SetAudioInfo();
        
        FileBrowser.SetFilters( true, new FileBrowser.Filter( "Audio", ".mp3", ".wav", ".ogg" ));
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Select Files", "Load");
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
            #if UNITY_ANDROID
            Uri uri = new Uri(paths[i]);
            paths[i] = uri.LocalPath;
            paths[i] = "/storage/emulated/0/"+ paths[i].Remove(0, paths[i] .LastIndexOf(':') +1);
            text.text = paths[i];
            #endif
            
            
            
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
    
    
    
    private void ImportFromIntent(string importPath)
        {
            try
            {
                // Get the current activity
                AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

                // Get the current intent
                AndroidJavaObject intent = activityObject.Call<AndroidJavaObject>("getIntent");

                // Get the intent data using AndroidJNI.CallObjectMethod so we can check for null
                IntPtr method_getData = AndroidJNIHelper.GetMethodID(intent.GetRawClass(), "getData", "()Ljava/lang/Object;");
                IntPtr getDataResult = AndroidJNI.CallObjectMethod(intent.GetRawObject(), method_getData, AndroidJNIHelper.CreateJNIArgArray(new object[0]));
                if (getDataResult.ToInt32() != 0)
                {
                    // Now actually get the data. We should be able to get it from the result of AndroidJNI.CallObjectMethod, but I don't now how so just call again
                    AndroidJavaObject intentURI = intent.Call<AndroidJavaObject>("getData");
                    text.text = intentURI.ToString();
                    // Open the URI as an input channel
                    AndroidJavaObject contentResolver = activityObject.Call<AndroidJavaObject>("getContentResolver");
                    AndroidJavaObject inputStream = contentResolver.Call<AndroidJavaObject>("openInputStream", intentURI);
                    AndroidJavaObject inputChannel = inputStream.Call<AndroidJavaObject>("getChannel");

                    // Open an output channel
                    AndroidJavaObject outputStream = new AndroidJavaObject("java.io.FileOutputStream", importPath);
                    AndroidJavaObject outputChannel = outputStream.Call<AndroidJavaObject>("getChannel");

                    // Copy the file
                    long bytesTransfered = 0;
                    long bytesTotal = inputChannel.Call<long>("size");
                    while (bytesTransfered < bytesTotal)
                    {
                        bytesTransfered += inputChannel.Call<long>("transferTo", bytesTransfered, bytesTotal, outputChannel);
                    }

                    // Close the streams
                    inputStream.Call("close");
                    outputStream.Call("close");
                    

                }
                
            }
            catch (System.Exception ex)
            {
                text.text = "error";
            }
            
        }
    
    
    
    
    
}