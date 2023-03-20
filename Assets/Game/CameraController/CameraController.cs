using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraController : FateMonoBehaviour
{
    [SerializeField] protected Camera targetCamera;
    protected Transform cameraTransform;

    protected virtual void Awake()
    {
        if (targetCamera != null)
            cameraTransform = targetCamera.transform;
        else
            Debug.LogError("Camera missing.");
    }

    public Transform CamTransform { get => cameraTransform; }
    public Camera Camera { get => targetCamera; }

    public abstract void ZoomOut();
    public abstract void ZoomIn();

}
