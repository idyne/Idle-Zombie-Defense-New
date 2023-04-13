using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesButton : UIElement
{
    [SerializeField] private UIElement notification;
    [SerializeField] private UpgradeListEntityRuntimeSet set;
    private bool isNotificationLocked = false;

    public void CheckNotification()
    {
        if (isNotificationLocked) return;
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
    public void LockNotification() => isNotificationLocked = true;
    public void UnlockNotification() => isNotificationLocked = false;
}
