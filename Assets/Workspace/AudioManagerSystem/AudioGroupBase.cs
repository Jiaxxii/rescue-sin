using UnityEngine;

namespace Workspace.AudioManagerSystem
{
    public class AudioGroupBase<T> : IAudioGroup where T : AudioGroupBase<T>
    {
        public AudioGroupBase() => Name = nameof(T);
        public AudioGroupBase(string groupName) => Name = groupName;

        public string Name { get; }

        protected readonly System.Collections.Generic.Dictionary<string, AudioSource> Dictionary = new();


        public AudioSource this[string itemName] => Get(itemName);


        public virtual void Add(string itemName, AudioSource au) => Dictionary.Add(itemName, au);
        public virtual bool TryAdd(string itemName, AudioSource au) => Dictionary.TryAdd(itemName, au);

        public virtual AudioSource Get(string itemName) => Dictionary[itemName];
        public virtual bool TryGet(string itemName, out AudioSource audioSource) => Dictionary.TryGetValue(itemName, out audioSource);


        public virtual bool ContainsKey(string itemName) => Dictionary.ContainsKey(itemName);
    }
}