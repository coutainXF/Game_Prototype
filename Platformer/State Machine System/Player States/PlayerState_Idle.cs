using State_Machine_System.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace State_Machine_System.Player_States
{
    [CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Idle",fileName = "PlayerState_Idle")]
    public class PlayerState_Idle : PlayerState
    {
        [SerializeField] private float decreasion = 5f;
        public override void Enter()
        {
            base.Enter();
            currentSpeed = _playerController.MoveSpeed;
        }

        public override void LogicUpdate()
        {
            if (_playerInput.Move)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Run));
            }
            
            if (_playerInput.Jump)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Jumpup));
            }
            
            if (!_playerController.isGrounded)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Fall));
            }
            
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, decreasion * Time.deltaTime);
        }

        public override void PhysicUpdate()
        {
            _playerController.SetVelocityX(currentSpeed * _playerController.transform.localScale.x);
        }
    }
}