using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FateGames.Tweening;

public class CircleCameraController : CameraController
{
    private bool zoomingOut = false;
    [SerializeField] private bool lockYAxis = false;
    [SerializeField] private FloatVariable cameraDistance;
    [SerializeField] private float cameraForwardRange = 10;
    [SerializeField] private float cameraBackwardRange = -10;
    [SerializeField] private float cameraZoomSpeed = 10;
    private Vector3 direction;
    private Vector3 initialCameraPosition;
    private float lastAngle = 270;
    private float startDriftSpeed = 0;
    private bool drift = false;
    private float totalDriftTime = 0.2f, driftStartTime;

    private float zoomDistance = 0;
    private float anchorDistance = 0;
    private bool onUI = false;

    [SerializeField] private Swerve swerve;
    private float anchorAngle = 0;
    private float angle = 270;
    private bool swerving = false;
    private bool pausedOnStart = false;
    private bool pausedOnSwerve = false;
    private bool pausedOnRelease = false;

    protected override void Awake()
    {
        base.Awake();
        swerve.Size = Screen.height;
        Init();
        swerve.OnStart.AddListener(() =>
        {

            pausedOnStart = Time.timeScale < 0.1f;
            if (pausedOnStart) return;
            SetAnchorAngle(); SetAnchorDistance(); drift = false; onUI = EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null; OnSwerve();

        });
        swerve.OnSwerve.AddListener(OnSwerve);
        swerve.OnRelease.AddListener(() =>
        {
            pausedOnRelease = Time.timeScale < 0.1f;
            if (pausedOnStart || pausedOnSwerve || pausedOnRelease) return;
            if (!swerving) return;
            swerving = false;
            startDriftSpeed = (angle - lastAngle) / Time.deltaTime;
            if (startDriftSpeed == 0) return;

            drift = true;
            driftStartTime = Time.time;
        });
    }

    private void OnSwerve()
    {
        pausedOnSwerve = Time.timeScale < 0.1f;
        if (pausedOnStart || pausedOnSwerve) return;
        if (zoomingOut || onUI) return;
        swerving = true;
        lastAngle = angle;
        zoomDistance = Mathf.Clamp(anchorDistance - swerve.YRate * cameraZoomSpeed, cameraBackwardRange, cameraForwardRange);
        //DayCycler.Instance.ChangeFogOffset(-zoomDistance);
        cameraTransform.localPosition = initialCameraPosition + direction * zoomDistance;
        if (!lockYAxis)
        {
            angle = (anchorAngle + swerve.XRate * 180) % 360f;
            transform.eulerAngles = new Vector3(0, angle, 0);
        }
        UpdateCameraDistance();
    }

    private void Start()
    {
        if (cameraTransform != null)
        {
            zoomDistance = Mathf.Clamp(anchorDistance - swerve.YRate * cameraZoomSpeed, cameraBackwardRange, cameraForwardRange);
            //DayCycler.Instance.ChangeFogOffset(-zoomDistance);
            cameraTransform.localPosition = initialCameraPosition + direction * zoomDistance;
            UpdateCameraDistance();
        }
        transform.eulerAngles = new Vector3(0, angle, 0);

    }

    private void Update()
    {
        if (drift)
        {
            float targetAngle = angle + (1 - (Time.time - driftStartTime) / totalDriftTime) * Time.deltaTime * startDriftSpeed;
            angle = targetAngle % 360f;
            transform.eulerAngles = new Vector3(0, angle, 0);
            if (Time.time > driftStartTime + totalDriftTime) drift = false;
        }
    }

    private void SetAnchorAngle()
    {
        if (lockYAxis) return;
        anchorAngle = angle;
    }

    private void SetAnchorDistance()
    {
        anchorDistance = zoomDistance;
    }


    public override void ZoomOut()
    {
        zoomingOut = true;
        FaTween.To(() => cameraTransform.position, x =>
        {
            cameraTransform.position = x;
            UpdateCameraDistance();
        }, cameraTransform.position - cameraTransform.forward * 5, 3).OnComplete(() => zoomingOut = false); ;
    }

    public override void ZoomIn()
    {
        zoomingOut = true;
        FaTween.To(() => cameraTransform.position, x =>
        {
            cameraTransform.position = x;
            UpdateCameraDistance();
        }, cameraTransform.position + cameraTransform.forward * 5, 3).OnComplete(() => zoomingOut = false);
    }

    private void Init()
    {
        if (cameraTransform != null)
        {
            direction = cameraTransform.localRotation * Vector3.forward;
            initialCameraPosition = cameraTransform.localPosition;
        }
    }

    private void UpdateCameraDistance()
    {
        cameraDistance.Value = cameraTransform.localPosition.magnitude;
    }
}
