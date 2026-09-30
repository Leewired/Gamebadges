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
            Game.m_endController.ShowGameEnd(true); //TODO: make this a parameter for win or lose
            Game.m_gameData.m_playerHasKey = Game.m_player.m_hasKey;
            Game.m_gameData.WriteData();
        }

        public override void StopState()
        {
            base.StopState();
            Game.m_endController.Hide();
        }
    }
}
