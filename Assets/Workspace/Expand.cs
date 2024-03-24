using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;

namespace Workspace
{
    public static class Expand
    {
        public static float MapFloat(this float value, float inMin, float inMax, float outMin, float outMax)
        {
            return (outMax - outMin) / (inMax - inMin) * (value - inMin) + outMin;
        }


        public static float InverseMapFloat(this float value, float outMin, float outMax, float inMin, float inMax)
        {
            return (inMax - inMin) / (outMax - outMin) * (value - outMin) + inMin;
        }


        public static Tween SetEase(this Tween tween, Ease ease, AnimationCurve curve)
        {
            if (ease is not (Ease.Unset or Ease.INTERNAL_Zero or Ease.INTERNAL_Custom)) return tween.SetEase(ease);

            if (curve != null) return tween.SetEase(curve);

            Debug.LogWarning(
                $"{nameof(AnimationCurve)}为空时 {nameof(Ease)}不能是 {nameof(Ease.Unset)}、{nameof(Ease.INTERNAL_Zero)}、{nameof(Ease.INTERNAL_Custom)}");
            return tween.SetEase(Ease.Linear);
        }

        public static Tween SetEase(this Tween tween, AnimationCurve curve, Ease ease) => tween.SetEase(ease, curve);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 V2(this Vector3 vector) => new(vector.x, vector.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 V3(this Vector2 vector, float z = 0F) => new(vector.x, vector.y, z);
    }
}