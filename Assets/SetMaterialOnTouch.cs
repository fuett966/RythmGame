using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialOnTouch : MonoBehaviour
{
    public MeshRenderer target;
    public Material material;
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player"))
        {
            target.materials[0] = material;
        }*/
    }
}
