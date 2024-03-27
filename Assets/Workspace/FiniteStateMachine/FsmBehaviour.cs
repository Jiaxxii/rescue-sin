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

        protected Transform Player;

        [Tooltip("玩家的坐标点进行偏移")] [SerializeField]
        protected Vector2 playerOffset;

        // protected virtual TState CurrentState { get; set; }

        public virtual Vector3 CurrentPosition => transform.position;

        public Vector3 PlayerOffsetPosition => new(Player.position.x + playerOffset.x, Player.position.y + playerOffset.y, Player.position.z);
        public Vector3 PlayerLocalScale => Player.localScale;
        public Vector3 CurrentLocalScale => transform.localScale;


        protected virtual void Update()
        {
            StateMachine?.Update();
        }

        protected virtual void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }


        public void ChangeState(TState state) => StateMachine.ChangeState(state);


        public virtual float Distance() => Vector3.Distance(PlayerOffsetPosition, CurrentPosition);

        public virtual Vector2 DistanceVector2()
        {
            return new Vector2(Mathf.Abs(PlayerOffsetPosition.x - CurrentPosition.y), Mathf.Abs(PlayerOffsetPosition.y - CurrentPosition.y));
        }


        public virtual bool InRangeAs(float? forwardX, float? rearX, float? upY, float? downY)
        {
            var inHorizontal = true;
            if (forwardX.HasValue && rearX.HasValue)
            {
                inHorizontal = PlayerOffsetPosition.x < forwardX.Value && PlayerOffsetPosition.x > rearX.Value;
            }

            var inVertical = true;
            if (upY.HasValue && downY.HasValue)
            {
                inVertical = PlayerOffsetPosition.y < upY.Value && PlayerOffsetPosition.x > downY.Value;
            }

            return inHorizontal && inVertical;
        }

        public virtual bool InRangeOffset(float? offsetForwardX, float? offsetRearX, float? offsetUpY, float? offsetDownY)
        {
            float? actualForwardX = null, actualRearX = null, actualUpY = null, actualDownY = null;

            if (offsetForwardX.HasValue && offsetRearX.HasValue)
            {
                actualForwardX = CurrentPosition.x + offsetForwardX.Value;
                actualRearX = CurrentPosition.x - offsetRearX.Value;
            }

            if (offsetUpY.HasValue && offsetDownY.HasValue)
            {
                actualUpY = CurrentPosition.y + offsetUpY.Value;
                actualDownY = CurrentPosition.y - offsetDownY.Value;
            }

            return InRangeAs(actualForwardX, actualRearX, actualUpY, actualDownY);
        }


        public float Distance(Vector3 position) => Vector3.Distance(PlayerOffsetPosition, position);


        public Vector3 GetPlayerDirection() => (CurrentPosition - PlayerOffsetPosition).normalized;


        public Vector3 GetPlayerHorizontalDirection(float tolerance = 0)
        {
            var horizontalDifference = PlayerOffsetPosition.x - CurrentPosition.x;

            if (Mathf.Abs(horizontalDifference) <= tolerance)
                return Vector2.zero; // 玩家在水平方向上的位置与敌人在容差范围内，认为没有方向移动  

            return horizontalDifference > 0 ? Vector2.right : Vector2.left;
        }


        public Vector3 GetPlayerVerticalDirection(float tolerance = 0)
        {
            var verticalDifference = PlayerOffsetPosition.y - CurrentPosition.x;

            if (Mathf.Abs(verticalDifference) <= tolerance)
                return Vector2.zero; // 玩家在垂直方向上的位置与敌人在容差范围内，认为没有方向移动  

            return verticalDifference > 0 ? Vector2.up : Vector2.down;
        }
    }
}