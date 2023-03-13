using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static FateGames.Tweening.FaEaseFunctions;

namespace FateGames.Tweening
{
    public class FaTweenMaker
    {
        public static Color Maker(float duration, float progressTime, Color from, Color to,
            bool dynamic, EaseMode mode, Color staticStart, bool ignoreTimeScale)
        {
            if (dynamic) return (to - from) / (duration - progressTime) * DeltaTime(ignoreTimeScale) + from;
            else return (to - staticStart) * GetEaseValue(mode, duration, progressTime) + staticStart;
        }
        public static Vector4 Maker(float duration, float progressTime, Vector4 from, Vector4 to,
            bool dynamic, EaseMode mode, Vector4 staticStart, bool ignoreTimeScale)
        {
            if (dynamic) return (to - from) / (duration - progressTime) * DeltaTime(ignoreTimeScale) + from;
            else return (to - staticStart) * GetEaseValue(mode, duration, progressTime) + staticStart;
        }
        public static Vector3 Maker(float duration, float progressTime, Vector3 from, Vector3 to,
            bool dynamic, EaseMode mode, Vector3 staticStart, bool ignoreTimeScale)
        {
            if (dynamic) return (to - from) / (duration - progressTime) * DeltaTime(ignoreTimeScale) + from;
            else return (to - staticStart) * GetEaseValue(mode, duration, progressTime) + staticStart;
        }
        public static Vector2 Maker(float duration, float progressTime, Vector2 from, Vector2 to,
            bool dynamic, EaseMode mode, Vector2 staticStart, bool ignoreTimeScale)
        {
            if (dynamic) return (to - from) / (duration - progressTime) * DeltaTime(ignoreTimeScale) + from;
            else return (to - staticStart) * GetEaseValue(mode, duration, progressTime) + staticStart;
        }
        public static float Maker(float duration, float progressTime, float from, float to,
            bool dynamic, EaseMode mode, float staticStart, bool ignoreTimeScale)
        {
            if (dynamic) return (to - from) / (duration - progressTime) * DeltaTime(ignoreTimeScale) + from;
            else return (to - staticStart) * GetEaseValue(mode, duration, progressTime) + staticStart;
        }

        private static float DeltaTime(bool ignoreTimeScale)
        {
            if (ignoreTimeScale) return Time.deltaTime;
            else return Time.deltaTime * Time.timeScale;
        }
    }
}
