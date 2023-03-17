using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarSnapper : MonoBehaviour
{
    [SerializeField] private HealthBarSet set;
    void Start()
    {
        for (int i = 0; i < set.Items.Count; i++)
        {
            set.Items[i].SnapToTarget();
        }
    }
}
