using UnityEngine;
namespace State_Machine_System.Player_States
{
    [CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Defeated",fileName = "PlayerState_Defeated")]
    public class PlayerState_Defeated : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            // 播放特效
            
            // 播放音效
            
        }

        public override void LogicUpdate()
        {
            if (IsAnimationFinished)
            {
                _stateMachine.SwitchState(typeof(PlayerState_Float));
            }
            
        }
    }

}
