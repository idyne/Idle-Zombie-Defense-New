using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class MergeButton : UIElement
{
    [SerializeField] private SaveDataVariable saveData;

    private void OnEnable()
    {
        Hide();
    }

    public override void Show()
    {
        UpdateButton();
        base.Show();
    }

    public void UpdateButton()
    {

    }


}
