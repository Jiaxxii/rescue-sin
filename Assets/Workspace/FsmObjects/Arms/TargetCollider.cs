using System;
using UnityEngine;

namespace Workspace.FsmObjects.Arms
{
    public class TargetCollider
    {
        /// <summary>  
        /// 初始化一个新的<see cref="TargetCollider"/>实例，不设置任何属性的值。  
        /// </summary>  
        public TargetCollider() => Clear();

        /// <summary>  
        /// 使用<see cref="Collider2D"/>对象初始化<see cref="TargetCollider"/>实例。  
        /// </summary>  
        /// <param name="target">目标碰撞器。</param>  
        public TargetCollider(Collider2D target) => UpDate(target);

        /// <summary>  
        /// 使用<see cref="Collider2D"/>对象和标签初始化<see cref="TargetCollider"/>实例。  
        /// </summary>  
        /// <param name="target">目标碰撞器。</param>  
        /// <param name="tag">目标的标签。</param>  
        public TargetCollider(Collider2D target, string tag) => UpDate(target, tag);

        /// <summary>  
        /// 使用<see cref="UnityEngine.Transform"/>对象初始化<see cref="TargetCollider"/>实例。  
        /// </summary>  
        /// <param name="transform">目标的变换组件。</param>  
        public TargetCollider(Transform transform) => UpDate(transform);

        /// <summary>  
        /// 使用<see cref="UnityEngine.Transform"/>对象和标签初始化<see cref="TargetCollider"/>实例。  
        /// </summary>  
        /// <param name="transform">目标的变换组件。</param>  
        /// <param name="tag">目标的标签。</param>  
        public TargetCollider(Transform transform, string tag) => UpDate(transform, tag);

        /// <summary>  
        /// 使用<see cref="GameObject"/>对象初始化<see cref="TargetCollider"/>实例。  
        /// </summary>  
        /// <param name="gameObject">目标游戏对象。</param>  
        public TargetCollider(GameObject gameObject) => UpDate(gameObject);

        /// <summary>  
        /// 使用<see cref="GameObject"/>对象和标签初始化<see cref="TargetCollider"/>实例。  
        /// </summary>  
        /// <param name="gameObject">目标游戏对象。</param>  
        /// <param name="tag">目标的标签。</param>  
        public TargetCollider(GameObject gameObject, string tag) => UpDate(gameObject, tag);

        /// <summary>  
        /// 获取或设置目标碰撞器。  
        /// </summary>  
        public Collider2D Target { get; set; }


        /// <summary>  
        /// 获取或设置目标变换组件。  
        /// </summary>  
        public Transform Transform { get; set; }

        /// <summary>  
        /// 获取或设置目标标签。  
        /// </summary>  
        public string Tag { get; set; }

        /// <summary>  
        /// 获取或设置上一个<see cref="TargetCollider"/>实例。  
        /// </summary>  
        public TargetCollider Last { get; set; }

        /// <summary>  
        /// 当更新<see cref="TargetCollider"/>时触发的事件。  
        /// </summary>  
        public event Action<TargetCollider> OnUpDateEventHandler;

        public void UpDate(Collider2D collider2D, string tag = null)
        {
            Target = collider2D;
            Transform = collider2D.gameObject.transform;
            Tag = string.IsNullOrEmpty(tag) ? collider2D.gameObject.tag : tag;
            OnUpDateEventHandler?.Invoke(this);
            Last = this;
        }

        public void UpDate(Transform transform, string tag = null)
        {
            Target = null;
            Transform = transform;
            Tag = string.IsNullOrEmpty(tag) ? transform.tag : tag;
            OnUpDateEventHandler?.Invoke(this);
            Last = this;
        }

        public void UpDate(GameObject gameObject, string tag = null)
        {
            Target = null;
            Transform = gameObject.transform;
            Tag = string.IsNullOrEmpty(tag) ? gameObject.tag : tag;
            OnUpDateEventHandler?.Invoke(this);
            Last = this;
        }

        public void SetNull(bool collider, bool transform, bool tag)
        {
            if (collider && transform && tag)
            {
                Debug.LogWarning($"将所有成员设置为null可能不是预期中的操作,请考虑\"{nameof(Clear)}()\"方法!");
            }

            if (collider) Target = null;
            if (transform) Transform = null;
            if (tag) Tag = null;
        }

        public T GetComponent<T>() => GetTransform().GetComponent<T>();

        public bool TryGetComponent<T>(string compareTag, out T component)
        {
            if (!CompareTag(compareTag))
            {
                component = default;
                return false;
            }

            component = GetComponent<T>();
            return true;
        }

        public void Clear()
        {
            Target = null;
            Transform = null;
            Tag = string.Empty;
            Last = null;
            OnUpDateEventHandler = null;
        }

        public bool CompareTag(string tag) => Target != null ? Target.gameObject.CompareTag(tag) :
            !string.IsNullOrEmpty(Tag) ? Tag == tag :
            Transform != null ? Transform.gameObject.CompareTag(tag) :
            throw new NullReferenceException($"无法比较标签,因为\"{nameof(Target)}\",\"{nameof(Transform)}\",\"{nameof(Tag)}\"都为空!");

        public Transform GetTransform() => Target != null ? Target.transform :
            Transform != null ? Transform : throw new NullReferenceException($"实例中的任何\"{nameof(UnityEngine.Transform)}\"都是空的!");

        public string GetTag() => !string.IsNullOrEmpty(Tag) ? Tag :
            Target is not null ? Target.gameObject.tag :
            Transform is not null ? Transform.tag : throw new NullReferenceException($"实例中的任何\"{nameof(UnityEngine.Transform)}\"都是空的!");
    }
}