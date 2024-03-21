using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Workspace.FiniteStateMachine
{
    [Serializable]
    public struct Range : IEquatable<Range>
    {
        public Range(float min, float max)
        {
            if (min >= max)
            {
                throw new ArgumentException("min 必须小于 max", nameof(min));
            }

            this.min = min;
            this.max = max;
        }

        [SerializeField] private float min;

        [SerializeField] private float max;

        public float Min => min;

        public float Max => max;

        public bool Equals(Range other)
        {
            // 使用容差值比较浮点数（根据您的需要调整 epsilon 的值）  
            const float epsilon = 0.0001f;
            return Mathf.Abs(Min - other.Min) < epsilon && Mathf.Abs(Max - other.Max) < epsilon;
        }

        public override bool Equals(object obj) => obj is Range other && Equals(other);

        public static bool operator ==(Range left, Range right) => left.Equals(right);


        public static bool operator !=(Range left, Range right) => !left.Equals(right);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash *= 23 + Min.GetHashCode();
                return hash * 23 + Max.GetHashCode();
            }
        }

        public override string ToString() => $"({Min},{Max})";
    }
}