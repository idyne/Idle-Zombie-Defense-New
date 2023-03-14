using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
[CreateAssetMenu(menuName ="Zombie/Zombie Level Data")]
public class ZombieLevelData : ScriptableObject
{
    public float Cooldown = 2;
    public Color Color;
    public int MaxHealth = 50;
    public float Speed = 1;
    public int Damage = 5;
    public float Scale = 1;
}
