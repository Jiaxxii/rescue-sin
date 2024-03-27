using System;
using UnityEngine;
using Workspace.FiniteStateMachine;

namespace Workspace.FsmObjects.Friendly.SinFsmLogic
{
    public class SinMoveToPlayer : BaseState<SinState, ISin, SinMoveToPlayer.MoveToPlayerProperty>
    {
        [Serializable]
        public class MoveToPlayerProperty
        {
            [SerializeField] private float exitForward;
            [SerializeField] private float exitRear;
            [SerializeField] private float speed;

            public float ExitForward => exitForward;

            public float ExitRear => exitRear;

            public float Speed => speed;
        }

        public SinMoveToPlayer(ISin resources, MoveToPlayerProperty privateRes) : base(resources, privateRes)
        {
        }

        public override SinState State => SinState.MoveToPlayer;

        private bool _isMoveIng;

        public override void OnEnter()
        {
        }

        public override void OnUnityUpdate()
        {
            // 面向玩家了 允许移动
            // 使用一个bool防止动画一直重复播放前几帧
            if (!_isMoveIng)
            {
                _isMoveIng = true;
                Resources.Animator.Play("sin_run");
            }

            Resources.Transform.position += PrivateRes.Speed * Time.deltaTime * Resources.GetPlayerHorizontalDirection();
            

            if (Resources.InRangeOffset(PrivateRes.ExitForward, PrivateRes.ExitRear, null, null))
            {
                Resources.ChangeState(SinState.Idle);
            }
        }

        public override void OnExit()
        {
            _isMoveIng = false;
        }
    }
}