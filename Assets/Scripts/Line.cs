using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private float _bpm;

    public float BPM
    {
        get { return _bpm; }
        set { _bpm = value; }
    }

    [SerializeField] private float _step = 1f;

    [SerializeField] private float _sampledtime;
    [SerializeField] private int _lastInterval;


    [Range(0, 1)] [SerializeField] private float value;

    [SerializeField] private List<Transform> _curve;


    [SerializeField] private Transform _movableObject;


    private List<Transform> positions;
    private bool _isStopped;

    private int firstIndex = 0;
    private int lastIndex = 1;

    public void StartMove()
    {

        _isStopped = false;
        firstIndex = 0;
        lastIndex = 1;

        _curve[1].position = positions[firstIndex].position;
        _curve[3].position = positions[lastIndex].position;
        _curve[2].position = CalculateMiddlePoint(0.5f, 4f);
        
        _lastInterval = 0;
        _sampledtime = (GameManager.instance.AudioSource.timeSamples / (GameManager.instance.AudioSource.clip.frequency * (60f / (_bpm * _step))));
        
        value = 0;
        PlusValue();
    }

    public void ContinueMove()
    {
        _isStopped = false;
        
        _curve[1].position = positions[firstIndex].position;
        _curve[3].position = positions[lastIndex].position;
        _curve[2].position = CalculateMiddlePoint(0.5f, 4f);

        
        if (firstIndex < lastIndex)
        {
            MinusValue();
        }
        else
        {
            PlusValue();
        }
    }

    public void StopMove()
    {
        _isStopped = true;

        /*firstIndex = 0;
        lastIndex = 1;

        _curve[1].position = positions[firstIndex].position;
        _curve[3].position = positions[lastIndex].position;

        _lastInterval = 0;

        _curve[2].position = CalculateMiddlePoint(0.5f, 4f);

        value = 0;*/

    }


    private Vector3 CalculateMiddlePoint(float valueBetween, float height)
    {
        Vector3 middlePoint = Vector3.Lerp(_curve[1].position, _curve[3].position, valueBetween);
        middlePoint.y += height;
        return middlePoint;
    }

    public void SetPositionsList(List<Transform> list)
    {
        positions = new List<Transform>(list);
    }


    async void PlusValue()
    {
        if (_isStopped)
        {
            _isStopped = false;
            return;
        }

        /*if (firstIndex == 6 || firstIndex == 14 || firstIndex == 20 || firstIndex == 28)
        {
            _step = 1f;
        }*/

        _sampledtime = (GameManager.instance.AudioSource.timeSamples /
                        (GameManager.instance.AudioSource.clip.frequency * (60f / (_bpm * _step))));
        if (Mathf.FloorToInt(_sampledtime) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(_sampledtime);
        }

        while (value < 1)
        {
            if (_isStopped)
            {
                _isStopped = false;
                return;
            }
            await Task.Delay(1);
            _sampledtime = (GameManager.instance.AudioSource.timeSamples /
                            (GameManager.instance.AudioSource.clip.frequency * (60f / (_bpm * _step))));
            value = _sampledtime - _lastInterval;
            Move();
        }


        firstIndex += 2;
        if (firstIndex >= positions.Count)
        {
            _isStopped = true;
        }


        _curve[1].position = positions[firstIndex].position;
        _curve[2].position = CalculateMiddlePoint(0.5f, 4f);

        MinusValue();
    }

    async void MinusValue()
    {
        if (_isStopped)
        {
            _isStopped = false;
            return;
        }

        /*if (lastIndex == 9 || lastIndex == 17 || lastIndex == 23 || lastIndex == 35)
        {
            _step = 0.5f;
        }*/

        _sampledtime = (GameManager.instance.AudioSource.timeSamples /
                        (GameManager.instance.AudioSource.clip.frequency * (60f / (_bpm * _step))));
        if (Mathf.FloorToInt(_sampledtime) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(_sampledtime);
        }

        while (value >= 0)
        {
            if (_isStopped)
            {
                _isStopped = false;
                return;
            }

            await Task.Delay(1);

            _sampledtime = (GameManager.instance.AudioSource.timeSamples /
                            (GameManager.instance.AudioSource.clip.frequency * (60f / (_bpm * _step))));
            value = 1 - (_sampledtime - _lastInterval);
            Move();
        }
        
        lastIndex += 2;
        if (lastIndex >= positions.Count)
        {
            _isStopped = true;
        }
        _curve[3].position = positions[lastIndex].position;
        _curve[2].position = CalculateMiddlePoint(0.5f, 4f);

        PlusValue();
    }

    private void Move()
    {
        List<Vector3> list = new List<Vector3>();
        for (int i = 1; i < _curve.Count - 1; i++)
        {
            list.Add(Vector3.Lerp(_curve[i].position, _curve[i + 1].position, value));
        }

        LerpLine(list);
    }


    private void LerpLine(List<Vector3> list2)
    {
        if (list2.Count > 2)
        {
            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < list2.Count - 1; i++)
            {
                list.Add(Vector3.Lerp(list2[i], list2[i + 1], value));
            }

            LerpLine(list);
        }
        else
        {
            _movableObject.position = Vector3.Lerp(list2[0], list2[1], value);
        }
    }
}