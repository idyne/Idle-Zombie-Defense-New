using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class Gun : FateMonoBehaviour
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected ObjectPool bulletPool;
    [SerializeField] protected Transform[] muzzles;
    [SerializeField] private ObjectPool fireRateEffect;
    [SerializeField] protected ObjectPool muzzleFire;
    [SerializeField] protected Transform muzzleFireEffectPoint;
    protected int shotCount = 0;
    protected Transform Muzzle { get => muzzles[shotCount % muzzles.Length]; }

    public virtual void Shoot(Damageable target)
    {
        Vector3 shootDirection = target.ShotPoint.position - Muzzle.position;

        ShootTo(shootDirection);
    }

    public virtual void ShootTo(Vector3 direction)
    {
        Bullet bullet = bulletPool.Get<Bullet>(Muzzle.position, Quaternion.LookRotation(direction));
        if (muzzleFire)
            muzzleFire.Get<Transform>(muzzleFireEffectPoint.position, muzzleFireEffectPoint.rotation);
        bullet.Shoot(direction, damage);
        shotCount++;
    }

    public void ShowFireRateEffect()
    {
        fireRateEffect.Get<Transform>(Muzzle.position, Quaternion.identity);
    }


}
