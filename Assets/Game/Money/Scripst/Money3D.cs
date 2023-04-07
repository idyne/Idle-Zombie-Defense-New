using FateGames.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Money3D : FateMonoBehaviour, IPooledObject
{
    [SerializeField] private Money3DRuntimeSet runtimeSet;
    private Rigidbody rb;
    [SerializeField] private ObjectPool money2DPool;
    public event Action OnRelease;
    [HideInInspector] public Action OnFinish;

    private WaitForSeconds waitForSeconds;
    private IEnumerator waitOnFloorRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        waitForSeconds = new WaitForSeconds(2f);
    }

    public void OnObjectSpawn()
    {
        runtimeSet.Add(this);
        rb.isKinematic = false;
        Activate();
        waitOnFloorRoutine = WaitAndTurnToUI();
        StartCoroutine(waitOnFloorRoutine);
    }

    public void Release()
    {
        rb.isKinematic = true;
        Deactivate();
        runtimeSet.Remove(this);
        OnRelease();
    }

    private IEnumerator WaitAndTurnToUI()
    {
        yield return waitForSeconds;
        money2DPool.Get<Money2D>(Vector3.zero, Quaternion.identity).DirectGoToUI(Camera.main.WorldToScreenPoint(transform.position), OnFinish);
        Release();
    }

    public void Finish()
    {
        CancelWait();
        OnFinish();
        Release();
    }

    private void CancelWait()
    {
        if (waitOnFloorRoutine == null) return;
        StopCoroutine(waitOnFloorRoutine);
        waitOnFloorRoutine = null;

    }

    private void OnDisable()
    {
        runtimeSet.Remove(this);
    }

}
