using System;
using State_Machine_System.Base;
using UnityEngine;

namespace State_Machine_System.Player_States
{
    public class PlayerState : ScriptableObject,IState
    {
        [SerializeField] private string stateName;
        [SerializeField,Range(0f,1f)] private float transitionDuration = 0.1f;
        
        protected float currentSpeed;//代表当前速度
        protected Animator _animator;
        protected PlayerInput _playerInput;
        protected PlayerStateMachine _stateMachine;
        protected PlayerController _playerController;
        
        protected int stateHash;
        private float stateStartTime;

        protected bool IsAnimationFinished => StateDuration >= _animator.GetCurrentAnimatorStateInfo(0).length;
        protected float StateDuration => Time.time - stateStartTime;

        public void Initialize(Animator animator, PlayerInput playerInput,PlayerController playerController, PlayerStateMachine stateMachine)
        {
            this._playerController = playerController; 
            this._playerInput = playerInput;
            this._animator = animator;
            this._stateMachine = stateMachine;
        }

        private void OnEnable()
        {
            stateHash = Animator.StringToHash(stateName);
        }

        public virtual void Enter()
        {
            _animator.CrossFade(stateHash,transitionDuration);//通过动画哈希播放动画
            stateStartTime = Time.time;
        }

        public virtual void Exit()
        {

        }

        public virtual void LogicUpdate()
        {

        }

        public virtual void PhysicUpdate()
        {
            
        }
    }
}
