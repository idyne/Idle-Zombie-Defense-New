using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames.Tweening
{
    public class FaDelayedCall : FaCore
    {
        private Action delayedCall;
        public FaDelayedCall(float duration, Action delayedCall, bool ignoreTimeScale) : base()
        {
            this.delayedCall = delayedCall;
            this.duration = duration;
            this.ignoreTimeScale = ignoreTimeScale;
        }

        public override void Proceed()
        {
            AddDeltaTime();
            if (progressTime >= duration) Kill(true);
        }

        protected override void FinishJob()
        {
            delayedCall();
        }
    }
}
