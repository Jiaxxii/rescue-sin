using System;
using UnityEngine;

namespace Workspace.AudioManagerSystem
{
    /// <summary>
    /// 注意*此结构体的比较只比较<see cref="GameObject"/>.name与<see cref="Name"/>是否相等
    /// </summary>
    public readonly struct GroupPair : IEquatable<GroupPair>
    {
        public GroupPair(GameObject gameObject)
        {
            GameObject = gameObject;
            if (GameObject == null) throw new NullReferenceException($"对象引用未设置对象实例!\"{nameof(gameObject)}\"");

            Name = gameObject.name;
#if DEBUG
            if (GameObject.name != Name)
                Debug.LogWarning($"属性{nameof(GameObject)}({GameObject.name})与属性{nameof(Name)}({Name})不一致，这可能不是预期的!");
#endif
        }

        public GameObject GameObject { get; }
        public string Name { get; }


        public bool Equals(GroupPair other) => other.GameObject.name == GameObject.name && other.Name == Name;

        public override bool Equals(object obj)
            => obj switch
            {
                GroupPair pair => pair.GameObject.name == GameObject.name && pair.Name == Name,
                string str => str == GameObject.name && str == Name,
                _ => false
            };

        public bool Equals(string name) => GameObject.name == name && Name == name;


        public override int GetHashCode() => 27 + (GameObject.name.GetHashCode() ^ 173) + (Name.GetHashCode() ^ 173);


        public static bool operator ==(GroupPair left, GroupPair right) => left.Equals(right);

        public static bool operator !=(GroupPair left, GroupPair right) => !left.Equals(right);
        
    }
}