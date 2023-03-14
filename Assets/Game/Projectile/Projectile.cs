using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;

public abstract class Projectile : FateMonoBehaviour, IPooledObject
{
    [SerializeField] protected bool areaOfEffect = false;
    [SerializeField] protected float areaRadius = 1;
    [SerializeField] protected float speed = 40;
    [SerializeField] protected ObjectPool vfxPool;
    [SerializeField] protected TrailRenderer trailRenderer;
    [SerializeField] protected LayerMask areaOfEffectLayerMask;

    protected Motion motion;
    [HideInInspector] public int Damage = 1;
    public Action<Damageable> specialEffect;
    protected Damageable target;
    public bool AddToTargetFutureHealth = false;

    public float Speed { get => speed; }
    public float AreaRadius { get => areaRadius; }

    public event Action OnRelease;

    public virtual void OnObjectSpawn()
    {
        specialEffect = null;
        Damage = 1;
        Activate();
    }

    public override void Activate()
    {
        base.Activate();
        if (trailRenderer)
            trailRenderer.emitting = true;
    }
    public override void Deactivate()
    {
        base.Deactivate();
        if (trailRenderer)
            trailRenderer.emitting = false;
    }
    public virtual void Release()
    {
        Deactivate();
        OnRelease.Invoke();
    }

    private void Update()
    {
        motion?.Update();
    }

    public virtual void OnReached()
    {
        Hit();
        Release();
    }

    public void Hit()
    {
        if (areaOfEffect) HitArea();
        else HitTarget();
        if (vfxPool)
        {
            Transform vfx = vfxPool.Get<Transform>(transform.position, Quaternion.identity);
        }
    }

    public void HitArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, areaRadius, areaOfEffectLayerMask);
        foreach (Collider collider in colliders)
        {
            Damageable damageable = collider.GetComponent<Damageable>();
            damageable.Hit(Damage, AddToTargetFutureHealth);
        }
    }

    public void HitTarget()
    {
        if (!target) return;
        target.Hit(Damage, AddToTargetFutureHealth);
    }

    public void Move(Vector3 to)
    {
        transform.position = to;
    }

    protected abstract class Motion
    {
        public readonly Projectile projectile;

        public Motion(Projectile projectile)
        {
            this.projectile = projectile;
        }
        public abstract void Update();

    }

}
