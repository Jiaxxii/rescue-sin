using UnityEngine;
using Workspace.FiniteStateMachine;
using Workspace.FiniteStateMachine.ExpandInterface;
using Workspace.FsmObjects.Friendly.SinFsmLogic;
using Workspace.ScriptableObjectData;

namespace Workspace.FsmObjects.Friendly
{
    public interface ISin : IPlayerPosition
    {
        Transform Transform { get; }
        AnimationLibrary AniLibrary { get; }
        Animator Animator { get; }

        void ChangeState(SinState state);
    }

    public class Sin : FsmBehaviour<SinState, ISin>, ISin
    {
        [SerializeField] private SinIdle.IdleProperty idleProperty;
        [SerializeField] private SinLookPlayer.LookPlayerProperty lookPlayerProperty;
        [SerializeField] private SinMoveToPlayer.MoveToPlayerProperty moveToPlayerProperty;
        [SerializeField] private Animator animator;

        public AnimationLibrary AniLibrary { get; private set; }

        public Animator Animator => animator;

        public Transform Transform => transform;

        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine<SinState, ISin>(this);
            StateMachine.Add(new SinIdle(this, idleProperty));
            StateMachine.Add(new SinLookPlayer(this, lookPlayerProperty));
            StateMachine.Add(new SinMoveToPlayer(this, moveToPlayerProperty));

            AniLibrary = Resources.Load<AnimationLibrary>("SO/AnimationLibrary");
        }

        private void Start()
        {
            StateMachine.ChangeState(SinState.Idle);
        }
    }
}