using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private GameObject sphere;
    [SerializeField] private GameObject note;
    [SerializeField] private GameObject tube;
    [SerializeField] private GameObject environment;
    private List<Transform> notes;


    private void ClearLevel()
    {
        for (int i = 0; i < notes.Count - 1; i++)
        {
            Destroy(notes[i].gameObject);
        }
        
    }

    public List<Transform> GenerateLevel(float clipDuration, float clipBeatPerMinute, float stepModify)
    {
        if (notes != null && notes.Count != 0)
        {
            ClearLevel();
        }
        notes = new List<Transform>();
        int noteCount = (int) ((clipBeatPerMinute / (60 * stepModify)) * clipDuration)+1;
        Debug.Log("Clip duration "+ clipDuration);
        Debug.Log("Note count "+ noteCount);
        
        for (int i = 0; i < noteCount; i++)
        {
            GameObject noteTemp = Instantiate(note, startPosition, false);
            notes.Add(noteTemp.transform);
            if (i == 0)
            {
                noteTemp.transform.position = startPosition.position;
                continue;
            }

            if (i % 2 == 1)
            {
                noteTemp.transform.position = new Vector3(notes[i - 1].position.x + Random.Range(3, 5), notes[i - 1].position.y - Random.Range(1, 4), 0f);
            }

            else
            {
                noteTemp.transform.position = new Vector3(notes[i - 1].position.x - Random.Range(3, 5), notes[i - 1].position.y - Random.Range(2, 4), 0f);
            }

        }

        sphere.transform.position = notes[0].position;

        return notes;
    }


}