using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

[CreateAssetMenu(menuName = "Wave/Wave State Variable")]
public class WaveStateVariable : Variable<WaveController.WaveState>
{
    private void OnEnable()
    {
        Value = WaveController.WaveState.NONE;
    }
}


