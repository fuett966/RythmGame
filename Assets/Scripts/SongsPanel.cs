using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongsPanel : MonoBehaviour
{
    private void OnEnable()
    {
        UIManager.instance.SetAudioInfo();
    }
}
