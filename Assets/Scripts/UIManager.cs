using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject _songsPanel;
    
    [Header("Parent Objects")] 
    [SerializeField]
    private GameObject _songsContent;
    
    
    [Header("Prefabs")] 
    [SerializeField]
    private GameObject _songContentPrefab;


    [SerializeField]private List<SongName> _songObjects;









    public static UIManager instance = null;
    void Awake () 
    {
        if (instance == null) 
        {
            instance = this;
        } 
        else if(instance == this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    
    
    
    

    public void SetAudioInfo()
    {
        string path = Path.Combine(Application.persistentDataPath, "Audio");
            //Application.dataPath + "/../Assets/StreamingAssets/Audio";
        if (!FileBrowserHelpers.DirectoryExists(Application.persistentDataPath + "Audio"))
        {
            FileBrowserHelpers.CreateFolderInDirectory(Application.persistentDataPath, "Audio");
        }
        string[] files = Directory.GetFiles(path);
        
        if (_songObjects != null && _songObjects.Count > 0)
        {
            ResetSongNames();
        }
        
        SpawnSongNames(files);
    }

    private void SpawnSongNames(string[] files)
    {
        _songObjects = new List<SongName>();

        
        
        for (int i = 0; i < files.Length; i++)
        {
            if (!GameManager.instance.IsAudioFile(files[i]))
            {
                continue;
            }
            
            GameObject song = Instantiate(_songContentPrefab);
            song.transform.SetParent(_songsContent.transform);
            _songObjects.Add(song.GetComponent<SongName>());
            song.GetComponent<SongName>().ChangeText(Path.GetFileName(files[i]));
            song.GetComponent<SongName>().SetName(Path.GetFileName(files[i]));
            song.GetComponent<SongName>().SetPath(files[i]);
        }
    }

    private void ResetSongNames()
    {
        for (int i = 0; i < _songObjects.Count; i++)
        {
            Destroy(_songObjects[i].gameObject);
        }
    }

    public void SetSelectedSong()
    {
        if (!IsHaveSelectedSongName())
        {
            return;
        }
        foreach (SongName songObject in _songObjects)
        {
            if (songObject.isSelected)
            {
                GameManager.instance.SetAudioFromPath(songObject.GetPath());
                break;
            }
        }
        _songsPanel.SetActive(false);
    }

    public bool IsHaveSelectedSongName()
    {
        foreach (SongName songObject in _songObjects)
        {
            if (songObject.isSelected)
            {
                return true;
            }
        }

        return false;
    }

    public void UnselectAllSelectedSongName()
    {
        foreach (SongName songObject in _songObjects)
        {
            if (songObject.isSelected)
            {
                songObject.SetUnselected();
            }
        }
    }
}
