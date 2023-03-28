using FateGames.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Money3D : FateMonoBehaviour, IPooledObject
{
    private Rigidbody rb;
    [HideInInspector] public int Gain = 0;
    [SerializeField] private ObjectPool money2DPool;

    public event Action OnRelease;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnObjectSpawn()
    {
        rb.isKinematic = false;
        Activate();
        StartCoroutine(WaitAndTurnToUI());
    }

    public void Release()
    {
        rb.isKinematic = true;
        Deactivate();
        OnRelease();
    }

    private IEnumerator WaitAndTurnToUI()
    {
        yield return new WaitForSeconds(2f);
        money2DPool.Get<Money2D>(Vector3.zero, Quaternion.identity).DirectGoToUI(Camera.main.WorldToScreenPoint(transform.position), Gain);
        Release();
    }


}
