using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class UnguidedProjectile : Projectile
{
    private IEnumerator releaseAfterSecondRoutine = null;
    private SphereCollider sphereCollider;
    private void Awake()
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
        releaseAfterSecondRoutine = ReleaseAfterSeconds(5);
        StartCoroutine(releaseAfterSecondRoutine);
    }

    private IEnumerator ReleaseAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Release();
        releaseAfterSecondRoutine = null;
    }

    public override void OnReached()
    {
        if (releaseAfterSecondRoutine != null)
        {
            StopCoroutine(releaseAfterSecondRoutine);
            releaseAfterSecondRoutine = null;
        }
        base.OnReached();
    }

    protected class UnguidedMotion : Motion
    {

        public readonly Vector3 direction;
        public UnguidedMotion(Projectile projectile, Vector3 direction) : base(projectile)
        {
            this.direction = direction.normalized;
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
        if (other.CompareTag("Zombie"))
            target = other.GetComponent<Damageable>();
        OnReached();
    }
}
