using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject _songsPanel;

    [SerializeField] 
    private TextMeshProUGUI _scoreText;
    
    [SerializeField] 
    private TextMeshProUGUI _timerToStartGameText;
    [SerializeField] 
    private TwoButtonsChecker _autoPanel;
    [SerializeField] 
    private TwoButtonsCheckersContainer _heroesPanel;
    [SerializeField] 
    private GameObject _heroesEnablingPanel;
    [SerializeField] 
    private Button _startSongButton;
    [SerializeField] 
    private Button _addNewSongButton;
    
    
    
    [Header("Parent Objects")] 
    [SerializeField]
    private GameObject _songsContent;
    
    
    
    [Header("Text for UI")]
    [SerializeField]
    private string _scoreString = "";
    
    
    
    
    [Header("Prefabs")] 
    [SerializeField]
    private GameObject _songContentPrefab;


    private List<SongName> _songObjects;



    [Header("Bools for debug")] 
    [SerializeField]
    private bool _isAutoGame;
    [SerializeField]
    private bool _isUseHeroes;

    [SerializeField] private HeroesType _heroesType;





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

    public void ChangeScoreText(int score)
    {
        _scoreText.text = _scoreString + score.ToString();
    }
    public void ChangeTimerText(string time)
    {
        _timerToStartGameText.text = time;
    }

    public void SetAutoGame(bool value)
    {
        _isAutoGame = value;
    }
    public void SetUseHeroes(bool value)
    {
        _isUseHeroes = value;
    }
    public void SetHeroesType(HeroesType type)
    {
        _heroesType = type;
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
    
    public enum HeroesType
    {
        Dota,
        AngryBirds,
        GenshinImpact,
        BrawlStars
    }
}
