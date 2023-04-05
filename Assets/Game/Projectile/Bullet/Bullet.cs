using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using FateGames.Tweening;

public class Bullet : FateMonoBehaviour, IPooledObject
{
    /*
     *  USAGE
     *  
     *  1) Get the bullet from its pool
     *  2) Set damage to the bullet
     *  3) Shoot
     *  
     * 
     */
    [SerializeField] protected float speed = 40;
    [SerializeField] protected float radius = 0.5f;
    [SerializeField] protected LayerMask damageableLayerMask, groundLayerMask;
    private int damage = 1;
    private List<Damageable> hitDamageables = new();
    protected Vector3 direction;
    public event Action OnRelease;

    private void Awake()
    {
        Log("Awake", false);
    }

    public void Shoot(Vector3 direction, int damage)
    {
        Log("Shoot", false);
        SetDamage(damage);
        this.direction = direction;
        ShootToDamageable();
    }

    protected void ShootToDamageable()
    {
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 100, damageableLayerMask))
        {
            float time = hit.distance / speed;
            transform.FaMove(hit.point, time).OnComplete(OnReached);
        }
        else
        {
            ShootToGround();
        }
    }

    protected virtual void OnReached()
    {
        int maxColliders = 1;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, damageableLayerMask);
        if (numColliders > 0)
        {
            for (int i = 0; i < numColliders; i++)
            {
                Damageable damageable = hitColliders[i].GetComponent<Damageable>();
                Hit(damageable);
            }
        }
        else
        {
            ShootToDamageable();
        }
    }

    protected virtual void ShootToGround()
    {
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 100, groundLayerMask))
        {
            float time = hit.distance / speed;
            transform.FaMove(hit.point, time).OnComplete(Release);
        }
        else
        {
            Release();
        }
    }


    protected void Hit(Damageable damageable)
    {
        Log("Hit", false);
        if (hitDamageables.Contains(damageable)) return;
        damageable.Hit(damage);
        hitDamageables.Add(damageable);
    }

    public virtual void Release()
    {
        Log("Release", false);
        Deactivate();
        OnRelease.Invoke();
    }

    protected void SetDamage(int damage)
    {
        Log("SetDamage", false);
        this.damage = damage;
    }

    public virtual void OnObjectSpawn()
    {
        Log("OnObjectSpawn", false);
        ResetHitDamageables();
        Activate();
    }

    private void ResetHitDamageables()
    {
        Log("ResetHitDamageables", false);
        hitDamageables = new();
    }
}
