using System;
using DG.Tweening;
using UnityEngine;
using Workspace.FiniteStateMachine;
using Workspace.Friendly;

namespace Workspace.Enemy.YuKFsmLogic
{
    public class YuKLookAt : BaseState<YuKState, IYuK, YuKLookAt.LookAtProperty>
    {
        [Serializable]
        public class LookAtProperty
        {
            [SerializeField] private float rotateTowardsSpeed;
            public float RotateTowardsSpeed => rotateTowardsSpeed;

            [Header("检测角色进入的范围")] [SerializeField] [Tooltip("应该设置为角色面向的坐标")]
            private Transform frontPoint;

            [SerializeField] [Tooltip("应该设置为角色背向的坐标")]
            private Transform rearPoint;


            public Transform FrontPoint => frontPoint;

            public Transform RearPoint => rearPoint;
        }

        public YuKLookAt(IYuK resources, LookAtProperty privateRes) : base(resources, privateRes)
        {
        }

        public override YuKState State => YuKState.NotLookPlayer;

        private (float? forward, float? rear) _range;
        private float _currentLocalScaleX;

        private Tween _tween;

        public override void OnEnter()
        {
            // 记录一开始的缩放
            _currentLocalScaleX = Resources.CurrentLocalScale.x;
            // 固定位置
            _range = Resources.Unification(PrivateRes.FrontPoint.position.x, PrivateRes.RearPoint.position.x);
        }


        public override void OnUnityUpdate()
        {
            // 先检查玩家是否在范围
            if (Resources.InRangeAs(_range.forward, _range.rear, null, null))
            {
                // 玩家在范围时 如果面向玩家 就转身看向空气
                if (IsLookPlayer() && _tween == null)
                {
                    // 向玩家就先转身 不移动
                    Turn();
                }

                if (_tween != null && _tween.IsComplete())
                {
                    // 没有面向玩家就逃离
                    Resources.ChangeState(YuKState.Move);
                }

                return;
            }

            // 不在范围时 判断是否面向玩家
            if (IsLookPlayer())
            {
                // 看向玩家就切换到默认
                Resources.ChangeState(YuKState.Idle);
                return;
            }

            if (_tween == null)
                // 看向玩家
                Turn();
        }

        private void Turn()
        {
            _tween = DOTween.To(
                    () => Resources.Transform.localScale,
                    v => Resources.Transform.localScale = v,
                    new Vector3(-_currentLocalScaleX, Resources.CurrentLocalScale.y, Resources.CurrentLocalScale.z), PrivateRes.RotateTowardsSpeed)
                .SetAutoKill(false);
        }

        public override void OnExit()
        {
            _tween.Kill();
            _tween = null;
        }


        private bool IsLookPlayer(float tolerance = 0.001F)
        {
            var playerDir = Resources.GetPlayerHorizontalDirection();
            var currentDirection = Resources.Transform.localScale.x;

            // 判断角色是否需要转身  
            if (playerDir == Vector3.left)
            {
                // 如果玩家在左边，角色应该面向左（-1）  
                return Math.Abs(currentDirection - (-1)) < tolerance;
            }

            if (playerDir == Vector3.right)
            {
                // 如果玩家在右边，角色应该面向右（1）  
                return Mathf.Abs(currentDirection - 1) < tolerance;
            }

            // 如果没有有效的方向信息，返回false或者处理异常情况  
            throw new ArgumentException();
        }
    }
}