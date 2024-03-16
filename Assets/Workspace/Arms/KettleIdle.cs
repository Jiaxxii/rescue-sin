using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Workspace.FiniteStateMachine;

namespace Workspace.Arms
{
    public class KettleIdle : BaseState<KettleState, IKettle, KettleIdle.IdleProperty>
    {
        public KettleIdle(IKettle resources, IdleProperty privateRes) : base(resources, privateRes)
        {
        }

        [System.Serializable]
        public class IdleProperty
        {
            [SerializeField] [Tooltip("定义玩家与水壶的距离最远超出")]
            private float idleOutRange;

            [SerializeField] [Tooltip("定义水壶偏移的Y坐标")]
            private float offsetY;

            [SerializeField] [Tooltip("水壶一次往返的时间")]
            private float duration;

            [SerializeField] [Tooltip("水壶运动模式")] private Ease moveEase;

            [SerializeField] [Tooltip("水壶运动模式")] private AnimationCurve moveCurve;

            public float IdleOutRange => idleOutRange;
            public float OffsetY => offsetY;

            public float Duration => duration;

            public Ease MoveEase => moveEase;

            public AnimationCurve MoveCurve => moveCurve;
        }


        public override KettleState State => KettleState.Idle;


        public override void OnEnter()
        {
            Resources.Transform.DOLocalMoveY(PrivateRes.OffsetY, PrivateRes.Duration * .5F)
                .SetEase(PrivateRes.MoveCurve, PrivateRes.MoveEase)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public override void OnUnityUpdate()
        {
            // 默认状态下于玩家分开一定的距离时推出状态
            if (Resources.GetPlayerFloatDistance() >= PrivateRes.IdleOutRange)
            {
                Resources.ChangedState(KettleState.MoveToPlayer);
            }
        }


        public override void OnExit()
        {
            Resources.Transform.DOKill();
        }
    }
}