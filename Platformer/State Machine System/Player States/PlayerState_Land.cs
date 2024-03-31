using UnityEngine;

namespace State_Machine_System.Player_States
{
    [CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Land",fileName = "PlayerState_Land")]
    public class PlayerState_Land : PlayerState
    {
        [SerializeField] private float stiffTime = 0.2f;
        public override void Enter()
        {
            base.Enter();
            _playerController.SetVelocity(Vector3.zero);
        }

        public override void LogicUpdate()
        {
            if (_playerInput.HasJumpInputBuffer || _playerInput.Jump)//若存在跳跃缓冲时切换到跳跃状态
            {
                _stateMachine.SwitchState(typeof(PlayerState_Jumpup));
            }

            if (StateDuration < stiffTime)
            {//如果状态持续时间小于硬直时间，则直接return不执行下方的代码
                return;
            }
            
            if (_playerInput.Move)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Run));
            }

            if (IsAnimationFinished)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Idle));
            }
        }
    }
}
