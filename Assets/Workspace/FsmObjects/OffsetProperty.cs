using System;
using UnityEngine;

namespace Workspace.FsmObjects
{
    [Serializable]
    public class OffsetProperty : IEquatable<OffsetProperty>
    {
        [SerializeField] private string name;
        [SerializeField] private Vector2 offset;

        public string Name => name;

        public Vector2 Offset => offset;

        public override bool Equals(object obj) => obj is OffsetProperty property && property.Name == Name && property.Offset == Offset;

        public override int GetHashCode()
        {
            unchecked
            {
                return 27 + (Name.GetHashCode() ^ 173) + (Offset.GetHashCode() ^ 173);
            }
        }

        public bool Equals(OffsetProperty other)
        {
            return other != null && other.Name == Name && other.offset == Offset;
        }
    }
}