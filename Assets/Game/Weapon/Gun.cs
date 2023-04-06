using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class Gun : FateMonoBehaviour
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected ObjectPool bulletPool;
    [SerializeField] protected Transform[] muzzles;
    [SerializeField] protected SoundEntity shootSound;
    [SerializeField] protected SoundManager soundManager;
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
        bullet.Shoot(direction, damage);
        shotCount++;
        soundManager.PlaySound(shootSound, Muzzle.position);
    }


}
