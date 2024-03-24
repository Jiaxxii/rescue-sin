using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Workspace.ScriptableObjectData
{
    public abstract class DictionaryBase<T>
    {
        private Dictionary<string, T> _dictionary;
        public abstract string Name { get; }

        public Dictionary<string, T> Dictionary
        {
            get
            {
                if (_dictionary != null) return _dictionary;

                return _dictionary = Init();
            }
        }

        protected abstract Dictionary<string, T> Init();

        public virtual T GetRandom()
        {
            var key = Dictionary.Keys.ToArray();
            return Dictionary[key[Random.Range(0, key.Length)]];
        }
    }
}