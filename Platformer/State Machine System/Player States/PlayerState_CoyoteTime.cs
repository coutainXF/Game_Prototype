using UnityEngine;

namespace State_Machine_System.Player_States
{    
    [CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/CoyoteTime",fileName = "PlayerState_CoyoteTime")]
    public class PlayerState_CoyoteTime : PlayerState
    {
        [SerializeField] private float runSpeed = 5f;//序列化移动速度     ScriptableObject会生成对应的可编程实例
        [SerializeField] private float coyoteTime = 0.1f;//持续时间
        
        public override void Enter()
        {
            base.Enter();

            //disable the gravity affects
            _playerController.SetUseGravity(false);
        }

        public override void Exit()
        {
            //when exit, open the gravity affects
            _playerController.SetUseGravity(true);
        }

        public override void LogicUpdate()
        {
            if (_playerInput.Jump)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Jumpup));
            }

            //如果进入土狼时间状态且超过限制时间或者没有水平输入，将切换到下落状态
            if (StateDuration > coyoteTime || !_playerInput.Move)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Fall));
            }
        }
        public override void PhysicUpdate()
        {
            _playerController.Move(runSpeed);
        }
    }
}
