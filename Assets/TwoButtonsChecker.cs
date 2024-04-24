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
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;
    
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

    private void SetButtonColors(Button button, Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        button.colors = colors;
    }
    
}
