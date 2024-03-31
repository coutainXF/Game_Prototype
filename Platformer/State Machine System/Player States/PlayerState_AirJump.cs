using UnityEngine;

namespace State_Machine_System.Player_States
{
    [CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/AirJump",fileName = "PlayerState_AirJump")]
    public class PlayerState_AirJump : PlayerState
    {
        [SerializeField] private float jumpForce = 7f;//跳跃力度
        [SerializeField] private float moveSpeed = 5f;//移动速度
        [SerializeField] private ParticleSystem jumpVFX;
        [SerializeField] private AudioClip jumpSFX;//跳跃语音
        public override void Enter()
        {
            base.Enter();
            _playerController.audioSource.PlayOneShot(jumpSFX);
            _playerController.SetVelocityY(jumpForce);
            _playerController.canAirJump = false;
            Instantiate(jumpVFX,_playerController.transform.position,Quaternion.identity);
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