using System;
using System.Collections.Generic;
using UnityEngine;

namespace State_Machine_System.Base
{
    public class StateMachine : MonoBehaviour
    {
        private IState _currentState;
        protected Dictionary<System.Type, IState> stateTable;

        private void Update()
        {
            _currentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            _currentState.PhysicUpdate();
        }

        protected void SwitchOn(IState newState)
        {
            _currentState = newState;
            _currentState.Enter();  
        }

        public void SwitchState(IState newState)
        {
            _currentState.Exit();
            SwitchOn(newState);
        }

        public void SwitchState(System.Type newStateType)
        {
            SwitchState(stateTable[newStateType]);
        }
    }
}
