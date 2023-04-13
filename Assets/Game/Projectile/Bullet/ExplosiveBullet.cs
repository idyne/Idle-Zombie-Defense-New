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
    [SerializeField] private Transform trailTransform;
    [SerializeField] private GameObject meshObject;
    [SerializeField] private SoundEntity sound;
    [SerializeField] private SoundManager soundManager;
    private Tween delayedReleaseTween = null;

    public override void OnObjectSpawn()
    {
        Log("OnObjectSpawn", false);
        ActivateMesh();
        AttachTrail();
        base.OnObjectSpawn();
    }

    protected override void OnReached()
    {
        Log("OnReached", false);
        int maxColliders = 1;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, damageableLayerMask);
        if (numColliders > 0)
        {
            Explode();
        }
        else
        {
            ShootToDamageable();
        }
    }

    protected override void ShootToGround()
    {
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 100, groundLayerMask))
        {
            float time = hit.distance / speed;
            transform.DOMove(hit.point, time).OnComplete(Explode);
        }
        else
        {
            Explode();
        }
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
        soundManager.PlaySound(sound, transform.position);
        DeactivateMesh();
        DetachTrail();
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, areaOfEffectRadius, hitColliders, damageableLayerMask);
        if (numColliders > 0)
        {
            for (int i = 0; i < numColliders; i++)
            {
                Damageable damageable = hitColliders[i].GetComponent<Damageable>();
                Hit(damageable, damage / numColliders, false);
            }

        }
        // TODO change here when implemented the PooledEffect
        if (visualEffectPool)
            visualEffectPool.Get<Transform>(transform.position, Quaternion.identity);
        delayedReleaseTween = DOVirtual.DelayedCall(1, Release);
    }

}
