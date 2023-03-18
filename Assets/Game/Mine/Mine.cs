using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Mine : FateMonoBehaviour
{
    [SerializeField] private float detonationDelay = 2;
    [SerializeField] private float radius = 1;
    [SerializeField] private ObjectPool visualEffectPool;
    [SerializeField] private float areaOfEffectRadius = 2.5f;
    [SerializeField] private LayerMask damageableLayerMask;
    [SerializeField] private GameObject meshObject, detonatedMeshObject;
    private SphereCollider sphereCollider = null;
#pragma warning disable CS0108 
    private Rigidbody rigidbody = null;
#pragma warning restore CS0108
    private void Awake()
    {
        InitializeRigidbody();
        InitializeSphereCollider();
        SwitchMeshes(false);
    }
    private void InitializeRigidbody()
    {
        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }
    private void InitializeSphereCollider()
    {
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = radius;
    }
    public virtual void Detonate()
    {
        SwitchMeshes(true);
        sphereCollider.enabled = false;
        DOVirtual.DelayedCall(detonationDelay, Explode);
    }
    public virtual void Renew()
    {
        SwitchMeshes(false);
        sphereCollider.enabled = true;
    }
    public void Explode()
    {
        int maxColliders = 30;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, areaOfEffectRadius, hitColliders, damageableLayerMask);
        if (numColliders > 0)
        {
            for (int i = 0; i < numColliders; i++)
            {
                Zombie zombie = hitColliders[i].GetComponent<Zombie>();
                OnExplosion(zombie);
            }

        }
        // TODO change here when implemented the PooledEffect
        if (visualEffectPool)
            visualEffectPool.Get<Transform>(transform.position, Quaternion.identity);
    }

    protected abstract void OnExplosion(Zombie zombie);

    private void OnTriggerEnter(Collider other)
    {
        Detonate();
    }
    private void SwitchMeshes(bool detonated)
    {
        meshObject.SetActive(!detonated);
        detonatedMeshObject.SetActive(detonated);
    }
}
