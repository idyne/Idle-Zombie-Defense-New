using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FateGames.Core;

public class Barrier : DamageableStructure
{
#pragma warning disable CS0108
    [SerializeField] private Collider collider;
#pragma warning restore CS0108 
    [SerializeField] private Transform[] parts;
    [SerializeField] private ObjectPool effectPool;

    private void Awake()
    {
        onSetHealth += CheckParts;
    }

    private void CheckParts()
    {
        float percent = (float)health / maxHealth;
        for (int i = 0; i < parts.Length; i++)
        {
            if (percent <= 1 - (1f / parts.Length * (i + 1)))
            {
                Transform part = parts[i];
                part.DOScale(Vector3.zero, 0.2f);
                effectPool.Get<Transform>(part.position, part.rotation);
            }
            else
            {
                break;
            }
        }
    }
    private void ActivateCollider() => collider.enabled = true;
    private void DeactivateCollider() => collider.enabled = false;

    public override void Die()
    {
        //Deactivate();
        DeactivateCollider();
    }
    public override void Repair()
    {
        SetHealth(maxHealth);
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].DOScale(Vector3.one, 0.4f);
        }
        ActivateCollider();
    }
}
