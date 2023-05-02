using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FateGames.Core
{
    public class Swerve : FateMonoBehaviour
    {
        public float Size = Screen.width * 2;
        public Vector2 AnchorPosition { get; protected set; } = Vector2.zero;
        public Vector2 MousePosition { get; protected set; } = Vector2.zero;
        public Vector2 Difference { get => MousePosition - AnchorPosition; }
        public float Distance { get => Difference.magnitude; }
        public float Rate { get => Distance / Size; }
        public float XRate { get => Difference.x / Size; }
        public float YRate { get => Difference.y / Size; }
        public readonly UnityEvent OnStart = new();
        public readonly UnityEvent OnSwerve = new();
        public readonly UnityEvent OnRelease = new();


        private void Update()
        {
            if (Input.touchSupported && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnMouseButtonDown();
                        break;
                    case TouchPhase.Moved:
                        OnMouseButton();
                        break;
                    case TouchPhase.Stationary:
                        OnMouseButton();
                        break;
                    case TouchPhase.Ended:
                        OnMouseButtonUp();
                        break;
                    case TouchPhase.Canceled:
                        OnMouseButtonUp();
                        break;
                    default:
                        break;
                }
            }
            else if (!Input.touchSupported)
            {
                if (Input.GetMouseButtonDown(0)) OnMouseButtonDown();
                else if (Input.GetMouseButton(0)) OnMouseButton();
                else if (Input.GetMouseButtonUp(0)) OnMouseButtonUp();
            }
        }

        protected virtual void OnMouseButtonDown()
        {
            if (Input.touchSupported)
            {
                if (Input.touchCount == 0) return;
                AnchorPosition = Input.GetTouch(0).position;
                MousePosition = Input.GetTouch(0).position;
            }
            else
            {
                AnchorPosition = Input.mousePosition;
                MousePosition = Input.mousePosition;
            }

            OnStart.Invoke();
        }

        protected virtual void OnMouseButton()
        {
            Vector2 mousePosition;
            if (Input.touchSupported)
            {
                if (Input.touchCount == 0) return;
                mousePosition = Input.GetTouch(0).position;
            }
            else
            {
                mousePosition = Input.mousePosition;
            }
            Vector2 direction = (mousePosition - AnchorPosition).normalized;
            MousePosition = AnchorPosition + direction * Mathf.Clamp((mousePosition - AnchorPosition).magnitude, 0, Size);
            OnSwerve.Invoke();
        }

        protected virtual void OnMouseButtonUp()
        {
            OnRelease.Invoke();
        }

    }
}

