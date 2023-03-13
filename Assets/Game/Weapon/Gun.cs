using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class Gun : Weapon
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected ObjectPool projectilePool;
    [SerializeField] protected ShootMode mode = ShootMode.SINGLE;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected int burstSize;
    [SerializeField] protected float burstCooldown = 0.1f;
    protected WaitForSeconds waitForBurstCooldown;
    public ShootMode Mode { get => mode; }

    protected override void OnEnable()
    {
        base.OnEnable();
        waitForBurstCooldown = new WaitForSeconds(burstCooldown);
    }

    public override IEnumerator Use(Damageable target)
    {
        IEnumerator use()
        {
            if (InCooldown) yield break;
            StartCoroutine(Cooldown());
            switch (mode)
            {
                case ShootMode.BURST:
                    yield return ShootBurst(target, burstSize);
                    break;
                case ShootMode.SINGLE:
                    yield return ShootSingle(target);
                    break;
            }
        }
        useRoutine = use();
        yield return useRoutine;
    }

    protected virtual IEnumerator ShootSingle(Damageable target)
    {
        GuidedProjectile projectile = projectilePool.Get<GuidedProjectile>(muzzle.position, muzzle.rotation);
        projectile.Damage = damage;
        target.AddFutureHealth(-damage);
        projectile.Shoot(target);
        yield return null;
    }

    protected virtual IEnumerator ShootBurst(Damageable target, int count)
    {
        if (count <= 0)
        {
            yield break;
        }
        yield return ShootSingle(target);
        yield return waitForBurstCooldown;
        yield return ShootBurst(target, count - 1);
    }

    

    public enum ShootMode { BURST, SINGLE }
}
