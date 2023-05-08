using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class FireRateBoostButton : MonoBehaviour
{
    [SerializeField] private UIElement uiElement;
    public void Show()
    {
        if (!FireRateBoostPanel.isShown) return;
        uiElement.Show();
    }
    public void Hide()
    {

        uiElement.Hide();
    }
}
