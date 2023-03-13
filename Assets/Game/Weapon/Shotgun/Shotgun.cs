using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] protected int numberOfShots = 7;
    protected override IEnumerator ShootSingle(Damageable target)
    {
        for (int i = 0; i < numberOfShots; i++)
        {
            UnguidedProjectile projectile = projectilePool.Get<UnguidedProjectile>(muzzle.position, muzzle.rotation);
            projectile.Damage = damage;
            //target.AddFutureHealth(-damage);
            Vector3 difference = target.ShotPoint.position - muzzle.position;
            Vector3 direction = RandomPointOnPlane(muzzle.position + difference.normalized * 5, -difference, 0.5f) - muzzle.position;
            //Debug.DrawRay(muzzle.position, difference, Color.red, 0.5f);
            projectile.Shoot(direction);
        }
        yield return null;
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
