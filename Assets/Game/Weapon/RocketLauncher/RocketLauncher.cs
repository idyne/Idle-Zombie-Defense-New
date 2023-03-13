using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Gun
{
    [SerializeField] private List<Transform> muzzles;

    protected void ShootSingle(Damageable target, Transform muzzle)
    {
        UnguidedProjectile projectile = projectilePool.Get<UnguidedProjectile>(muzzle.position, muzzle.rotation);
        projectile.Damage = damage;
        //target.AddFutureHealth(-damage);
        projectile.Shoot(target.ShotPoint.position - transform.position);
    }
    protected override IEnumerator ShootBurst(Damageable target, int count)
    {
        if (count <= 0)
        {
            yield break;
        }
        ShootSingle(target, muzzles[(count - 1) % muzzles.Count]);
        yield return waitForBurstCooldown;
        yield return ShootBurst(target, count - 1);
    }
}
