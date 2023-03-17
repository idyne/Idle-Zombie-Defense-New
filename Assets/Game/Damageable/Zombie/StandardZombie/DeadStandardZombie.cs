using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using DG.Tweening;

public class DeadStandardZombie : FateMonoBehaviour, IPooledObject
{
#pragma warning disable CS0108 
    [SerializeField] private Animation animation;
#pragma warning restore CS0108 
    private Rigidbody[] rigidbodies;

    public event System.Action OnRelease;

    private void Awake()
    {
        InitializeRigidbodies();
    }

    private void InitializeRigidbodies()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void ActivateRigidbodies()
    {
        foreach (Rigidbody rigidbody in rigidbodies)
            rigidbody.isKinematic = false;
    }

    private void DeactivateRigidbodies()
    {
        foreach (Rigidbody rigidbody in rigidbodies)
            rigidbody.isKinematic = true;
    }

    public void Initialize(ZombieLevelData data, Transform transform)
    {
        this.transform.localScale = Vector3.one * data.Scale;
        this.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }



    public void Animate()
    {
        animation.Play("DeadStandardZombiePose");
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddForce(5 * (transform.position.normalized * Random.Range(0.2f, 0.8f) + Vector3.up * Random.Range(0.5f, 1f)), ForceMode.VelocityChange);

        }
        //ActivateRigidbodies();
        DOVirtual.DelayedCall(2, Release);
    }

    public void OnObjectSpawn()
    {
        Activate();
        //DeactivateRigidbodies();
        //Animate();
    }

    public void Release()
    {
        Deactivate();
        OnRelease.Invoke();
    }
}
