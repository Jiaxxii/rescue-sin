namespace Workspace.FiniteStateMachine
{
    public class StateMachine<TState, TResources>
        where TState : System.Enum
    {
        private IState<TState> _currentState;

        public TResources Resources { get; private set; }


        public StateMachine(TResources resources)
        {
            Resources = resources;
        }

        public void ChangeState(IState<TState> stateObject)
        {
            _currentState?.OnExit();

            _currentState = stateObject;
            _currentState.OnEnter();
        }

        public void Update()
            => _currentState?.OnUnityUpdate();

        public void FixeUpdate()
            => _currentState?.OnUnityFixeUpdate();

        public void LateUpdate()
            => _currentState?.OnUnityLateUpdate();
    }
}