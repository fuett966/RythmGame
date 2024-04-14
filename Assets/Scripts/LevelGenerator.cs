using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private GameObject sphere;
    [SerializeField] private GameObject note;
    [SerializeField] private GameObject tube;
    [SerializeField] private GameObject environment;
    private float _clipDuration; // clip duration * bpm
    private float _clipBitPerMinute;
    private List<Transform> notes;
    
    

    public List<Transform> GenerateLevel(float clipDuration,float clipBPM)
    {
        //list id tube
        
        
        int noteCount = (int)((clipBPM/(60*2)) * clipDuration);
        Debug.Log(noteCount);
        if (notes != null && notes.Count != 0)
        {
            for (int i = 0; i < notes.Count-1; i++)
            {
                Destroy(notes[i].gameObject);
            }
        }
        notes = new List<Transform>();
        
        for (int i = 0; i < noteCount; i++)
        {
            GameObject noteTemp = Instantiate(note,startPosition,false);
            notes.Add(noteTemp.transform);
            if (i == 0)
            {
                noteTemp.transform.position = startPosition.position;
                continue;
            }

            if (i % 2 == 1)
            {
                noteTemp.transform.position = new Vector3(notes[i-1].position.x + Random.Range(1,5),notes[i-1].position.y - Random.Range(1,5),0f); 
            }

            else
            {
                noteTemp.transform.position = new Vector3(notes[i-1].position.x - Random.Range(1,5),notes[i-1].position.y - Random.Range(1,5),0f); 
            }

        }

        sphere.transform.position = notes[0].position;

        return notes;
    }

    
}
