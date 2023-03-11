using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class UnguidedProjectile : Projectile
{
    private SphereCollider sphereCollider;
    private void OnEnable()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    public virtual void Shoot(Vector3 direction)
    {
        motion = new UnguidedMotion(this, direction);
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        sphereCollider.enabled = true;
    }

    protected class UnguidedMotion : Motion
    {

        public readonly Vector3 direction;
        public UnguidedMotion(Projectile projectile, Vector3 direction) : base(projectile)
        {
            this.direction = direction;
        }
        public override void Update()
        {
            Vector3 from = projectile.transform.position;
            Vector3 to = from + projectile.Speed * Time.deltaTime * direction;
            projectile.Move(Vector3.MoveTowards(from, to, Time.deltaTime * projectile.Speed));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        sphereCollider.enabled = false;
        target = other.GetComponent<Damageable>();
        OnReached();
    }
}
