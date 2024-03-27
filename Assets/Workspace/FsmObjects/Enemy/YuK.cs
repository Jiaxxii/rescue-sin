using UnityEngine;
using Workspace.EditorAttribute;
using Workspace.FiniteStateMachine;
using Workspace.FiniteStateMachine.ExpandInterface;
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

        void ChangeState(YuKState state);

        public ( float? forward, float? rear) Unification(float? forward, float? rear);
    }

    public class YuK : FsmBehaviour<YuKState, IYuK>, IYuK
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


        public Transform FrontPoint => frontPoint;
        public Transform RearPoint => rearPoint;

        public Animator YukAnimator => yuKAnimator;
        public AudioSource AudioSource => audioSource;

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