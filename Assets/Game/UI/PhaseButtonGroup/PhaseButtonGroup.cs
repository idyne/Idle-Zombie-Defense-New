using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseButtonGroup : UIElement
{
    [SerializeField] private WaveStateVariable waveState;
    public override void Show()
    {
        if (waveState.Value == WaveController.WaveState.STARTED)
            base.Show();
    }
}
