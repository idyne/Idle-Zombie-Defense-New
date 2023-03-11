using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedProjectile : Projectile
{
    public virtual void Shoot(Damageable target)
    {
        this.target = target;
        motion = new GuidedMotion(this, target);
    }

    protected class GuidedMotion : Motion
    {
        public readonly Damageable target;
        public GuidedMotion(GuidedProjectile projectile, Damageable target) : base(projectile)
        {
            this.target = target;
        }
        public override void Update()
        {
            Vector3 from = projectile.transform.position;
            Vector3 to = target.ShotPoint.position;
            projectile.Move(Vector3.MoveTowards(from, to, Time.deltaTime * projectile.Speed));
            if (Vector3.Distance(projectile.transform.position, target.ShotPoint.position) <= 0.1f)
                projectile.OnReached();
        }
    }


}
