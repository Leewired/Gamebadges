using MazeGame.Core;
using StateMachine.Core;

namespace StateMachine.States
{
    public class EndState : BaseState
    {
        public EndState(Core.StateMachine fsm) : base(fsm)
        {

        }

        public override void StartState()
        {
            base.StartState();
            Game.m_endController.ShowGameEnd();
        }

        public override void StopState()
        {
            base.StopState();
            Game.m_endController.Hide();
        }
    }
}
