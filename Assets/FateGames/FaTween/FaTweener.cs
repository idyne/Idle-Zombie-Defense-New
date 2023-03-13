using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static FateGames.Tweening.FaEaseFunctions;

namespace FateGames.Tweening
{
    public class FaTweener<T1> : FaCore
    {
        private Func<T1> getter;
        private Action<T1> setter;
        private Func<float, float, T1, T1, bool, EaseMode, T1, bool, T1> maker;
        private T1 target;
        private bool dynamicRoute = false;
        private EaseMode easeMode = EaseMode.Linear;
        private T1 staticStart;

        public FaTweener(Func<T1> getter, Action<T1> setter, Func<float, float, T1, T1, bool, EaseMode, T1, bool, T1> maker,
            T1 target, float duration, bool ignoreTimeScale) : base()
        {
            this.getter = getter;
            this.setter = setter;
            this.maker = maker;
            this.target = target;
            this.duration = duration;
            this.ignoreTimeScale = ignoreTimeScale;
            staticStart = getter();
        }

        public override void Proceed()
        {
            AddDeltaTime();

            if (progressTime < duration)
                setter(maker(duration, progressTime, getter(), target, dynamicRoute, easeMode, staticStart, ignoreTimeScale));
            else
                Kill(true);
        }

        protected override void FinishJob()
        {
            setter(target);
        }

        public FaTweener<T1> SetEaseFunction(EaseMode easeMode)
        {
            if (easeMode != this.easeMode)
            {
                if (!dynamicRoute) this.easeMode = easeMode;
                else Debug.LogError("Ease Function cannot be assigned to a Dynamic tween.");
            }
            return this;
        }

        public FaTweener<T1> SetDynamicRoute(bool dynamicRoute)
        {
            if (dynamicRoute != this.dynamicRoute)
            {
                if (easeMode == EaseMode.Linear) this.dynamicRoute = dynamicRoute;
                else Debug.LogError("Tweens with Ease Functions cannot be Dynamic.");
            }
            return this;
        }
    }
}