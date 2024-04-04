using System;
using UnityEngine;

namespace Workspace.AudioManagerSystem
{
    public interface IAudioGroup
    {
        public string Name { get; }

        AudioSource this[string itemName] { get; }


        public void Add(string itemName, AudioSource au);
        public bool TryAdd(string itemName, AudioSource au);

        AudioSource Get(string itemName);
        bool TryGet(string itemName, out AudioSource audioSource);


        bool ContainsKey(string itemName);
    }
}