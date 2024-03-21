using System;
using UnityEngine;
using Workspace.FiniteStateMachine.ExpandInterface;

namespace Workspace.FiniteStateMachine
{
    public class FsmBehaviour<TState, TResources>
        : MonoBehaviour, IPlayerPosition
        where TState : Enum
    {
        protected StateMachine<TState, TResources> StateMachine;

        // private readonly Dictionary<TState, IState<TState>> _stateMap = new();

        [Header("玩家设置")] [SerializeField] protected Transform player;

        [Tooltip("玩家的坐标点进行偏移")] [SerializeField]
        protected Vector2 playerOffset;

        // protected virtual TState CurrentState { get; set; }

        public virtual Vector3 CurrentPosition => transform.position;

        public Vector3 PlayerOffsetPosition => new(player.position.x + playerOffset.x, player.position.y + playerOffset.y, player.position.z);

        protected virtual void Update()
        {
            StateMachine?.Update();
        }


        public void ChangedState(TState state) => StateMachine.ChangeState(state);


        public virtual float Distance() => Vector3.Distance(PlayerOffsetPosition, CurrentPosition);

        
        public virtual bool InRangeAs(Range? rangeX, Range? rangeY)
        {
            var resultX = true;
            var resultY = true;

            if (rangeX != null)
            {
                resultX = PlayerOffsetPosition.x >= rangeX.Value.Min && PlayerOffsetPosition.x <= rangeX.Value.Max;
            }

            if (rangeY != null)
            {
                resultY = PlayerOffsetPosition.y >= rangeY.Value.Min && PlayerOffsetPosition.y <= rangeY.Value.Max;
            }

            return resultX && resultY;
        }

        
        public virtual bool InRangeOffset(Range? offsetX, Range? offsetY)
        {
            Range? rangeX = null;
            Range? rangeY = null;
            if (offsetX != null) rangeX = new Range(CurrentPosition.x + offsetX.Value.Min, CurrentPosition.x + offsetX.Value.Max);
            if (offsetY != null) rangeY = new Range(CurrentPosition.y + offsetY.Value.Min, CurrentPosition.y + offsetY.Value.Max);
            return InRangeAs(rangeX, rangeY);
        }

        
        public float Distance(Vector3 position) => Vector3.Distance(PlayerOffsetPosition, position);
        
        
        public Vector2 GetPlayerDirection() => (CurrentPosition - PlayerOffsetPosition).normalized;
        

        public Vector2 GetPlayerHorizontalDirection(float tolerance = 0)
        {
            var horizontalDifference = PlayerOffsetPosition.x - CurrentPosition.x;

            if (Mathf.Abs(horizontalDifference) <= tolerance)
                return Vector2.zero; // 玩家在水平方向上的位置与敌人在容差范围内，认为没有方向移动  

            return horizontalDifference > 0 ? Vector2.right : Vector2.left;
        }
        
        
        public Vector2 GetPlayerVerticalDirection(float tolerance = 0)
        {
            var verticalDifference = PlayerOffsetPosition.y - CurrentPosition.x;

            if (Mathf.Abs(verticalDifference) <= tolerance)
                return Vector2.zero; // 玩家在垂直方向上的位置与敌人在容差范围内，认为没有方向移动  

            return verticalDifference > 0 ? Vector2.up : Vector2.down;
        }
    }
}