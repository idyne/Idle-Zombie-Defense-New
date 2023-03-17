using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class BillboardUpdater : MonoBehaviour
{
    [SerializeField] private BillboardRuntimeSet set;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        for (int i = 0; i < set.Items.Count; i++)
        {
            set.Items[i].LookAt(cameraTransform);
        }
    }
}
