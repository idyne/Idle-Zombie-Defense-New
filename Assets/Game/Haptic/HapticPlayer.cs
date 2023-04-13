using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

[CreateAssetMenu(menuName = "Haptic Player")]
public class HapticPlayer : ScriptableObject
{
    [SerializeField] private BoolReference hapticOn;
    public void PlayHaptic(Lofelt.NiceVibrations.HapticPatterns.PresetType presetType)
    {
        if (hapticOn.Value)
            HapticManager.PlayHaptic(presetType);
    }
    public void PlayLightHaptic()
    {
        PlayHaptic(Lofelt.NiceVibrations.HapticPatterns.PresetType.LightImpact);
    }
    public void PlaySuccessHaptic()
    {
        PlayHaptic(Lofelt.NiceVibrations.HapticPatterns.PresetType.Success);
    }
    public void PlayFailHaptic()
    {
        PlayHaptic(Lofelt.NiceVibrations.HapticPatterns.PresetType.Failure);
    }
}
