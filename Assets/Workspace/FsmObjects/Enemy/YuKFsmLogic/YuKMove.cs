using System;
using UnityEngine;
using Workspace.EditorAttribute;
using Workspace.FiniteStateMachine;

namespace Workspace.FsmObjects.Enemy.YuKFsmLogic
{
    public class YuKMove : BaseState<YuKState, IYuK, YuKMove.MoveProperty>
    {
        [Serializable]
        public class MoveProperty
        {
            [RewriteName("移动速度")] [SerializeField] private float speed;

            [RewriteName("逃离距离", "此值最好大于进入范围的2倍")] [SerializeField]
            private float outDistance;

            public float Speed => speed;


            public float OutDistance => outDistance;
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
            Resources.Transform.position += _direction * (PrivateRes.Speed * Time.deltaTime);


            if (Resources.Distance() > PrivateRes.OutDistance)
            {
                Resources.ChangeState(YuKState.Look);
            }
        }


        public override void OnExit()
        {
            _direction = Vector3.zero;
            Resources.YukAnimator.Play("idle");
        }
    }
}