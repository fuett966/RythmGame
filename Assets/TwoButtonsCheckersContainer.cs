using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoButtonsCheckersContainer : MonoBehaviour
{
    [SerializeField] private List<TwoButtonsChecker> _checkers;
    public List<TwoButtonsChecker> Checkers => _checkers;

    [SerializeField] private UIManager.HeroesType selectedHeroesType;
    
}
