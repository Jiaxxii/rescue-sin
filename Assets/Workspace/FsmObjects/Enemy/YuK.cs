using System;
using System.Collections.Generic;
using UnityEngine;
using Workspace.EditorAttribute;
using Workspace.FiniteStateMachine;
using Workspace.FiniteStateMachine.ExpandInterface;
using Workspace.FsmObjects.AnimationAction;
using Workspace.FsmObjects.Arms;
using Workspace.FsmObjects.Arms.KettleObject;
using Workspace.FsmObjects.Enemy.YuKFsmLogic;

namespace Workspace.FsmObjects.Enemy
{
    public interface IYuK : IPlayerPosition
    {
        Animator YukAnimator { get; }
        AudioSource AudioSource { get; }
        public Transform Transform { get; }

        public Transform FrontPoint { get; }
        public Transform RearPoint { get; }
        public TargetCollider Target { get; }
        void ChangeState(YuKState state);

        Vector2 DistanceVector2(Vector3 pos1, Vector3 pos2);

        Vector2 GetOffset(string tagName);

        public ( float? forward, float? rear) Unification(float? forward, float? rear);
    }

    public class YuK : FsmBehaviour<YuKState, IYuK>, IYuK, IHurt
    {
        [Header("检测角色进入的范围")] [SerializeField] [RewriteName("身前坐标", "应该设置为角色面向的坐标")]
        private Transform frontPoint;

        [SerializeField] [RewriteName("身后坐标", "应该设置为角色背向的坐标")]
        private Transform rearPoint;

        [SerializeField] private Animator yuKAnimator;
        [SerializeField] private AudioSource audioSource;


        [Header("默认状态")] [SerializeField] private YuKIdle.IdleProperty idleProperty;

        [Header("转身")] [SerializeField] private YuKLookAt.LookAtProperty lookAtProperty;

        [Header("逃离")] [SerializeField] private YuKMove.MoveProperty moveProperty;
        [Header("受伤(电击)")] [SerializeField] private YuKHurt.HurtProperty hurtProperty;

        [SerializeField] private List<OffsetProperty> offsetProperty;


        public Transform FrontPoint => frontPoint;
        public Transform RearPoint => rearPoint;

        public Animator YukAnimator => yuKAnimator;
        public AudioSource AudioSource => audioSource;

        public Transform Transform => transform;

        public TargetCollider Target { get; private set; }

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
            StateMachine.Add(new YuKHurt(this, hurtProperty));
            ChangeState(YuKState.Idle);
        }


        public (float? forward, float? rear) Unification(float? forward, float? rear)
        {
            return transform.localScale.x > 0 ? (forward, rear) : (rear, forward);
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public Vector2 DistanceVector2(Vector3 pos1, Vector3 pos2)
        {
            return new Vector2(Mathf.Abs(pos1.x - pos2.x), Mathf.Abs(pos1.y - pos2.y));
        }


        public void Hurt(GameObject other)
        {
            if (Target == null) Target = new TargetCollider(other);
            else Target.UpDate(other);

            ChangeState(YuKState.Hurt);
        }

        public Vector2 GetOffset(string tagName) => offsetProperty.FindConfig(v => v.Name == tagName).Offset;
    }
}