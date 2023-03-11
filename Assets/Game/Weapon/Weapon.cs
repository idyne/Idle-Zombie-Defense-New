using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float maxCooldown;
    protected float cooldown;
    protected float lastUseTime = float.MinValue;
    public bool InCooldown { get => Time.time < lastUseTime + cooldown; }

    private void OnEnable()
    {
        ResetCooldown();
    }
    public abstract void Use(Damageable target);

    public void ResetCooldown()
    {
        cooldown = maxCooldown;
    }
}
