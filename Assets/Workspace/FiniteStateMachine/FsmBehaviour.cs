using System;
using System.Collections.Generic;
using UnityEngine;
using Workspace.FiniteStateMachine.ExpandInterface;

namespace Workspace.FiniteStateMachine
{
    public class FsmBehaviour<TState, TResources>
        : MonoBehaviour, IPlayerPosition
        where TState : Enum
    {
        protected StateMachine<TState, TResources> StateMachine;

        private readonly Dictionary<TState, IState<TState>> _stateMap = new();

        [Header("玩家设置")] [SerializeField] protected Transform player;

        [Tooltip("玩家的坐标点进行偏移")] [SerializeField]
        protected Vector2 playerOffset;

        protected virtual TState CurrentState { get; set; }

        public virtual Vector3 CurrentPosition => transform.position;

        public Vector3 PlayerOffsetPosition => new(player.position.x + playerOffset.x, player.position.y + playerOffset.y, player.position.z);

        protected virtual void Update()
        {
            StateMachine?.Update();
        }

        protected void Add(IState<TState> stateObject)
        {
            var state = stateObject.State;
            if (!_stateMap.TryAdd(state, stateObject))
            {
                Debug.LogWarning($"重复添加的状态\"{state}\"");
            }
        }


        public virtual void ChangedState(TState state)
        {
            if (!_stateMap.TryGetValue(state, out var stateObject))
            {
                Debug.LogError($"没有定义的状态\"{state}\"");
                return;
            }

            CurrentState = stateObject.State;
            StateMachine.ChangeState(stateObject);
        }


        #region Distance

        /// <summary>
        /// 返回目标与玩家之间的距离 (玩家偏移坐标)
        /// </summary>
        /// <param name="position">目标</param>
        /// <returns></returns>
        public float Distance(Vector3 position) => Vector3.Distance(PlayerOffsetPosition, position);

        /// <summary>
        /// 返回目标与玩家之间的距离 (玩家偏移坐标)
        /// </summary>
        /// <returns></returns>
        public virtual float Distance() => Vector3.Distance(PlayerOffsetPosition, CurrentPosition);

        #endregion

        #region InPlayerDirection

        /// <summary>
        /// 返回目标在玩家的方向 (玩家偏移坐标)
        /// </summary>
        /// <param name="position">目标坐标</param>
        /// <returns></returns>
        public Vector3 InPlayerDirection(Vector3 position) => (PlayerOffsetPosition - position).normalized;

        /// <summary>
        /// 返回目标在玩家的方向 (玩家偏移坐标)
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 InPlayerDirection() => (PlayerOffsetPosition - CurrentPosition).normalized;

        #endregion

        #region InPlayer

        #region InRangeX_Y

        /// <summary>
        /// 返回玩家是否在指定的X范围内 (玩家偏移坐标)
        /// </summary>
        /// <param name="range">绝对范围</param>
        /// <returns></returns>
        public bool InRangeX(Vector2 range) => range.x < PlayerOffsetPosition.x && range.y > PlayerOffsetPosition.x;

        /// <summary>
        /// 返回玩家是否在指定的Y范围内 (玩家偏移坐标)
        /// </summary>
        /// <param name="range">绝对范围</param>
        /// <returns></returns>
        public bool InRangeY(Vector2 range) => range.x > PlayerOffsetPosition.y && range.y < PlayerOffsetPosition.y;


        #region CurrentInRangeX_Y

        /// <summary>
        /// 返回玩家是否在指定的X范围内 (玩家偏移坐标) (相对偏移)
        /// </summary>
        /// <param name="offsetRange">基于 CurrentPosition.x 进行偏移</param>
        /// <returns></returns>
        public virtual bool CurrentInRangeX(Vector2 offsetRange) => InRangeX(new Vector2(CurrentPosition.x - offsetRange.x, CurrentPosition.x + offsetRange.y));

        /// <summary>
        /// 返回玩家是否在指定的Y范围内 (玩家偏移坐标) (相对偏移)
        /// </summary>
        /// <param name="offsetRange">基于 CurrentPosition.y 进行偏移</param>
        /// <returns></returns>
        public virtual bool CurrentInRangeY(Vector2 offsetRange) => InRangeY(new Vector2(CurrentPosition.y - offsetRange.x, CurrentPosition.y + offsetRange.y));

        #endregion

        #endregion

        #region InLeft_Right_Up_Down

        /// <summary>
        /// 返回目标是否在玩家的左边 (玩家偏移坐标)
        /// </summary>
        /// <param name="position">目标绝对位置</param>
        /// <returns></returns>
        public bool InPlayerLeft(Vector3 position) => PlayerOffsetPosition.x > position.x;

        /// <summary>
        /// 返回目标是否在玩家的左边 (玩家偏移坐标)
        /// </summary>
        /// <returns></returns>
        public virtual bool InPlayerLeft() => PlayerOffsetPosition.x > CurrentPosition.x;

        /// <summary>
        /// 返回目标是否在玩家的右边 (玩家偏移坐标)
        /// </summary>
        /// <param name="position">目标绝对位置</param>
        /// <returns></returns>
        public bool InPlayerRight(Vector3 position) => PlayerOffsetPosition.x < position.x;

        /// <summary>
        /// 返回目标是否在玩家的右边 (玩家偏移坐标)
        /// </summary>
        /// <returns></returns>
        public virtual bool InPlayerRight() => PlayerOffsetPosition.x < CurrentPosition.x;

        /// <summary>
        /// 返回目标是否在玩家的上边 (玩家偏移坐标)
        /// </summary>
        /// <param name="position">目标绝对位置</param>
        /// <returns></returns>
        public bool InPlayerUp(Vector3 position) => PlayerOffsetPosition.y > position.y;

        /// <summary>
        /// 返回目标是否在玩家的上边 (玩家偏移坐标)
        /// </summary>
        /// <returns></returns>
        public virtual bool InPlayerUp() => PlayerOffsetPosition.y > CurrentPosition.y;

        /// <summary>
        /// 返回目标是否在玩家的下边 (玩家偏移坐标)
        /// </summary>
        /// <param name="position">目标绝对位置</param>
        /// <returns></returns>
        public bool InPlayerDown(Vector3 position) => PlayerOffsetPosition.y < position.y;

        /// <summary>
        /// 返回目标是否在玩家的下边 (玩家偏移坐标)
        /// </summary>
        /// <returns></returns>
        public virtual bool InPlayerDown() => PlayerOffsetPosition.y < CurrentPosition.y;

        #endregion

        #endregion
    }
}