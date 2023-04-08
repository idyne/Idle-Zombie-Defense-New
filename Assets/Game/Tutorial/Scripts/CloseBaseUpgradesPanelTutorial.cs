using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/CloseBaseUpgradesPanelTutorial")]
public class CloseBaseUpgradesPanelTutorial : TutorialData
{
    public override bool IsItSuitableAndUnpassed()
    {
        return !IsPassed();
    }

    public override bool IsPassed()
    {
        return saveData.Value.CloseBaseUpgradesPanelTutorialPassed;
    }

    public override void Pass()
    {
        saveData.Value.CloseBaseUpgradesPanelTutorialPassed = true;
    }
}

public partial class SaveData
{
    public bool CloseBaseUpgradesPanelTutorialPassed = true;
}
