using UnityEngine;

namespace State_Machine_System.Player_States
{
    [CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Jumpup",fileName = "PlayerState_Jumpup")]
    public class PlayerState_Jumpup : PlayerState
    {
        [SerializeField] private float jumpForce = 7f;//跳跃力度
        [SerializeField] private float moveSpeed = 5f;//移动速度
        [SerializeField] private AudioClip jumpSFX;//跳跃语音
        public override void Enter()
        {
            base.Enter();
            _playerController.audioSource.PlayOneShot(jumpSFX);
            _playerInput.HasJumpInputBuffer = false;//跳起来时不再有输入缓冲
            _playerController.SetVelocityY(jumpForce);
        }

        public override void LogicUpdate()
        {
            if (_playerInput.StopJump || _playerController.isFalling)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Fall));
            }
        }

        public override void PhysicUpdate()
        {
            _playerController.Move(moveSpeed);
        }
    }
}
