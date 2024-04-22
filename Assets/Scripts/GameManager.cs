
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [Header("Values")]
    
    
    private float _bpm;
    public float BPM => _bpm;
    
    private int _score = 0;
    public int Score => _score;
    
    
    
    [Header("Objects")]
    
    [SerializeField]private AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;
    
    
    
    [Header("Scripts")]
    
    [SerializeField] private AudioVisualizer _audioVisualizer;
    [SerializeField] private BPMAnalyzer _bpmAnalyzer;
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private Line _line;
    [SerializeField] private BeatManager _beatManager;
    [SerializeField] private MusicFileBrowser _musicFileBrowser;
    
    
    [Header("Positions")]
    
    [SerializeField] private Transform _startPosition;
    
    
    
    [Header("Bools")]
    
    [SerializeField] private bool _audioAnalyzed;
    [SerializeField] private bool _levelGenerated;
    
    
    
    public static GameManager instance = null;
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

    public void AnalyzeBPM()
    {
        _bpm = UniBpmAnalyzer.AnalyzeBpm(_audioSource.clip);
        _beatManager._bpm = _bpm;
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
        _line.SetPositionsList(_levelGenerator.GenerateLevel(_audioSource.clip.length,_bpm));
        _levelGenerated = true;
    }

    public void StartGame()
    {
        if (!_levelGenerated)
        {
            Debug.LogError("Level not generated!");
            return;
        }
        _audioSource.Play();
        _line.StartMove();
    }
    
    public void PauseGame()
    {
        _audioSource.Stop();
        _line.StopMove();
    }
    public void ContinueGame()
    {
        _audioSource.Play();
        _line.ContinueMove();
    }

    public void SetAudioFromPath(string path)
    {
        StartCoroutine(LoadAudioClip(path));
    }
    
    IEnumerator LoadAudioClip(string filePath)
    {
        
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://"+filePath, AudioType.MPEG))
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
                Debug.Log("Music loading success");
            }
        }
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
