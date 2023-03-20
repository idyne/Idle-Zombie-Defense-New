using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static FateGames.Tweening.FaTweenMaker;

namespace FateGames.Tweening
{
    public static class FaTween
    {
        #region Delayed Call
        public static FaDelayedCall DelayedCall(float delay, Action action, bool ignoreTimeScale = false)
        {
            return new FaDelayedCall(delay, action, ignoreTimeScale);
        }
        #endregion

        #region To
        public static FaTweener<Color> To(Func<Color> getter, Action<Color> setter, Color target, float duration, bool ignoreTimeScale = false)
        {
            return new FaTweener<Color>(getter, setter, Maker, target, duration, ignoreTimeScale);
        }
        public static FaTweener<Vector4> To(Func<Vector4> getter, Action<Vector4> setter, Vector4 target, float duration, bool ignoreTimeScale = false)
        {
            return new FaTweener<Vector4>(getter, setter, Maker, target, duration,  ignoreTimeScale);
        }
        public static FaTweener<Vector3> To(Func<Vector3> getter, Action<Vector3> setter, Vector3 target, float duration, bool ignoreTimeScale = false)
        {
            return new FaTweener<Vector3>(getter, setter, Maker, target, duration, ignoreTimeScale);
        }
        public static FaTweener<Vector2> To(Func<Vector2> getter, Action<Vector2> setter, Vector2 target, float duration, bool ignoreTimeScale = false)
        {
            return new FaTweener<Vector2>(getter, setter, Maker, target, duration, ignoreTimeScale);
        }
        public static FaTweener<float> To(Func<float> getter, Action<float> setter, float target, float duration, bool ignoreTimeScale = false)
        {
            return new FaTweener<float>(getter, setter, Maker, target, duration, ignoreTimeScale);
        }
        #endregion

        #region Move
        public static FaTweener<Vector3> FaMove(this Transform trans, Vector3 target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.position,
                (Vector3 x) => trans.position = x,
                target, duration);
        }

        public static FaTweener<float> FaMoveX(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.position.x,
                (float v) =>
                {
                    Vector3 temp = trans.position;
                    temp.x = v;
                    trans.position = temp;
                },
                target, duration);
        }

        public static FaTweener<float> FaMoveY(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.position.y,
                (float v) =>
                {
                    Vector3 temp = trans.position;
                    temp.y = v;
                    trans.position = temp;
                },
                target, duration);
        }

        public static FaTweener<float> FaMoveZ(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.position.z,
                (float v) =>
                {
                    Vector3 temp = trans.position;
                    temp.z = v;
                    trans.position = temp;
                },
                target, duration);
        }

        public static FaTweener<Vector3> FaLocalMove(this Transform trans, Vector3 target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localPosition,
                (Vector3 x) => trans.localPosition = x,
                target, duration);
        }

        public static FaTweener<float> FaLocalMoveX(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localPosition.x,
                (float v) =>
                {
                    Vector3 temp = trans.localPosition;
                    temp.x = v;
                    trans.localPosition = temp;
                },
                target, duration);
        }

        public static FaTweener<float> FaLocalMoveY(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localPosition.y,
                (float v) =>
                {
                    Vector3 temp = trans.localPosition;
                    temp.y = v;
                    trans.localPosition = temp;
                },
                target, duration);
        }

        public static FaTweener<float> FaLocalMoveZ(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localPosition.z,
                (float v) =>
                {
                    Vector3 temp = trans.localPosition;
                    temp.z = v;
                    trans.localPosition = temp;
                },
                target, duration);
        }
        #endregion

        #region Scale
        public static FaTweener<Vector3> FaLocalScale(this Transform trans, Vector3 target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localScale,
                (Vector3 x) => trans.localScale = x,
                target, duration);
        }
        public static FaTweener<float> FaLocalScaleX(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localScale.x,
                (float v) =>
                {
                    Vector3 temp = trans.localScale;
                    temp.x = v;
                    trans.localScale = temp;
                },
                target, duration);
        }
        public static FaTweener<float> FaLocalScaleY(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localScale.y,
                (float v) =>
                {
                    Vector3 temp = trans.localScale;
                    temp.y = v;
                    trans.localScale = temp;
                },
                target, duration);
        }
        public static FaTweener<float> FaLocalScaleZ(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localScale.z,
                (float v) =>
                {
                    Vector3 temp = trans.localScale;
                    temp.z = v;
                    trans.localScale = temp;
                },
                target, duration);
        }
        #endregion

        #region Rotation
        public static FaTweener<Vector3> FaRotate(this Transform trans, Vector3 target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.eulerAngles,
                (Vector3 x) => trans.eulerAngles = x,
                target, duration);
        }
        public static FaTweener<float> FaRotateX(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.eulerAngles.x,
                (float v) =>
                {
                    Vector3 temp = trans.eulerAngles;
                    temp.x = v;
                    trans.eulerAngles = temp;
                },
                target, duration);
        }
        public static FaTweener<float> FaRotateY(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.eulerAngles.y,
                (float v) =>
                {
                    Vector3 temp = trans.eulerAngles;
                    temp.y = v;
                    trans.eulerAngles = temp;
                },
                target, duration);
        }
        public static FaTweener<float> FaRotateZ(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.eulerAngles.z,
                (float v) =>
                {
                    Vector3 temp = trans.eulerAngles;
                    temp.z = v;
                    trans.eulerAngles = temp;
                },
                target, duration);
        }
        public static FaTweener<Vector3> FaLocalRotate(this Transform trans, Vector3 target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localEulerAngles,
                (Vector3 x) => trans.localEulerAngles = x,
                target, duration);
        }
        public static FaTweener<float> FaLocalRotateX(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localEulerAngles.x,
                (float v) =>
                {
                    Vector3 temp = trans.localEulerAngles;
                    temp.x = v;
                    trans.localEulerAngles = temp;
                },
                target, duration);
        }
        public static FaTweener<float> FaLocalRotateY(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localEulerAngles.y,
                (float v) =>
                {
                    Vector3 temp = trans.localEulerAngles;
                    temp.y = v;
                    trans.localEulerAngles = temp;
                },
                target, duration);
        }
        public static FaTweener<float> FaLocalRotateZ(this Transform trans, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => trans.localEulerAngles.z,
                (float v) =>
                {
                    Vector3 temp = trans.localEulerAngles;
                    temp.z = v;
                    trans.localEulerAngles = temp;
                },
                target, duration);
        }
        #endregion

        #region Color
        public static FaTweener<float> FaFade(this Image image, float target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => image.color.a,
                (float v) =>
                {
                    Color temp = image.color;
                    temp.a = v;
                    image.color = temp;
                },
                target, duration);
        }

        public static FaTweener<Color> FaColor(this Image image, Color target, float duration, bool ignoreTimeScale = false)
        {
            return To(() => image.color,
                (Color v) =>
                {
                    image.color = v;
                },
                target, duration);
        }
        #endregion

        #region Projectile Motion
        public static FaProjectileMotion FaProjectileMotion(this Transform transform, Vector3 target, float duration, float gravity = -9.81f, bool ignoreTimeScale = false)
        {
            return new FaProjectileMotion(transform, target, duration, gravity, ignoreTimeScale);
        }
        #endregion
    }
}
