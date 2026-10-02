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
            if(Game.m_player.m_playerBeatLevel)
            {
                Game.m_endController.ShowVictory();
            }
            else
            {
                Game.m_endController.ShowGameOver();
            }
            //reset game data
            Game.m_player.m_playerBeatLevel = false;
            Game.m_gameData.m_playerHasKey = false;
            Game.m_gameData.m_playerBeatLevel = false;
            Game.m_gameData.WriteData();
        }

        public override void StopState()
        {
            base.StopState();
            Game.m_endController.Hide();
        }
    }
}
