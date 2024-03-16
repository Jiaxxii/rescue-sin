using System;

namespace Workspace.FiniteStateMachine
{
    public abstract class BaseState<TState, TResources, TPrivate> : IState<TState>
        where TState : Enum
        where TPrivate : class
    {
        protected BaseState(TResources resources, TPrivate privateRes)
        {
            Resources = resources;
            PrivateRes = privateRes;
        }

        public abstract TState State { get; }

        protected readonly TResources Resources;
        protected readonly TPrivate PrivateRes;


        public abstract void OnEnter();

        public abstract void OnUnityUpdate();
        public abstract void OnExit();

        public virtual void OnUnityFixeUpdate()
        {
        }

        public virtual void OnUnityLateUpdate()
        {
        }
    }
}