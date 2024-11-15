using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Values")] private float _bpm;
    public float BPM => _bpm;

    private int _score = 0;
    public int Score => _score;


    [Header("Objects")] [SerializeField] private AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;


    [Header("Scripts")] [SerializeField] private AudioVisualizer _audioVisualizer;
    [SerializeField] private BPMAnalyzer _bpmAnalyzer;
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private Line _line;
    [SerializeField] private BeatManager _beatManager;
    [SerializeField] private MusicFileBrowser _musicFileBrowser;


    [Header("Positions")] [SerializeField] private Transform _startPosition;


    [Header("Bools for debug")] [SerializeField]
    public bool _audioAnalyzed;

    [SerializeField] public bool _levelGenerated;
    [SerializeField] public bool _gameStarted;
    [SerializeField] public UIManager.HeroesType _heroesType;

    public static GameManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
        Application.targetFrameRate = 60;
       // Screen.SetResolution(1080, 1920, false);
        DontDestroyOnLoad(gameObject);
        
    }

    public void AnalyzeBPM()
    {
        _bpm = UniBpmAnalyzer.AnalyzeBpm(_audioSource.clip);
        _beatManager._bpm = _bpm;
        if (_bpm < 125)
        {
            _line._step = 1f;
        }
        else if (_bpm < 200)
        {
            _line._step = 0.5f;
        }
        else if(_bpm < 290)
        {
            _line._step = 0.25f;
        }
        else 
        {
            _line._step = 0.125f;
        }
        _line.BPM = _bpm;
        _audioAnalyzed = true;
    }

    public void GenerateLevel()
    {
        if (!_audioAnalyzed)
        {
            Debug.LogError("Audio not analyzed!");
            return;
        }

        _line.SetPositionsList(_levelGenerator.GenerateLevel(_audioSource.clip.length, _bpm, _line._step,
            UIManager.instance._isUseHeroes, _heroesType));
        _levelGenerated = true;
    }

    public void StartPlay()
    {
        UIManager.instance._mainButton.gameObject.SetActive(false);
        UIManager.instance._pauseButton.gameObject.SetActive(false);
        UIManager.instance._returnToMenuButton.gameObject.SetActive(false);
        StartTimer(3);
    }

    private async void StartTimer(float timeAwait)
    {
        if (_gameStarted)
        {
            Debug.LogError("Game already started!");
            return;
        }

        UIManager.instance.SetActiveLoadScreen(true);
        UIManager.instance.SetSelectedSong();
        
        _gameStarted = true;
        while (!_audioAnalyzed || !_levelGenerated)
        {
            await Task.Delay(1000);
        }
        float tempTime = timeAwait;
        while (tempTime > 0)
        {
            await Task.Delay(1000);
            tempTime -= 1;
        }
        if (UIManager.instance._isAutoGame)
        {
            UIManager.instance._scoreText.text = "";
            UIManager.instance._scoreButton.SetActive(false);
        }
        else
        {
            UIManager.instance._scoreText.text = "0";
            UIManager.instance._scoreButton.SetActive(true);
        }

        UIManager.instance.SetActiveLoadScreen(false);
        StartGame();
    }

    private async void StartTimerInGame(float timeAwait)
    {
        float tempTime = timeAwait;
        while (tempTime > 0)
        {
            await Task.Delay(1000);
            tempTime -= 1;
            UIManager.instance.ChangeTimerText(tempTime.ToString());
        }

        UIManager.instance.ChangeTimerText("");

        
        _audioSource.Play();
        _line.StartMove();
        UIManager.instance._mainButton.gameObject.SetActive(true);
        /*UIManager.instance._pauseButton.gameObject.SetActive(true);
        UIManager.instance._returnToMenuButton.gameObject.SetActive(true);*/
    }

    private void StartGame()
    {
        if (!_levelGenerated)
        {
            Debug.LogError("Level not generated!");
            return;
        }

        StartTimerInGame(3);
    }

    public void StopGame()
    {
        _gameStarted = false;
        _audioSource.Stop();
        _line.StopMove();
    }

    public void PauseGame()
    {
        Debug.Log("Game paused");
        /*if (!_audioSource.lis)
        {
            return;
        }*/
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.Play();
        }

        //_line.StopMove();

    }

    public void AddScore()
    {
        if (_line.isTact)
        {
            _score += 1;
            UIManager.instance._scoreText.text = _score.ToString();
            _line.isTact = false;
        }
        else
        {

        }

    }

    public void ContinueGame()
    {
        Debug.Log("Game continued");
        _audioSource.Play();
        _line.ContinueMove();
    }

    public void SetAudioFromPath(string path)
    {
        StartCoroutine(LoadAudioClip(path));
    }

    public void SetAudioFromClip(AudioClip clip)
    {
        _audioSource.clip = clip;
        AnalyzeBPM();
        GenerateLevel();
        Debug.Log("Music loading success");
        
    }

    public void ResetGame()
    {
        _levelGenerator.sphere.transform.position = _levelGenerator.startPosition.position;
        _line.ResetValues();
        _gameStarted = false;
        _audioAnalyzed = false;
        _levelGenerated = false;

    }

    IEnumerator LoadAudioClip(string filePath)
    {

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Music UNLOAD!");
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                _audioSource.clip = audioClip;
                AnalyzeBPM();
                GenerateLevel();
                Debug.Log("Music loading success");
            }
        }
    }

    public void ClickInGame()
    {
        _score += 1;
        UIManager.instance.ChangeScoreText(_score);
    }


    public bool IsAudioFile(string filePath)
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