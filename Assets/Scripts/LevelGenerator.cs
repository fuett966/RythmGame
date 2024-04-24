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

    [Header("Heroes")] 
    [SerializeField] private GameObject[] _dota;
    [SerializeField] private GameObject[] _genshin;
    [SerializeField] private GameObject[] _brawl;
    [SerializeField] private GameObject[] _angryBirds;

    private void ClearLevel()
    {
        for (int i = 0; i < notes.Count - 1; i++)
        {
            Destroy(notes[i].gameObject);
        }
        
    }

    public List<Transform> GenerateLevel(float clipDuration, float clipBeatPerMinute, float stepModify, bool withHeroes, UIManager.HeroesType heroesType)
    {
        if (notes != null && notes.Count != 0)
        {
            ClearLevel();
        }
        notes = new List<Transform>();
        int noteCount = (int) ((clipBeatPerMinute / (60 * stepModify)) * clipDuration)+1;
        
        
        
        
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
                noteTemp.transform.position = new Vector3(notes[i - 1].position.x + Random.Range(2, 5), notes[i - 1].position.y - Random.Range(2, 5), 0f);
            }
            else
            {
                noteTemp.transform.position = new Vector3(notes[i - 1].position.x - Random.Range(3, 5), notes[i - 1].position.y - Random.Range(3, 5), 0f);
            }

            if (withHeroes && i % 4 == 0)
            {
                GameObject heroObject = SpawnHeroWithType(heroesType);

                heroObject.transform.position = noteTemp.transform.position;
                heroObject.transform.position = new Vector3(heroObject.transform.position.x+2f , heroObject.transform.position.y,heroObject.transform.position.z+2f);
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