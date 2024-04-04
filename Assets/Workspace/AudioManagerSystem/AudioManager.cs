using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Workspace.AudioManagerSystem
{
    public interface IAudioManager
    {
        /// <summary>
        /// 尝试向指定的组中添加新的<see cref="AudioSource"/>对象
        /// </summary>
        /// <param name="groupName">组</param>
        /// <param name="itemName">新的<see cref="AudioSource"/>索引名称 (用于后续查找)</param>
        /// <param name="audioGroup">音乐分组类型</param>
        /// <returns>添加成功时为 true</returns>
        public bool TrtAddAudioSource(string groupName, string itemName, IAudioGroup audioGroup);

        /// <summary>
        /// 尝试向指定的组中添加新的<see cref="AudioSource"/>对象
        /// </summary>
        /// <param name="createFunc">组</param>
        /// <param name="itemName">新的<see cref="AudioSource"/>索引名称 (用于后续查找)</param>
        /// <param name="audioGroup">音乐分组类型</param>
        /// <returns>添加成功时为 true</returns>
        public bool TrtAddAudioSource(Func<GroupPair> createFunc, string itemName, IAudioGroup audioGroup);

        /// <summary>
        /// 尝试通过<see cref="GroupPair"/>实例与 <see cref="itemName"/> 获取<see cref="AudioSource"/>实例
        /// </summary>
        /// <param name="group">组</param>
        /// <param name="itemName">索引名称</param>
        /// <param name="audioSource"><see cref="AudioSource"/></param>
        /// <returns>成功时为 true</returns>
        public bool TryGetSource(GroupPair group, string itemName, out AudioSource audioSource);

        /// <summary>
        /// 尝试通过<see cref="GroupPair"/>实例与 <see cref="itemName"/> 获取<see cref="AudioSource"/>实例
        /// </summary>
        /// <param name="group">组</param>
        /// <param name="itemName">索引名称</param>
        /// <returns><see cref="AudioSource"/></returns>
        /// <exception cref="ArgumentException">未定义的组异常</exception>
        public AudioSource GetSource(GroupPair group, string itemName);


        /// <summary>
        /// 在<see cref="AudioManager"/>游戏对象上创建名称为 <see cref="groupName"/> 的游戏对象并且返回一个<see cref="GroupPair"/>
        /// </summary>
        /// <param name="groupName">组名称 (不可重复)</param>
        /// <returns>
        /// 用于 <see cref="GetSource"/> <see cref="TrtAddAudioSource(System.Func{GroupPair},string,Workspace.AudioManagerSystem.IAudioGroup)"/> 等方法的key参数
        /// </returns>
        /// <exception cref="ArgumentException">组(名称)已经在节点上包含</exception>
        public GroupPair Create(string groupName);
    }

    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [SerializeField] private Transform root;
        private readonly Dictionary<GroupPair, IAudioGroup> _audioSources = new();

        private static IAudioManager _instance;

        public static IAudioManager Instance => _instance ??= FindObjectOfType<AudioManager>();

        private void Awake()
        {
            _ = _instance;
        }


        public bool TrtAddAudioSource(string groupName, string itemName, IAudioGroup audioGroup)
        {
            return TrtAddAudioSource(() => Create(groupName), itemName, audioGroup);
        }

        public bool TrtAddAudioSource(Func<GroupPair> createFunc, string itemName, IAudioGroup audioGroup)
        {
            var group = createFunc?.Invoke() ?? throw new AggregateException("未指定创建方式!");
            if (!_audioSources.TryAdd(group, audioGroup))
            {
                return false;
            }

            // 向组中添加新的AudioSource
            _audioSources[group].Add(itemName, group.GameObject.AddComponent<AudioSource>());
            return true;
        }

        public bool TryGetSource(GroupPair group, string itemName, out AudioSource audioSource)
        {
            if (_audioSources.TryGetValue(group, out var source)) return source.TryGet(itemName, out audioSource);

            audioSource = null;
            return false;
        }

        public AudioSource GetSource(GroupPair group, string itemName)
        {
            if (!TryGetSource(group, itemName, out var audioSource))
                throw new ArgumentException($"未定义的组\"{group.GameObject.name}\"");

            return audioSource;
        }


        public GroupPair Create(string groupName)
        {
            // 检查节点下是否存在组
            if (transform.Find(groupName) != null)
            {
                throw new ArgumentException($"\"{groupName}\"组已经存在!");
            }

            var obj = new GameObject(groupName);
            obj.transform.SetParent(root == null ? transform : root);

            return new GroupPair(obj);
        }
    }
}