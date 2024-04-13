using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target; // Целевой объект для слежения
    public float smoothSpeed = 0.125f; // Скорость плавного следования камеры
    public Vector3 offset; // Смещение камеры относительно целевого объекта

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.DOLookAt(target.position,smoothSpeed);
    }
    
}