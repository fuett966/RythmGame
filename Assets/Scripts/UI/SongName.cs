using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    
    [SerializeField] private string _fileName;
    [SerializeField] private string _path;
    
    
    
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _normalColor;
    

    public bool isSelected;
    
    public void ChangeText(string newText)
    {
        _text.text = newText;
    }
    
    public void SetName(string fileName)
    {
        _fileName = fileName;
    }

    public string GetName()
    {
        return _fileName;
    }
    

    public void SetPath(string path)
    {
        _path = path;
    }

    public string GetPath()
    {
        return _path;
    }

    public void SetSelected()
    {
        if (UIManager.instance.IsHaveSelectedSongName())
        {
            UIManager.instance.UnselectAllSelectedSongName();
        }
        isSelected = true;
        /*Button button = this.GetComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = _selectedColor;
        colors.selectedColor = _selectedColor;
        button.colors = colors;*/
    }
    public void SetUnselected()
    {
        /*isSelected = false;
        Button button = this.GetComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = _normalColor;
        colors.selectedColor = _normalColor;
        button.colors = colors;*/
    }
}
