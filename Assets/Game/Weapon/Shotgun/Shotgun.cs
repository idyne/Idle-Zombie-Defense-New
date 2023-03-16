using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] protected int shotSize = 7;

    public override void Shoot(Damageable target)
    {
        for (int i = 0; i < shotSize; i++)
        {
            Vector3 difference = target.ShotPoint.position - Muzzle.position;
            Vector3 shootDirection = RandomPointOnPlane(target.ShotPoint.position, -difference, 0.5f) - Muzzle.position;
            Bullet bullet = bulletPool.Get<Bullet>(Muzzle.position, Quaternion.LookRotation(shootDirection));
            bullet.Shoot(shootDirection, damage);
        }
        shotCount++;
    }

    private Vector3 RandomPointOnPlane(Vector3 position, Vector3 normal, float radius)
    {
        Vector3 randomPoint;

        do
        {
            randomPoint = Vector3.Cross(Random.insideUnitSphere, normal);
        } while (randomPoint == Vector3.zero);

        randomPoint.Normalize();
        randomPoint *= radius;
        randomPoint += position;

        return randomPoint;
    }
}
