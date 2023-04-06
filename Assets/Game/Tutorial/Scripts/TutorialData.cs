using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialData : ScriptableObject
{
    [SerializeField] protected SaveDataVariable saveData;

    public abstract bool IsItSuitableAndUnpassed();

    public abstract bool IsPassed();

    public abstract void Pass();
}
