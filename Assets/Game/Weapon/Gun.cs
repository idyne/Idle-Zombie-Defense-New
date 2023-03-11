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
    [SerializeField] protected float burstPeriod;


    public override void Use(Damageable target)
    {
        if (InCooldown) return;
        switch (mode)
        {
            case ShootMode.BURST:
                StartCoroutine(ShootBurst(target, burstSize, burstPeriod));
                break;
            case ShootMode.SINGLE:
                ShootSingle(target);
                break;
        }
        lastUseTime = Time.time;
    }

    protected virtual void ShootSingle(Damageable target)
    {
        GuidedProjectile projectile = projectilePool.Get<GuidedProjectile>(muzzle.position, muzzle.rotation);
        projectile.Damage = damage;
        target.AddFutureHealth(-damage);
        projectile.Shoot(target);
    }

    protected virtual IEnumerator ShootBurst(Damageable target, int count, float period)
    {
        ShootSingle(target);
        yield return new WaitForSeconds(period);
        yield return ShootBurst(target, count - 1, period);
    }

    public enum ShootMode { BURST, SINGLE }
}
