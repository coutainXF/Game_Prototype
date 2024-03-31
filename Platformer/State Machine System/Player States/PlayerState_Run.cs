using UnityEngine;
using UnityEngine.InputSystem;

namespace State_Machine_System.Player_States
{
    [CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Run",fileName = "PlayerState_Run")]
    public class PlayerState_Run : PlayerState
    {
        [SerializeField] private float runSpeed = 5f;//序列化移动速度     ScriptableObject会生成对应的可编程实例
        [SerializeField] private float acceleration = 5f;//序列化增加速度
        
        public override void Enter()
        {
            base.Enter();

            currentSpeed = _playerController.MoveSpeed;//当前速度
        }

        public override void LogicUpdate()
        {
            if (!_playerInput.Move)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Idle));
            }
            
            if (_playerInput.Jump)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Jumpup));
            }

            //当玩家处于跑步状态没有接触地面，不再转换到下落状态转而转换到土狼时间状态
            if (!_playerController.isGrounded)
            {
                _stateMachine.SwitchState(typeof(PlayerState_CoyoteTime));
            }
            
            currentSpeed = Mathf.MoveTowards(currentSpeed, runSpeed, acceleration * Time.deltaTime);
        }

        public override void PhysicUpdate()
        {
            _playerController.Move(currentSpeed);
        }
    }
}