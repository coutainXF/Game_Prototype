namespace State_Machine_System.Base
{
    public interface IState
    {
        void Enter();

        void Exit();

        void LogicUpdate();

        void PhysicUpdate();
    }
}
