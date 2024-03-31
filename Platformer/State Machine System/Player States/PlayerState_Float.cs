using UnityEngine;
namespace State_Machine_System.Player_States
{
    [CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Float",fileName = "PlayerState_Float")]
    public class PlayerState_Float : PlayerState
    {
        [SerializeField] private VoidEventChannel lostEventChannel;
        public override void Enter()
        {
            base.Enter();
            lostEventChannel.Broadcast();
        }
    }

}