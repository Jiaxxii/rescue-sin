using System.Collections.Generic;
using UnityEngine;

namespace Workspace.FiniteStateMachine
{
    public class StateMachine<TState, TResources>
        where TState : System.Enum
    {
        private readonly Dictionary<TState, IState<TState>> _stateMap = new();
        private IState<TState> _currentState;

        public TResources Resources { get; private set; }
        
        public TState CurrentState { get; set; }


        public StateMachine(TResources resources)
        {
            Resources = resources;
        }

        public void Add(IState<TState> stateObject)
        {
            var state = stateObject.State;
            if (!_stateMap.TryAdd(state, stateObject))
            {
                Debug.LogWarning($"重复添加的状态\"{state}\"");
            }
        }
        public void ChangeState(TState state)
        {
            if (!_stateMap.TryGetValue(state, out var stateObject))
            {
                Debug.LogError($"没有定义的状态\"{state}\"");
                return;
            }
            
            CurrentState = stateObject.State;
            
            _currentState?.OnExit();

            _currentState = stateObject;
            _currentState.OnEnter();
        }

       

        public void Update()
            => _currentState?.OnUnityUpdate();

        public void FixedUpdate()
            => _currentState?.OnUnityFixedUpdate();

        public void LateUpdate()
            => _currentState?.OnUnityLateUpdate();
    }
}