using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FateGames.Tweening;

public class CircleCameraController : CameraController
{
    private bool zoomingOut = false;

    [SerializeField] private float cameraForwardRange = 10;
    [SerializeField] private float cameraBackwardRange = -10;
    [SerializeField] private float cameraZoomSpeed = 10;
    private Vector3 direction;
    private Vector3 initialCameraPosition;

    private float zoomDistance = 0;
    private float anchorDistance = 0;
    private bool onUI = false;

    private Swerve swerve;
    private float anchorAngle = 0;
    private float angle = 270;

    protected override void Awake()
    {
        base.Awake();
        Init();
        swerve = InputManager.GetSwerve(Screen.height);
        swerve.OnStart.AddListener(SetAnchorAngle);
        swerve.OnStart.AddListener(SetAnchorDistance);
        swerve.OnStart.AddListener(() => { onUI = EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null; });
        swerve.OnSwerve.AddListener(() =>
        {
            if (zoomingOut || onUI) return;
            angle = anchorAngle + swerve.XRate * 180;
            zoomDistance = Mathf.Clamp(anchorDistance - swerve.YRate * cameraZoomSpeed, cameraBackwardRange, cameraForwardRange);
            //DayCycler.Instance.ChangeFogOffset(-zoomDistance);
            cameraTransform.localPosition = initialCameraPosition + direction * zoomDistance;
            transform.rotation = Quaternion.LookRotation(Quaternion.Euler(0, angle, 0) * Vector3.right);
        });
    }

    private void Start()
    {
        if (cameraTransform != null)
        {
            zoomDistance = Mathf.Clamp(anchorDistance - swerve.YRate * cameraZoomSpeed, cameraBackwardRange, cameraForwardRange);
            //DayCycler.Instance.ChangeFogOffset(-zoomDistance);
            cameraTransform.localPosition = initialCameraPosition + direction * zoomDistance;
        }
            
    }

    private void SetAnchorAngle()
    {
        anchorAngle = angle;
    }

    private void SetAnchorDistance()
    {
        anchorDistance = zoomDistance;
    }


    public override void ZoomOut()
    {
        zoomingOut = true;
        FaTween.To(() => cameraTransform.position, x => cameraTransform.position = x, cameraTransform.position - cameraTransform.forward * 5, 3).OnComplete(() => zoomingOut = false); ;
    }

    public override void ZoomIn()
    {
        zoomingOut = true;
        FaTween.To(() => cameraTransform.position, x => cameraTransform.position = x, cameraTransform.position + cameraTransform.forward * 5, 3).OnComplete(() => zoomingOut = false);
    }

    private void Init()
    {
        if (cameraTransform != null)
        {
            direction = cameraTransform.localRotation * Vector3.forward;
            initialCameraPosition = cameraTransform.localPosition;
        }
    }
}
