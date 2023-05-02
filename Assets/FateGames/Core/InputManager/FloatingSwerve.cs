using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FateGames.Core
{
    public class FloatingSwerve : Swerve
    {
        protected override void OnMouseButton()
        {
            MousePosition = Input.mousePosition;
            Vector2 direction = (MousePosition - AnchorPosition).normalized;
            AnchorPosition = AnchorPosition + direction * Mathf.Clamp((AnchorPosition - MousePosition).magnitude - Size, 0, float.MaxValue);
            OnSwerve.Invoke();
        }
    }
}