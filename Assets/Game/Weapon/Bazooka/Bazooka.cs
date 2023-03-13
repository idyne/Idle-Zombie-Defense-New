using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Gun
{
    protected override IEnumerator ShootSingle(Damageable target)
    {
        GuidedProjectile projectile = projectilePool.Get<GuidedProjectile>(muzzle.position, muzzle.rotation);
        projectile.Damage = damage;
        //target.AddFutureHealth(-damage);
        projectile.Shoot(target);
        yield return null;
    }
}
