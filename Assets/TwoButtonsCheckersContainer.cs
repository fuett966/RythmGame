using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoButtonsCheckersContainer : MonoBehaviour
{
    [SerializeField] private List<TwoButtonsChecker> _checkers;
    public List<TwoButtonsChecker> Checkers => _checkers;

    [SerializeField] private UIManager.HeroesType selectedHeroesType;
    public UIManager.HeroesType SelectedHeroesType => selectedHeroesType;

    public void UpdateTypeHeroes(UIManager.HeroesType type)
    {
        GameManager.instance._heroesType = type;
        selectedHeroesType = type;


        foreach (TwoButtonsChecker twoButtonsChecker in _checkers)
        {
            if (twoButtonsChecker.type == type)
            {
                continue;
            }
            twoButtonsChecker.IsEnabled = false;
        }
    }

}
