using System;
using UnityEngine;
using Workspace.FiniteStateMachine;

namespace Workspace.Friendly.SinFsmLogic
{
    public class SinLookPlayer : BaseState<SinState, ISin, SinLookPlayer.LookPlayerProperty>
    {
        [Serializable]
        public class LookPlayerProperty
        {
            [SerializeField] private float rotateTowardsSpeed;
            public float RotateTowardsSpeed => rotateTowardsSpeed;
        }

        public SinLookPlayer(ISin resources, LookPlayerProperty privateRes) : base(resources, privateRes)
        {
        }

        public override SinState State => SinState.LookPlayer;

        private float _currentLocalScaleX;

        public override void OnEnter()
        {
            // 计入一下一开始“面向”的方向
            _currentLocalScaleX = Resources.Transform.localScale.x;
        }


        public override void OnUnityUpdate()
        {
            // 判断是否面向玩家
            if (IsLookPlayer())
            {
                Resources.ChangeState(SinState.MoveToPlayer);
                return;
            }

            // 不面向玩家就先面向玩家 不移动
            var ls = Resources.Transform.localScale;

            Resources.Transform.localScale = new Vector3(Mathf.MoveTowards(ls.x, -_currentLocalScaleX, Time.deltaTime * PrivateRes.RotateTowardsSpeed),
                ls.y, ls.z);
        }

        public override void OnExit()
        {
        }


        private bool IsLookPlayer(float tolerance = 0.001F)
        {
            var playerDir = Resources.GetPlayerHorizontalDirection();
            var currentDirection = Resources.Transform.localScale.x;

            // 判断角色是否需要转身  
            if (playerDir == Vector3.left)
            {
                // 如果玩家在左边，角色应该面向左（-1）  
                return Mathf.Abs(currentDirection + 1) < tolerance;
            }

            if (playerDir == Vector3.right)
            {
                // 如果玩家在右边，角色应该面向右（1）  
                return Mathf.Abs(currentDirection - 1) < tolerance;
            }

            // 如果没有有效的方向信息，返回false或者处理异常情况  
            return false;
        }
    }
}