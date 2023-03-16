using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class Bullet : FateMonoBehaviour, IPooledObject
{
    /*
     *  USAGE
     *  
     *  1) Get the bullet from its pool
     *  2) Set damage to the bullet
     *  3) Shoot
     *  
     * 
     */
    [SerializeField] protected float speed = 40;
    [SerializeField] protected float radius = 0.5f;
    [SerializeField] protected bool piercing = false;
    private IEnumerator releaseAfterSecondsCoroutine = null;
    private int damage = 1;
    private SphereCollider sphereCollider;
#pragma warning disable CS0108 
    private Rigidbody rigidbody;
#pragma warning restore CS0108 
    private List<Damageable> hitDamageables = new();

    public event Action OnRelease;

    private void Awake()
    {
        Log("Awake", false);
        InitializeSphereCollider();
        InitializeRigidbody();
    }
    private void InitializeSphereCollider()
    {
        Log("InitializeSphereCollider", false);
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = false;
        sphereCollider.radius = radius;
    }
    private void InitializeRigidbody()
    {
        Log("InitializeRigidbody", false);
        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.useGravity = false;
        //rigidbody.isKinematic = true;
    }
    public void Shoot(Vector3 direction, int damage)
    {
        Log("Shoot", false);
        SetDamage(damage);
        StartRigidbody();
        rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
        IEnumerator releaseAfterSecondsRoutine()
        {
            yield return new WaitForSeconds(4);
            Release();
        }
        releaseAfterSecondsCoroutine = releaseAfterSecondsRoutine();
        StartCoroutine(releaseAfterSecondsCoroutine);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Log("OnCollisionEnter", false);
        Damageable damageable = collision.transform.GetComponent<Damageable>();
        if (damageable)
        {
            Hit(damageable);
            // If not piercing bullet, release bullet to the pool
            if (!piercing)
                Release();
        }
        // If hit the ground, release bullet to the pool
        else
            Release();
    }

    protected void Hit(Damageable damageable)
    {
        Log("Hit", false);
        if (hitDamageables.Contains(damageable)) return;
        damageable.Hit(damage);
        hitDamageables.Add(damageable);
    }


    protected void DisableCollider() => sphereCollider.enabled = false;
    protected void EnableCollider() => sphereCollider.enabled = true;

    public virtual void Release()
    {
        Log("Release", false);
        if (releaseAfterSecondsCoroutine != null)
            StopCoroutine(releaseAfterSecondsCoroutine);
        releaseAfterSecondsCoroutine = null;
        DisableCollider();
        Deactivate();
        StopRigidbody();
        OnRelease.Invoke();
    }

    protected void SetDamage(int damage)
    {
        Log("SetDamage", false);
        this.damage = damage;
    }

    protected void StopRigidbody()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = true;
    }

    protected void StartRigidbody()
    {
        rigidbody.isKinematic = false;
    }

    public virtual void OnObjectSpawn()
    {
        Log("OnObjectSpawn", false);
        ResetHitDamageables();
        Activate();
        EnableCollider();
    }

    private void ResetHitDamageables()
    {
        Log("ResetHitDamageables", false);
        hitDamageables = new();
    }
}
