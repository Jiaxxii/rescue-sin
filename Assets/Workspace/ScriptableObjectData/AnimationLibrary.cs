using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Workspace.ScriptableObjectData
{
    [CreateAssetMenu(fileName = "New AnimationLibrary", menuName = "SO/AnimationLibrary")]
    public class AnimationLibrary : ScriptableObject
    {
        [SerializeField] private List<GroupItem> animationLibraryGroup;
        private readonly List<string> _keys = new();
        private Dictionary<string, GroupItem> _groupItems;
 
        private Dictionary<string, GroupItem> GroupItems
        {
            get
            {
                if (_groupItems != null) return _groupItems;

                _groupItems = new Dictionary<string, GroupItem>();

                foreach (var groupItem in animationLibraryGroup)
                {
                    if (!_groupItems.TryAdd(groupItem.Name, groupItem))
                    {
                        throw new AggregateException($"重复的组\"{groupItem.Name}\"");
                    }

                    _keys.Add(groupItem.Name);
                }

                return _groupItems;
            }
        }

        public GroupItem GetGroup(string groupName)
        {
            if (!GroupItems.TryGetValue(groupName, out var groupItem))
                throw new ArgumentException($"没有组\"{groupName}\"!");
            return groupItem;
        }

        public GroupItem GetGroup() => GroupItems[_keys[Random.Range(0, _keys.Count)]];

        public AnimationClips GetAnimationClips(string groupName, string stateName)
        {
            var groupItem = GetGroup(groupName);
            if (!groupItem.Dictionary.TryGetValue(stateName, out var animationClips))
                throw new ArgumentException($"组\"{groupName}\"中没有\"{stateName}\"状态!");
            return animationClips;
        }

        public AnimationClips GetAnimationClips() => GetGroup().GetRandom();


        public int GetClipHashCode(string groupName, string stateName, string clipName)
        {
            var animationClips = GetAnimationClips(groupName, stateName);
            if (!animationClips.Dictionary.TryGetValue(clipName, out var hashCode))
                throw new ArgumentException($"组\"{groupName}\"中状态\"{stateName}\"里没有\"{clipName}\"动画!");

            return hashCode;
        }

        public int GetClipHashCode() => GetGroup().GetRandom().GetRandom();
    }

    [Serializable]
    public class GroupItem : DictionaryBase<AnimationClips>
    {
        [SerializeField] private string groupName;
        [SerializeField] private List<AnimationClips> animationClipLibrary;


        public override string Name => groupName;

        protected override Dictionary<string, AnimationClips> Init()
        {
            var dic = new Dictionary<string, AnimationClips>();

            foreach (var animationClip in animationClipLibrary
                         .Where(animationClip => !dic.TryAdd(animationClip.Name, animationClip)))
            {
                throw new AggregateException($"重复的状态\"{animationClip.Name}\"");
            }

            return dic;
        }
    }

    [Serializable]
    public class AnimationClips : DictionaryBase<int>
    {
        [SerializeField] private string stateName;

        [SerializeField] private AnimationClip[] clips;
        

        public override string Name => stateName;

        protected override Dictionary<string, int> Init()
        {
            if (string.IsNullOrEmpty(stateName))
            {
                throw new AggregateException($"{nameof(stateName)} 不能为空!");
            }

            if (clips == null || clips.Length == 0)
            {
                throw new NullReferenceException("没有任何动画可以播放!");
            }
            
            var data = new Dictionary<string, int>();
            foreach (var clip in clips)
            {
                if (!data.TryAdd(clip.name, Animator.StringToHash(clip.name)))
                {
                    Debug.LogWarning($"\"{clip.name}\"已经包含！请确保动画的名称不同！");
                }
            }


            return data;
        }
    }
}