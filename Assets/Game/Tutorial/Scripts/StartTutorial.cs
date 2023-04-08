using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/StartTutorial")]
public class StartTutorial : TutorialData
{
    [SerializeField] private ZoneManager zoneManager;

    public override bool IsItSuitableAndUnpassed()
    {
        return zoneManager.WaveLevel == 1 && !IsPassed();
    }

    public override bool IsPassed()
    {
        return saveData.Value.StartTutorialPassed;
    }

    public override void Pass()
    {
        saveData.Value.StartTutorialPassed = true;
    }
}

public partial class SaveData
{
    public bool StartTutorialPassed = true;
}
