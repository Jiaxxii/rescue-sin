using System;
using UnityEngine;
using Workspace.FiniteStateMachine;
using Range = Workspace.FiniteStateMachine.Range;

namespace Workspace.Enemy.YuKFsmLogic
{
    public class YuKMove : BaseState<YuKState, IYuK, YuKMove.MoveProperty>
    {
        [Serializable]
        public class MoveProperty
        {
            [Header("检测角色进入的范围")] [SerializeField] [Tooltip("应该设置为角色面向的坐标")]
            private Transform frontPoint;

            [SerializeField] [Tooltip("偏移面向的退出点 防止与idle的值发送冲突")]
            private float frontOffset;

            [SerializeField] [Tooltip("应该设置为角色背向的坐标")]
            private Transform rearPoint;

            [SerializeField] [Tooltip("偏移背向的退出点 防止与idle的值发送冲突")]
            private float rearOffset;


            [SerializeField] private float speed;

            [SerializeField] private float outDistance;


            public float Speed => speed;

            public float FrontOffset => frontOffset;
            public float RearOffset => rearOffset;
            public float OutDistance => outDistance;


            public Transform FrontPoint => frontPoint;
            public Transform RearPoint => rearPoint;
        }

        public YuKMove(IYuK resources, MoveProperty privateRes) : base(resources, privateRes)
        {
        }

        public override YuKState State => YuKState.Move;


        private Vector3 _direction;

        public override void OnEnter()
        {
            Resources.YukAnimator.Play("run");
        }

        public override void OnUnityUpdate()
        {
            // 获取玩家向“我”走来的方向 向这个方向移动
            _direction = Resources.GetPlayerHorizontalDirection() != Vector3.left ? Vector3.left : Vector3.right;


            if (Resources.Distance() > PrivateRes.OutDistance)
            {
                Resources.ChangeState(YuKState.NotLookPlayer);
            }
        }

        public override void OnUnityFixedUpdate()
        {
            Resources.Transform.position += _direction * (PrivateRes.Speed * Time.fixedDeltaTime);
        }

        public override void OnExit()
        {
            _direction = Vector3.zero;
            Resources.YukAnimator.Play("idle");
        }
    }
}