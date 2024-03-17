using System;
using UnityEngine;
using Workspace.Enemy.YuKFsmLogic;
using Workspace.FiniteStateMachine;

namespace Workspace.Enemy
{
    public interface IYuK
    {
        public Vector3 PlayerOffsetPosition { get; }
    }

    public class YuK : FsmBehaviour<YuKState, IYuK>, IYuK
    {

        [Header("默认状态")] [SerializeField] private YuKIdle.IdleProperty idleProperty;


        private void Awake()
        {
            StateMachine = new StateMachine<YuKState, IYuK>(this);
        }


        private void Start()
        {
            Add(new YuKIdle(this, idleProperty));
            ChangedState(YuKState.Idle);
        }
    }
}