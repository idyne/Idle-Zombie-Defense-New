using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/SoldierBuyingTutorial")]
public class SoldierBuyingTutorial : TutorialData
{
    public override bool IsItSuitableAndUnpassed()
    {
        return !IsPassed();
    }

    public override bool IsPassed()
    {
        return saveData.Value.SoldierBuyingTutorialPassed;
    }

    public override void Pass()
    {
        saveData.Value.SoldierBuyingTutorialPassed = true;
    }
}

public partial class SaveData 
{
    public bool SoldierBuyingTutorialPassed = false;
}
