using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Tweening;

[RequireComponent(typeof(SphereCollider))]

public class ThrowableProjectile : Projectile
{
    private SphereCollider sphereCollider;
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    public virtual void Shoot(Damageable target)
    {
        this.target = target;
        ProjectileMotion projectileMotion = new ProjectileMotion(this, target);
        motion = projectileMotion;
        projectileMotion.Start();
    }
    public virtual void Shoot(Vector3 position)
    {
        ProjectileMotion projectileMotion = new ProjectileMotion(this, target);
        motion = projectileMotion;
        projectileMotion.Start();
    }
    protected class ProjectileMotion : Motion
    {
        public readonly Damageable target;
        public ProjectileMotion(ThrowableProjectile projectile, Damageable target) : base(projectile)
        {
            this.target = target;
        }
        public void Start()
        {
            projectile.transform.FaProjectileMotion(target.transform.position, 2);
        }
        public override void Update()
        {
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {

        }
            
    }
}
