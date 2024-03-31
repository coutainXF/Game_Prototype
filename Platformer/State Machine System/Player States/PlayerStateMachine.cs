using System;
using System.Collections.Generic;
using State_Machine_System.Base;
using UnityEngine;

namespace State_Machine_System.Player_States
{
    public class PlayerStateMachine : StateMachine
    {
        //public PlayerState_Idle idleState;
        //public PlayerState_Run runState;
        //一个个地初始化显然不符合

        [SerializeField] PlayerState[] states;
        private Animator _animator;
        private PlayerInput _playerInput;
        private PlayerController _playerController;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _playerInput = GetComponent<PlayerInput>();
            _playerController = GetComponent<PlayerController>();
            //Do player states initialization here
            //idleState.Initialize(_animator,this);
            //runState.Initialize(_animator,this);
            stateTable = new Dictionary<Type, IState>(states.Length);
            foreach (var state in states)
            {
                state.Initialize(_animator,_playerInput,_playerController,this);
                stateTable.Add(state.GetType(),state);
            }
        }

        private void Start()
        {
            SwitchOn(stateTable[typeof(PlayerState_Idle)]);//根据类型从字典中去到具体的实例
        }
    }
}
