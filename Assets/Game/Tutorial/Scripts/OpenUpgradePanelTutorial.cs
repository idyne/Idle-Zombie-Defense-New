using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/OpenUpgradePanelTutorial")]
public class OpenUpgradePanelTutorial : TutorialData
{
    [SerializeField] private ZoneManager zoneManager;

    public override bool IsItSuitableAndUnpassed()
    {
        return zoneManager.WaveLevel == 2 && !IsPassed();
    }

    public override bool IsPassed()
    {
        return saveData.Value.OpenUpgradePanelTutorialPassed;
    }

    public override void Pass()
    {
        saveData.Value.OpenUpgradePanelTutorialPassed = true;
    }
}

public partial class SaveData
{
    public bool OpenUpgradePanelTutorialPassed = false;
}
