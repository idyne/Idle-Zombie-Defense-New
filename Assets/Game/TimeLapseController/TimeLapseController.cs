using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Tweening;
using DG.Tweening;

public class TimeLapseController : MonoBehaviour
{
    [SerializeField] private Light directionalLight = null;
    [SerializeField] private FogController fogController = null;
    [SerializeField] private ZoneManager zoneManager = null;
    [SerializeField] private float transitionDuration = 2f;
    [SerializeField] private List<Color> lightColors = new List<Color>();
    [SerializeField] private List<Vector3> lightRotations = new List<Vector3>();
    [SerializeField] private List<GameObject> timeHeaders = new List<GameObject>();

    private Transform lightTransform = null;
    private Animator animator;

    private void Awake()
    {
        if (!directionalLight)
        {
            Debug.LogError("Directional light missing.");
            return;
        }

        lightTransform = directionalLight.transform;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        int timeIndex = (zoneManager.WaveLevel-1) % 4;

        directionalLight.color = lightColors[timeIndex];
        lightTransform.rotation = Quaternion.Euler(lightRotations[timeIndex]);

        AnimateHeader(timeIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Animate(0);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Animate(1);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Animate(2);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Animate(3);
        }
    }

    public void Animate(int targetIndex)
    {
        AnimateLight(targetIndex);
        AnimateHeader(targetIndex);
        AnimateFog(targetIndex);
    }

    private void AnimateLight(int targetIndex)
    {
        FaTween.To(() => directionalLight.color, (Color x) => directionalLight.color = x, lightColors[targetIndex], transitionDuration);
        lightTransform.DORotateQuaternion(Quaternion.Euler(lightRotations[targetIndex]), transitionDuration);
    }

    private void AnimateHeader(int targetIndex)
    {
        timeHeaders[targetIndex].SetActive(true);
        animator.SetTrigger("HeaderUp");
        FaTween.DelayedCall(2f, () => timeHeaders[targetIndex].SetActive(false));
    }

    private void AnimateFog(int targetIndex)
    {
        fogController.SetFogToTime(targetIndex, transitionDuration);
    }
}
