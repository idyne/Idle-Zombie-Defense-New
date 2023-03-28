using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class MergeButton : DynamicUIElement
{
    [SerializeField] private SaveDataVariable saveData;

    private void OnEnable()
    {
        Hide();
    }

    public override void Show()
    {
        UpdateElement();
        base.Show();
    }

    public override void UpdateElement()
    {
        
    }
}
