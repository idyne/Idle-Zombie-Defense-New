using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/UpgradeBaseHealthTutorial")]
public class UpgradeBaseHealthTutorial : TutorialData
{
    public override bool IsItSuitableAndUnpassed()
    {
        return !IsPassed();
    }

    public override bool IsPassed()
    {
        return saveData.Value.UpgradeBaseHealthTutorialPassed;
    }

    public override void Pass()
    {
        saveData.Value.UpgradeBaseHealthTutorialPassed = true;
    }
}

public partial class SaveData
{
    public bool UpgradeBaseHealthTutorialPassed = true;
}
