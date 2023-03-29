using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostMine : Mine
{
    [SerializeField] private float duration = 5;
    protected override void OnExplosion(Zombie zombie)
    {
        zombie.Freeze(duration);
    }
}
