using UnityEngine;
using System;

public class BPMAnalyzer : MonoBehaviour
{
    public AudioSource audioSource;
    public float bpm;
    public GameObject loadScreen;
    public BeatManager BeatManager;
    public Line line;
    public LevelGenerator levelGenerator;

    public static BPMAnalyzer instance = null;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AnalyzeBPM()
    {
        
        bpm = UniBpmAnalyzer.AnalyzeBpm(audioSource.clip);
        
        audioSource.Play();
        
        BeatManager._bpm = bpm;
        //line.SetMoveValue(bpm, 0.5f);
        line.SetPositionsList(levelGenerator.GenerateLevel(audioSource.clip.length,bpm));
        
        line.StartMove();
    }
    public void StopAnalyzeBPM()
    {
        audioSource.Stop();
        line.StopMove();
    }
}