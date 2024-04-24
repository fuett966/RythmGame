using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoButtonsChecker : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _enableButton;
    [SerializeField] private Button _disableButton;

    [Header("Buttons")]
    [SerializeField] private Sprite _selectedColor;
    [SerializeField] private Sprite _unselectedColor;
    
    [SerializeField] private bool _isEnabled;

    private void Start()
    {
        IsEnabled = _isEnabled;
    }

    public bool IsEnabled
    {
        get
        {
            return _isEnabled;
        }
        set
        {
            if (value)
            {
                SetButtonColors(_enableButton, _selectedColor);
                SetButtonColors(_disableButton, _unselectedColor);
            }
            else
            {
                SetButtonColors(_enableButton, _unselectedColor);
                SetButtonColors(_disableButton, _selectedColor);
            }

            _isEnabled = value;
        }
    }

    private void SetButtonColors(Button button, Sprite color)
    {
        button.GetComponent<Image>().sprite = color;
        /*ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        button.colors = colors;*/

    }
    
}
