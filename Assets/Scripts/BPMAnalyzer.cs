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
        //loadScreen.SetActive(true);
        
        bpm = UniBpmAnalyzer.AnalyzeBpm(audioSource.clip);
        
        audioSource.Play();
        BeatManager._bpm = bpm;
        line.SetPositionsList(levelGenerator.GenerateLevel(audioSource.clip.length,bpm));
        line.bpm = bpm;
        line.StartMove();
        //loadScreen.SetActive(false);
    }
    public void StopAnalyzeBPM()
    {
        //loadScreen.SetActive(true);
        
        //bpm = UniBpmAnalyzer.AnalyzeBpm(audioSource.clip);
        
        audioSource.Stop();
        //BeatManager._bpm = bpm;
        //line.bpm = bpm;
        line.StopMove();
        //loadScreen.SetActive(false);
    }
}