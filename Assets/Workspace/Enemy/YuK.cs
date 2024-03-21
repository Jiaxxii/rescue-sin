using System;
using UnityEngine;
using Workspace.Enemy.YuKFsmLogic;
using Workspace.FiniteStateMachine;
using Workspace.FiniteStateMachine.ExpandInterface;

namespace Workspace.Enemy
{
    public interface IYuK : IPlayerPosition
    {
        Animator YukAnimator { get; }
        public Transform Transform{ get; }
        void ChangedState(YuKState state);
    }

    public class YuK : FsmBehaviour<YuKState, IYuK>, IYuK
    {
        [SerializeField] private Animator yuKAnimator;
        [Header("默认状态")] [SerializeField] private YuKIdle.IdleProperty idleProperty;
        [Header("玩家进入范围")] [SerializeField] private YuKPlayerInRange.PlayerInRangeProperty playerInRangeProperty;

        public Animator YukAnimator => yuKAnimator;

        public Transform Transform => transform;

        private void Awake()
        {
            StateMachine = new StateMachine<YuKState, IYuK>(this);
        }

        private void Start()
        {
            StateMachine.Add(new YuKIdle(this, idleProperty));
            StateMachine.Add(new YuKPlayerInRange(this, playerInRangeProperty));
            ChangedState(YuKState.Idle);
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }
    }
}