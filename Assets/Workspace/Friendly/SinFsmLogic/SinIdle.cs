using System;
using UnityEngine;
using Workspace.EditorAttribute;
using Workspace.FiniteStateMachine;
using Workspace.ScriptableObjectData;
using Range = Workspace.FiniteStateMachine.Range;

namespace Workspace.Friendly.SinFsmLogic
{
    public class SinIdle : BaseState<SinState, ISin, SinIdle.IdleProperty>
    {
        [Serializable]
        public class IdleProperty
        {
            [SerializeField] private string groupName;
            [RewriteName("状态名称")] [SerializeField] private string stateName;

            [SerializeField] private float forward;
            [SerializeField] private float rear;

            public float Forward => forward;

            public float Rear => rear;

            public string GroupName => groupName;

            public string StateName => stateName;
        }

        public SinIdle(ISin resources, IdleProperty privateRes) : base(resources, privateRes)
        {
        }

        public override SinState State => SinState.Idle;

        private AnimationClips _animationClips;

        public override void OnEnter()
        {
            _animationClips =
                Resources.AniLibrary.GetAnimationClips(PrivateRes.GroupName, PrivateRes.StateName);
            Resources.Animator.Play(_animationClips.GetRandom());
        }

        public override void OnUnityUpdate()
        {
            if (!Resources.InRangeOffset(PrivateRes.Forward, PrivateRes.Rear, null, null))
            {
                Debug.Log("look");
                Resources.ChangeState(SinState.LookPlayer);
            }
        }

        public override void OnExit()
        {
        }
    }
}