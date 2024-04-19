using System.Collections;
using System.Collections.Generic;
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
    }

    public void GenerateLevel()
    {
        _line.SetPositionsList(_levelGenerator.GenerateLevel(_audioSource.clip.length,_bpm));
    }

    public void StartGame()
    {
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

    public void AddScore()
    {
        
    }
    
    
}
