using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using FateGames.Tweening;
using System;
using DG.Tweening;

public class Soldier : Shooter, IPooledObject
{
    [SerializeField] protected SoldierSet soldierSet;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform ragdollTransform;
    [SerializeField] protected Transform hipsTransform;
    [SerializeField] protected FloatVariable rewindDuration;
    private Rigidbody[] rigidbodies;
    public event Action OnRelease;
    private Vector3 positionBeforeDeath;
    private Quaternion rotationBeforeDeath;

    protected override void Awake()
    {
        base.Awake();
        rigidbodies = ragdollTransform.GetComponentsInChildren<Rigidbody>();
    }

    protected void OnEnable()
    {
        Log("OnEnable", false);
        soldierSet.Add(this);
    }

    protected virtual void OnDisable()
    {
        Log("OnDisable", false);
        soldierSet.Remove(this);
    }

    public void SetPosition(Vector3 position)
    {
        Log("SetPosition", false);
        transform.position = position;
    }
    public void OnObjectSpawn()
    {
        Log("OnObjectSpawn", false);
        Activate();
        towerDPS.Register(this);
        if (waveState.Value == WaveController.WaveState.STARTED)
            StartTargeting();
    }

    public void Release()
    {
        Log("Release", false);
        if (Targeting)
            StopTargeting();
        if (target)
            RemoveTarget();
        Deactivate();
        towerDPS.Unregister(this);
        OnRelease.Invoke();
    }

    public void ActivateRagdoll()
    {
        if (ragdollTransform)
        {
            positionBeforeDeath = transform.position;
            rotationBeforeDeath = transform.rotation;
            animator.enabled = false;
            //ragdollTransform.SetParent(null);
            ragdollTransform.gameObject.SetActive(true);
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(5, Vector3.zero, 10, 1, ForceMode.Impulse);
            }
        }
    }
    public void Rewind()
    {
        hipsTransform.SetParent(null);
        ragdollTransform.position = hipsTransform.position;
        hipsTransform.SetParent(ragdollTransform);
        foreach (Rigidbody rb in rigidbodies)
            rb.isKinematic = true;
        animator.enabled = true;
        animator.SetTrigger("Rewind");
        ragdollTransform.FaProjectileMotion(transform.position, rewindDuration.Value);
        ragdollTransform.DORotateQuaternion(transform.rotation, rewindDuration.Value);
        DOVirtual.DelayedCall(2, () =>
        {
            //ragdollTransform.transform.SetParent(transform);
            //ragdollTransform.gameObject.SetActive(false);
        });
    }


    public override void Shoot()
    {
        animator.SetTrigger("Shoot");
        base.Shoot();
    }

    public override void Face(Vector3 to)
    {
        // Called in Update()
        Vector3 direction = to - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 3f * shootFrequencyMultiplier.Value);
    }

    public virtual void Die()
    {
        Log("Die", false);
        if (Targeting)
            StopTargeting();
        if (target)
            RemoveTarget();
        ActivateRagdoll();
    }
}
