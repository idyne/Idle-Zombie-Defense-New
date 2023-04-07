using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesButton : UIElement
{
    [SerializeField] private UIElement notification;
    [SerializeField] private UpgradeListEntityRuntimeSet set;

    public void CheckNotification()
    {
        notification.Hide();
        for (int i = 0; i < set.Items.Count; i++)
        {
            PreparationUpgradeEntity entity = set.Items[i];
            if (entity.Upgradeable)
            {
                notification.Show();
                break;
            }
        }
    }
}
