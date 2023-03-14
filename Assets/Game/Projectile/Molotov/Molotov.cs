using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : ThrowableProjectile
{
    [SerializeField] private GameObject mesh;
    [SerializeField] private LayerMask enemyLayerMask;
    protected List<Zombie> cooldownList = new();
    protected int damagePerSecond { get => 5; }
    [SerializeField] protected float duration = 5f;
    [SerializeField] protected float hitPeriod = 0.5f;
    [SerializeField] protected float radius = 2.5f;
    protected WaitForSeconds waitForHitPeriod;

    private void Awake()
    {
        waitForHitPeriod = new(hitPeriod);
    }

    public override void OnReached()
    {
        DeactivateMesh();
        IEnumerator burn(float remainingSeconds)
        {
            if (remainingSeconds <= 0)
            {
                Release();
                yield break;
            }

            int maxColliders = 30;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, enemyLayerMask);
            if (numColliders > 0)
            {
                for (int i = 0; i < numColliders; i++)
                {
                    Damageable damageable = hitColliders[i].GetComponent<Damageable>();
                    damageable.Hit(damagePerSecond);
                }
            }
            yield return waitForHitPeriod;
            yield return burn(remainingSeconds - hitPeriod);
        }
        StartCoroutine(burn(duration));
        //base.OnReached();
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        ActivateMesh();
    }

    private void DeactivateMesh() => mesh.SetActive(false);
    private void ActivateMesh() => mesh.SetActive(true);
}
