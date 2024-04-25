using System;
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
    private GameObject _loadingScreen;
    [SerializeField]
    private GameObject _songsPanel;

    
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

    [Header("INGAMEUI")]
    
    [SerializeField] public GameObject _scoreButton;
    [SerializeField] public Image _mainButton;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _returnToMenuButton;
    [SerializeField] public TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerToStartGameText;
    [Header("InGameMainButton")]
    [SerializeField] private Sprite _mainEnabled;
    [SerializeField] private Sprite _mainDisabled;
    
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
    public bool _isAutoGame;
    [SerializeField]
    public bool _isUseHeroes;

    [SerializeField] public HeroesType _heroesType;





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
        SetAudioInfo();
    }

    public void SwitchActiveMainbutton()
    {
        if (_pauseButton.activeSelf || _returnToMenuButton.activeSelf)
        {
            _pauseButton.SetActive(false);
            _returnToMenuButton.SetActive(false);
            _mainButton.sprite = _mainDisabled;
        }
        else
        {
            _pauseButton.SetActive(true);
            _returnToMenuButton.SetActive(true);
            _mainButton.sprite = _mainEnabled;
        }
    }
    

    public void SetStartPlayButtonActive(bool value)
    {
        _startSongButton.GetComponent<Button>().interactable = value;
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

    public void SetActiveLoadScreen(bool value)
    {
        _loadingScreen.SetActive(value);
    }

    private void Start()
    {
        SongName[] list = _songsContent.GetComponentsInChildren<SongName>();
        _songObjects = new List<SongName>();
        foreach (SongName songName in list)
        {
            _songObjects.Add(songName);
        }

        SetAudioInfo();

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
        ResizeContainer();
    }
    private void ResizeContainer()
    {
        // Получаем все дочерние элементы SongName
        SongName[] songNames = _songsContent.GetComponentsInChildren<SongName>();

        // Вычисляем требуемую высоту
        float targetHeight = songNames.Length * 60f;

        // Изменяем высоту контейнера
        RectTransform rt = _songsContent.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, targetHeight);
    }

    private void SpawnSongNames(string[] files)
    {
        if (_songObjects == null)
        {
            _songObjects = new List<SongName>();
        }
        

        
        
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
            if (_songObjects[i]._clip != null)
            {
                continue;
            }
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
                Debug.LogError("TRACK" + songObject._clip);
                if (songObject._clip != null)
                {
                    GameManager.instance.SetAudioFromClip(songObject._clip);
                }
                else
                {
                    GameManager.instance.SetAudioFromPath(songObject.GetPath());
                }
                
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
        SetStartPlayButtonActive(false);
    }
    
    public enum HeroesType
    {
        Dota2,
        AngryBirds,
        GenshinImpact,
        BrawlStars
    }
}
