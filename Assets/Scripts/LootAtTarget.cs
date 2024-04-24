using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootAtTarget : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.LookAt(other.transform);
        }
    }
}
