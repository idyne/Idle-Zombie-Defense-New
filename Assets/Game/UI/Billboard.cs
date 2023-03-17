using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class Billboard : FateMonoBehaviour
{
    [SerializeField] private BillboardRuntimeSet set;
    private void OnEnable()
    {
        set.Add(this);
    }
    private void OnDisable()
    {
        set.Remove(this);
    }

    public void LookAt(Transform cameraTransform)
    {
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.back, cameraTransform.rotation * Vector3.up);
    }
}
