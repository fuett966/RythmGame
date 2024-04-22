using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class AudioFileDisplayer : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public void SetAudioInfo()
    {
        string path = Application.dataPath + "/../Assets/Audio";
        string[] files = Directory.GetFiles(path);

        foreach (string file in files)
        {
            textComponent.text = Path.GetFileName(file);
        }
    }
}
