using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FateGames.Core;
using UnityEngine.AI;
using UnityEngine.Events;

public class Barrier : DamageableStructure
{
#pragma warning disable CS0108
    [SerializeField] private Collider collider;
#pragma warning restore CS0108 
    [SerializeField] private NavMeshObstacle obstacle;
    [SerializeField] private Transform[] parts;
    [SerializeField] private ObjectPool effectPool;
    [SerializeField] private UnityEvent onDestroyed;
    private int breakLevel = 0;
    private void Awake()
    {
        onSetHealth += CheckParts;
    }

    private void CheckParts()
    {
        int breakLevel = 0;
        float percent = (float)health / maxHealth;
        for (int i = 0; i < parts.Length; i++)
        {
            if (percent <= 1 - (1f / parts.Length * (i + 1)))
            {
                breakLevel++;
                if (breakLevel <= this.breakLevel) continue;
                this.breakLevel = breakLevel;
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
    private void ActivateCollider()
    {
        collider.enabled = true;
        obstacle.enabled = true;
    }
    private void DeactivateCollider()
    {
        collider.enabled = false;
        obstacle.enabled = false;
    }

    public override void Die()
    {
        //Deactivate();
        DeactivateCollider();
        onDestroyed.Invoke();
    }
    public override void Repair()
    {
        breakLevel = 0;
        SetHealth(maxHealth);
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].DOScale(Vector3.one, 0.4f);
        }
        ActivateCollider();
    }
}
