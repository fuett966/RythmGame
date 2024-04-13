using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] public float bpm;
    [SerializeField] private float _step = 1f;

    public float sampledtime;
    public int _lastInterval;


    [Range(0, 1)] [SerializeField] private float value;

    private List<Transform> line3;

    private int countLine3;
    private Vector3 point1;

    [SerializeField] private Transform object3;
    [SerializeField] private Transform parentLine3;
    [SerializeField] private GameObject PositionsParent;
    private List<Transform> positions;
    private bool _isStopped;
    private int firstIndex = 0;
    private int lastIndex = 1;

    void Start()
    {
        positions = new List<Transform>();
        for (int i = 0; i < PositionsParent.transform.childCount; i++)
        {
            positions.Add(PositionsParent.transform.GetChild(i));
        }

        /*if (positions.Count / 2 != 0)
        {
            positions.Remove(positions[positions.Count - 1]);
        }*/

        line3 = new List<Transform>();
        RefreshLine3();

    }

    public void StopMove()
    {
        _isStopped = true;
        firstIndex = 0;
        lastIndex = 1;
        line3[1].position = positions[firstIndex].position;
        line3[3].position = positions[lastIndex].position;
        _lastInterval = 0;
        Vector3 middlePoint = Vector3.Lerp(line3[1].position, line3[3].position, 0.5f);
        middlePoint.y += Vector3.Distance(line3[1].position, line3[3].position);
        line3[2].position = middlePoint;
        RefreshLine3();

    }

    public void StartMove()
    {
        _isStopped = false;
        firstIndex = 0;
        lastIndex = 1;
        line3[1].position = positions[firstIndex].position;
        line3[3].position = positions[lastIndex].position;
        _lastInterval = 0;
        Vector3 middlePoint = Vector3.Lerp(line3[1].position, line3[3].position, 0.5f);
        middlePoint.y += Vector3.Distance(line3[1].position, line3[3].position);
        line3[2].position = middlePoint;

        value = 0;
        RefreshLine3();
        PlusValue();
    }

    void RefreshLine3()
    {
        parentLine3.GetComponentsInChildren<Transform>(line3);
        countLine3 = line3.Count;
    }

    void LerpLine3()
    {
        List<Vector3> list = new List<Vector3>();
        for (int i = 1; i < line3.Count - 1; i++)
        {
            list.Add(Vector3.Lerp(line3[i].position, line3[i + 1].position, value));
        }

        Lerp2Line3(list);
    }

    void Lerp2Line3(List<Vector3> list2)
    {
        if (list2.Count > 2)
        {
            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < list2.Count - 1; i++)
            {
                list.Add(Vector3.Lerp(list2[i], list2[i + 1], value));
            }

            Lerp2Line3(list);
        }
        else
        {
            object3.position = Vector3.Lerp(list2[0], list2[1], value);
        }
    }

    async void PlusValue()
    {

        while (value < 1)
        {
            if (_isStopped)
            {
                return;
            }

            await Task.Delay(1);
            sampledtime = (BPMAnalyzer.instance.audioSource.timeSamples /
                           (BPMAnalyzer.instance.audioSource.clip.frequency * (60f / (bpm * _step))));
            value = (sampledtime - (float) _lastInterval);
            Move();
        }

        if (Mathf.FloorToInt(sampledtime) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(sampledtime);
        }

        firstIndex += 2;
        if (firstIndex >= positions.Count)
        {
            firstIndex = 0;
            lastIndex = 1;
        }
            
        line3[1].position = positions[firstIndex].position;

        
        if (firstIndex == 8 || firstIndex == 12 || firstIndex == 18)
        {
            Vector3 middlePoint = Vector3.Lerp(line3[1].position, line3[3].position, 0.5f);
            line3[2].position = middlePoint;
        }
        else
        {
            Vector3 middlePoint = Vector3.Lerp(line3[1].position, line3[3].position, 0.5f);
            middlePoint.y += Vector3.Distance(line3[1].position, line3[3].position);
            line3[2].position = middlePoint;
        }


        MinusValue();
    }

    async void MinusValue()
    {
        while (value >= 0)
        {
            if (_isStopped)
            {
                return;
            }

            await Task.Delay(1);
            sampledtime = (BPMAnalyzer.instance.audioSource.timeSamples /
                           (BPMAnalyzer.instance.audioSource.clip.frequency * (60f / (bpm * _step))));
            value = 1 - (sampledtime - (float) _lastInterval);
            Move();
        }

        if (Mathf.FloorToInt(sampledtime) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(sampledtime);

        }
        

        lastIndex += 2;
        if (lastIndex >= positions.Count)
        {
            lastIndex = 1;
            firstIndex = 0;
        }
            

        line3[3].position = positions[lastIndex].position;
        /*if (lastIndex == 9 )
        {
            _step = 1;
        }
        if (lastIndex == 11 )
        {
            _step = 0.5f;
        }*/
        

        Vector3 middlePoint = Vector3.Lerp(line3[1].position, line3[3].position, 0.5f);
        middlePoint.y += Vector3.Distance(line3[1].position, line3[3].position);
        line3[2].position = middlePoint;


        PlusValue();
    }

    void Move()
    {
        if (parentLine3.childCount != countLine3 - 1)
        {
            RefreshLine3();
        }

        LerpLine3();
    }
}