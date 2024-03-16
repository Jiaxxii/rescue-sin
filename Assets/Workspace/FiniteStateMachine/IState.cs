using System;

namespace Workspace.FiniteStateMachine
{
    public interface IState<out TState>
        where TState : Enum
    {
        public TState State { get; }

        /// <summary>
        /// 状态进入时调用
        /// </summary>
        public void OnEnter();

        /// <summary>
        /// 如果启用了 Behaviour，则每帧调用 Update
        /// </summary>
        public void OnUnityUpdate();

        /// <summary>
        /// 用于物理计算且独立于帧率的 MonoBehaviour.FixedUpdate 消息
        /// </summary>
        public void OnUnityFixeUpdate();

        /// <summary>
        /// 如果启用了 Behaviour，则每帧调用 LateUpdate。
        /// </summary>
        public void OnUnityLateUpdate();

        /// <summary>
        /// 状态退出时
        /// </summary>
        public void OnExit();
    }
}