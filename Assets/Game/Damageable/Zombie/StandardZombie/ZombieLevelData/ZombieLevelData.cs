using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
[CreateAssetMenu(menuName ="Zombie/Zombie Level Data")]
public class ZombieLevelData : ScriptableObject
{
    public int Level = 1;
    public float Cooldown = 2;
    public Color Color;
    public float Speed = 1;
    public float Scale = 1;
    public int MaxHealth => Level * BaseMaxHealth;
    public int Damage => Level * BaseDamage;
    public int Money => Level * BaseMoney;

    public int BaseMaxHealth = 50;
    public int BaseDamage = 50;
    public int BaseMoney = 1;
}
