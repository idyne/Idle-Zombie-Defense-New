using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames.Tweening
{
    public class FaEaseFunctions
    {
        public enum EaseMode
        {
            Linear,
            InSine, OutSine, InOutSine,
            InQuad, OutQuad, InOutQuad,
            InCubic, OutCubic, InOutCubic,
            InQuart, OutQuart, InOutQuart,
            InQuint, OutQuint, InOutQuint,
            InExpo, OutExpo, InOutExpo,
            InCirc, OutCirc, InOutCirc,
            InBack, OutBack, InOutBack,
            InElastic, OutElastic, InOutElastic,
            InBounce, OutBounce, InOutBounce
        }

        public static float GetEaseValue(EaseMode mode, float duration, float progressTime)
        {
            switch (mode)
            {
                case EaseMode.InSine: return InSine(progressTime / duration);
                case EaseMode.OutSine: return OutSine(progressTime / duration);
                case EaseMode.InOutSine: return InOutSine(progressTime / duration);

                case EaseMode.InQuad: return InQuad(progressTime / duration);
                case EaseMode.OutQuad: return OutQuad(progressTime / duration);
                case EaseMode.InOutQuad: return InOutQuad(progressTime / duration);

                case EaseMode.InCubic: return InCubic(progressTime / duration);
                case EaseMode.OutCubic: return OutCubic(progressTime / duration);
                case EaseMode.InOutCubic: return InOutCubic(progressTime / duration);

                case EaseMode.InQuart: return InQuart(progressTime / duration);
                case EaseMode.OutQuart: return OutQuart(progressTime / duration);
                case EaseMode.InOutQuart: return InOutQuart(progressTime / duration);

                case EaseMode.InQuint: return InQuint(progressTime / duration);
                case EaseMode.OutQuint: return OutQuint(progressTime / duration);
                case EaseMode.InOutQuint: return InOutQuint(progressTime / duration);

                case EaseMode.InExpo: return InExpo(progressTime / duration);
                case EaseMode.OutExpo: return OutExpo(progressTime / duration);
                case EaseMode.InOutExpo: return InOutExpo(progressTime / duration);

                case EaseMode.InCirc: return InCirc(progressTime / duration);
                case EaseMode.OutCirc: return OutCirc(progressTime / duration);
                case EaseMode.InOutCirc: return InOutCirc(progressTime / duration);

                case EaseMode.InBack: return InBack(progressTime / duration);
                case EaseMode.OutBack: return OutBack(progressTime / duration);
                case EaseMode.InOutBack: return InOutBack(progressTime / duration);

                case EaseMode.InElastic: return InElastic(progressTime / duration);
                case EaseMode.OutElastic: return OutElastic(progressTime / duration);
                case EaseMode.InOutElastic: return InOutElastic(progressTime / duration);

                case EaseMode.InBounce: return InBounce(progressTime / duration);
                case EaseMode.OutBounce: return OutBounce(progressTime / duration);
                case EaseMode.InOutBounce: return InOutBounce(progressTime / duration);


                default: return (progressTime / duration);
            }
        }
        public static float InSine(float x) { return 1 - Mathf.Cos((x * Mathf.PI) / 2); }

        public static float OutSine(float x) { return Mathf.Sin((x * Mathf.PI) / 2); }

        public static float InOutSine(float x) { return (1 - Mathf.Cos(Mathf.PI * x)) / 2; }


        public static float InQuad(float x) { return x * x; }

        public static float OutQuad(float x) { return 1 - (1 - x) * (1 - x); }

        public static float InOutQuad(float x) { return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2; }


        public static float InCubic(float x) { return x * x * x; }

        public static float OutCubic(float x) { return 1 - Mathf.Pow(1 - x, 3); }

        public static float InOutCubic(float x) { return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2; }


        public static float InQuart(float x) { return x * x * x * x; }

        public static float OutQuart(float x) { return 1 - Mathf.Pow(1 - x, 4); }

        public static float InOutQuart(float x) { return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2; }


        public static float InQuint(float x) { return x * x * x * x * x; }

        public static float OutQuint(float x) { return 1 - Mathf.Pow(1 - x, 5); }

        public static float InOutQuint(float x) { return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2; }


        public static float InExpo(float x) { return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10); }

        public static float OutExpo(float x) { return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x); }

        public static float InOutExpo(float x)
        {
            return x == 0 ? 0 : x == 1 ? 1 : x < 0.5
                ? Mathf.Pow(2, 20 * x - 10) / 2 : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
        }


        public static float InCirc(float x) { return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2)); }

        public static float OutCirc(float x) { return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2)); }

        public static float InOutCirc(float x)
        {
            return x < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
                : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
        }


        public static float InBack(float x)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;
            return c3 * x * x * x - c1 * x * x;
        }

        public static float OutBack(float x)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;
            return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
        }

        public static float InOutBack(float x)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;
            return x < 0.5
              ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
              : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
        }


        public static float InElastic(float x)
        {
            float c4 = (2 * Mathf.PI) / 3;

            return x == 0
              ? 0
              : x == 1
              ? 1
              : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c4);
        }

        public static float OutElastic(float x)
        {
            float c4 = (2 * Mathf.PI) / 3;

            return x == 0
              ? 0
              : x == 1
              ? 1
              : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
        }

        public static float InOutElastic(float x)
        {
            float c5 = (2 * Mathf.PI) / 4.5f;

            return x == 0
              ? 0
              : x == 1
              ? 1
              : x < 0.5
              ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
              : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
        }


        public static float InBounce(float x) { return 1 - OutBounce(1 - x); }

        public static float OutBounce(float x)
        {
            float n1 = 7.5625f;
            float d1 = 2.75f;

            if (x < 1 / d1)
            {
                return n1 * x * x;
            }
            else if (x < 2 / d1)
            {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            }
            else if (x < 2.5 / d1)
            {
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            }
            else
            {
                return n1 * (x -= 2.625f / d1) * x + 0.984375f;
            }
        }

        public static float InOutBounce(float x)
        {
            return x < 0.5
              ? (1 - OutBounce(1 - 2 * x)) / 2
              : (1 + OutBounce(2 * x - 1)) / 2;
        }
    }
}
