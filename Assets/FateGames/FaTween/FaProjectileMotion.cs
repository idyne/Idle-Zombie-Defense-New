using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static FateGames.Tweening.FaEaseFunctions;

namespace FateGames.Tweening
{
    public class FaProjectileMotion : FaCore
    {
        private float gravity;
        private Transform transform;
        private Vector3 staticStart;
        private Vector3 target;
        private Vector3 velocity;


        public FaProjectileMotion(Transform transform, Vector3 target, float duration, float gravity, bool ignoreTimeScale) : base()
        {
            this.transform = transform;
            this.target = target;
            this.duration = duration;
            this.ignoreTimeScale = ignoreTimeScale;
            this.gravity = gravity;
            staticStart = transform.position;
            Vector3 dif = target - transform.position;
            velocity = new Vector3(dif.x, dif.y - gravity * duration * duration / 2, dif.z) / duration;
        }

        public override void Proceed()
        {
            AddDeltaTime();

            if (progressTime < duration)
            {
                transform.position = staticStart + new Vector3(velocity.x * progressTime,
                    velocity.y * progressTime + 0.5f * gravity * progressTime * progressTime, velocity.z * progressTime);
            }
            else
                Kill(true);
        }

        protected override void FinishJob()
        {
            transform.position = target;
        }
    }
}
