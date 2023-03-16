using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;
using DG.Tweening;

public class ExplosiveBullet : Bullet, IPooledObject
{
    [SerializeField] private ObjectPool visualEffectPool;
    [SerializeField] private float areaOfEffectRadius = 2.5f;
    [SerializeField] private LayerMask damageableLayerMask;
    [SerializeField] private Transform trailTransform;
    [SerializeField] private GameObject meshObject;
    private Tween delayedReleaseTween = null;

    protected override void OnCollisionEnter(Collision collision)
    {
        Log("OnCollisionEnter", false);
        StopRigidbody();
        DisableCollider();
        Explode();
        delayedReleaseTween = DOVirtual.DelayedCall(1, Release);
        //Release();
    }

    public override void OnObjectSpawn()
    {
        Log("OnObjectSpawn", false);
        ActivateMesh();
        AttachTrail();
        base.OnObjectSpawn();
    }

    public override void Release()
    {
        if (delayedReleaseTween != null)
        {
            delayedReleaseTween.Kill();
            delayedReleaseTween = null;
        }
        base.Release();
    }

    private void ActivateMesh() => meshObject.SetActive(true);
    private void DeactivateMesh() => meshObject.SetActive(false);

    private void AttachTrail()
    {
        Log("AttachTrail", false);
        trailTransform.SetParent(transform);
        trailTransform.localPosition = Vector3.zero;
    }
    private void DetachTrail()
    {
        Log("Explode", false);
    }

    private void Explode()
    {
        Log("Explode", false);
        DeactivateMesh();
        DetachTrail();
        int maxColliders = 30;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, areaOfEffectRadius, hitColliders, damageableLayerMask);
        if (numColliders > 0)
        {
            for (int i = 0; i < numColliders; i++)
            {
                Damageable damageable = hitColliders[i].GetComponent<Damageable>();
                Hit(damageable);
            }
            // TODO change here when implemented the PooledEffect
            if (visualEffectPool)
                visualEffectPool.Get<Transform>(transform.position, Quaternion.identity);
        }
    }

}
