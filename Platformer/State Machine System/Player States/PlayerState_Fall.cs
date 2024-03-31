using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State_Machine_System.Player_States
{
    [CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Fall",fileName = "PlayerState_Fall")]
    public class PlayerState_Fall : PlayerState
    {
        [SerializeField] private AnimationCurve speedCurve;
        [SerializeField] private float moveSpeed = 5f;//移动力度

        public override void LogicUpdate()
        {
            if (_playerController.isGrounded)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Land));
            }

            if (_playerInput.Jump)
            {
                if (_playerController.canAirJump)
                {
                    //switch to air jump state;
                    _stateMachine.SwitchState(typeof(PlayerState_AirJump));
                    return;;
                }
                //处于下落状态并按下跳跃按钮时存在跳跃缓冲
                //_playerInput.HasJumpInputBuffer = true;
                //开启协程来控制跳跃缓冲的时间，而非不限时间的开启跳跃缓冲。
                _playerInput.SetJumpInputBufferTimer();
            }

        }

        //对掉落速度的完全控制（使用动画曲线）
        public override void PhysicUpdate()
        {
            _playerController.SetVelocityY(speedCurve.Evaluate(StateDuration));
            _playerController.Move(moveSpeed);
        }
    }
}