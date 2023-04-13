using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
[CreateAssetMenu(menuName = "Zombie/Zombie Level Data")]
public class ZombieLevelData : ScriptableObject
{
    public float Cooldown = 2;
    public Color Color;
    public float Speed = 1;
    public float Scale = 1;


    public IntReference BaseMaxHealth;
    public IntReference BaseDamage;
    public IntReference BaseMoney;
}
