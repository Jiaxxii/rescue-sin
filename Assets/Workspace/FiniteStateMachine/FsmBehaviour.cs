using System.Collections.Generic;
using UnityEngine;

namespace Workspace.FiniteStateMachine
{
    public class FsmBehaviour<TState, TResources> : MonoBehaviour
        where TState : System.Enum
    {
        protected StateMachine<TState, TResources> StateMachine;

        private readonly Dictionary<TState, IState<TState>> _stateMap = new();

        protected virtual TState CurrentState { get; set; }

        protected virtual void Update()
        {
            StateMachine?.Update();
        }


        protected void Add(TState stateEnum, IState<TState> stateObject)
        {
            if (!_stateMap.TryAdd(stateEnum, stateObject))
            {
                Debug.LogWarning($"重复添加的状态\"{stateEnum}\"");
            }
        }


        public virtual void ChangedState(TState state)
        {
            if (!_stateMap.TryGetValue(state, out var stateObject))
            {
                Debug.LogError($"没有定义的状态\"{state}\"");
                return;
            }

            CurrentState = stateObject.State;
            StateMachine.ChangeState(stateObject);
        }
    }
}