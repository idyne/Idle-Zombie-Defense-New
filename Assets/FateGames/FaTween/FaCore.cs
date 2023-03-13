using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames.Tweening
{
    public abstract class FaCore
    {
        protected float duration = 0;
        protected float progressTime = 0;
        protected Action callBack;
        protected bool ignoreTimeScale;

        public abstract void Proceed();

        protected abstract void FinishJob();

        public FaCore()
        {
            FaTweenManager.Instance.Add(this);
        }

        public void Kill(bool finishJob = false)
        {
            if (finishJob) FinishJob();

            FaTweenManager.Instance.Remove(this);

            if (callBack != null) callBack();
        }

        public FaCore OnComplate(Action callBack)
        {
            this.callBack += callBack;
            return this;
        }

        /*public FaCore Recycle(bool recycle)
        {
            return this;
        }*/

        protected void AddDeltaTime()
        {
            if (ignoreTimeScale) progressTime += Time.deltaTime;
            else progressTime += Time.deltaTime * Time.timeScale;
        }
    }
}
