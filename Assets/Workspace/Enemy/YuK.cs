using System;
using UnityEngine;
using UnityEngine.Serialization;
using Workspace.Enemy.YuKFsmLogic;
using Workspace.FiniteStateMachine;
using Workspace.FiniteStateMachine.ExpandInterface;
using Range = Workspace.FiniteStateMachine.Range;

namespace Workspace.Enemy
{
    public interface IYuK : IPlayerPosition
    {
        Animator YukAnimator { get; }
        public Transform Transform { get; }
        void ChangeState(YuKState state);

        public ( float? forward, float? rear) Unification(float? forward, float? rear);
    }

    public class YuK : FsmBehaviour<YuKState, IYuK>, IYuK
    {
        [SerializeField] private Animator yuKAnimator;
        [Header("默认状态")] [SerializeField] private YuKIdle.IdleProperty idleProperty;
        [FormerlySerializedAs("notLookPlayerProperty")] [Header("转身")] [SerializeField] private YuKLookAt.LookAtProperty lookAtProperty;

        [Header("逃离")] [SerializeField] private YuKMove.MoveProperty moveProperty;

        //  [Header("玩家进入范围")] [SerializeField] private YuKPlayerInRange.PlayerInRangeProperty playerInRangeProperty;
        public Animator YukAnimator => yuKAnimator;

        public Transform Transform => transform;


        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine<YuKState, IYuK>(this);
        }

        private void Start()
        {
            StateMachine.Add(new YuKIdle(this, idleProperty));
            // StateMachine.Add(new YuKPlayerInRange(this, playerInRangeProperty));
            StateMachine.Add(new YuKLookAt(this, lookAtProperty));
            StateMachine.Add(new YuKMove(this, moveProperty));
            ChangeState(YuKState.Idle);
        }

        public void Unification(ref float? forward, ref float? rear)
        {
            if (transform.localScale.x > 0) return;
            (forward, rear) = (rear, forward);
        }

        public (float? forward, float? rear) Unification(float? forward, float? rear)
        {
            return transform.localScale.x > 0 ? (forward, rear) : (rear, forward);
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }
    }
}