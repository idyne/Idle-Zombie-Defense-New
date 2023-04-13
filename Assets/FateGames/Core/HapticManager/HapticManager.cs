using Lofelt.NiceVibrations;
using UnityEngine;
namespace FateGames.Core
{
    public static class HapticManager
    {
        public static void PlayHaptic(HapticPatterns.PresetType presetType = HapticPatterns.PresetType.LightImpact)
        {
#if UNITY_EDITOR
            Debug.Log("Haptic played: " + presetType);
#endif
            HapticPatterns.PlayPreset(presetType);
        }

    }
}