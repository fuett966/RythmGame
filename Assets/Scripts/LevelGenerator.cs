using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] public Transform startPosition;
    [SerializeField] public GameObject sphere;
    [SerializeField] private GameObject note;
    [SerializeField] private GameObject tube;
    [SerializeField] private GameObject environment;
    
    private List<Transform> notes;

    [Header("Heroes")] 
    [SerializeField] private GameObject[] _dota;
    [SerializeField] private GameObject[] _genshin;
    [SerializeField] private GameObject[] _brawl;
    [SerializeField] private GameObject[] _angryBirds;

    public List<GameObject> characters;
    public List<GameObject> tubes;

    private void ClearLevel()
    {
        if (notes != null && notes.Count != 0)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                Destroy(notes[i].gameObject);
            }
        }

        if (characters != null && characters.Count != 0)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                Destroy(characters[i]);
            }
        }
        if (tubes != null && tubes.Count != 0)
        {
            for (int i = 0; i < tubes.Count; i++)
            {
                Destroy(tubes[i]);
            }
        }

    }

    public List<Transform> GenerateLevel(float clipDuration, float clipBeatPerMinute, float stepModify, bool withHeroes, UIManager.HeroesType heroesType)
    {
        
            ClearLevel();
        
        notes = new List<Transform>();
        characters = new List<GameObject>();
        tubes = new List<GameObject>();
        int noteCount = (int) ((clipBeatPerMinute / (60 * stepModify)) * clipDuration)+1;

        
        
        for (int i = 0; i < noteCount; i++)
        {
            GameObject noteTemp;

            if ( (i % 10 == 6) && i != 0)
            {
                noteTemp = Instantiate(tube, startPosition, false);
                tubes.Add(noteTemp);
                Tube tubeObjectTemp = noteTemp.GetComponent<Tube>();
                notes.Add(tubeObjectTemp.startPosition);
                notes.Add(tubeObjectTemp.endPosition);
                
                
                
                noteTemp.transform.position = new Vector3(notes[i - 1].position.x - Random.Range(4, 7), notes[i - 1].position.y - Random.Range(3, 5), 0f);
                if (Random.Range(0, 5) >= 3)
                {
                    noteTemp.transform.Rotate(new Vector3(0,0,-45));
                }
                else
                {
                    noteTemp.transform.Rotate(new Vector3(0,0,45));
                }
                
                
                
                i++;
                continue;
            }
            noteTemp = Instantiate(note, startPosition, false); 
            
            
            notes.Add(noteTemp.transform);
            if (i == 0)
            {
                noteTemp.transform.position = startPosition.position;
                noteTemp.SetActive(false);
                continue;
            }
            if (i % 2 == 1)
            {
                noteTemp.transform.position = new Vector3(notes[i - 1].position.x + Random.Range(4, 7), notes[i - 1].position.y - Random.Range(3, 5), 0f);
                noteTemp.transform.Rotate(new Vector3(0,0,45));
            }
            else
            {
                noteTemp.transform.position = new Vector3(notes[i - 1].position.x - Random.Range(4, 7), notes[i - 1].position.y - Random.Range(3, 5), 0f);
                noteTemp.transform.Rotate(new Vector3(0,0,-45));
            }
            if (withHeroes && i % 8 == 0)
            {
                GameObject heroObject = SpawnHeroWithType(heroesType);
                characters.Add(heroObject);
                heroObject.transform.position = noteTemp.transform.position;
                heroObject.transform.position = new Vector3(heroObject.transform.position.x+2f , heroObject.transform.position.y,heroObject.transform.position.z+3f);
                heroObject.transform.rotation = new Quaternion(0f, 180f, 0f,0f);
                heroObject.transform.localScale *= 2f;
            }

        }

        sphere.transform.position = notes[0].position;

        return notes;
    }

    private GameObject SpawnHeroWithType(UIManager.HeroesType type)
    {
        GameObject gmHero;
        
        switch (type)
        {
            case  UIManager.HeroesType.Dota2:
                gmHero = Instantiate(_dota[Random.Range(0, _dota.Length )]);
                break;
            case  UIManager.HeroesType.BrawlStars:
                gmHero = Instantiate(_brawl[Random.Range(0, _brawl.Length)]);
                break;
            case  UIManager.HeroesType.GenshinImpact:
                gmHero = Instantiate(_genshin[Random.Range(0, _genshin.Length)]);
                break;
            case  UIManager.HeroesType.AngryBirds:
                gmHero = Instantiate(_angryBirds[Random.Range(0, _angryBirds.Length )]);
                break;
            default:
                gmHero = Instantiate(_dota[Random.Range(0, _dota.Length)]);
                break;
        }

        return gmHero;
    }


}