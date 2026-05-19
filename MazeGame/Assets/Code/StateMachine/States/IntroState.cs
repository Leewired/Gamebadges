
using MazeGame.Core;
using StateMachine.Core;

namespace StateMachine.States
{
    public class IntroState : BaseState
    {
        public IntroState(Core.StateMachine fsm) : base(fsm)
        {
            m_stateMachine = fsm;
        }

        public override void StartState()
        {
            base.StartState();
            Game.m_introController.ShowMazeReady();
            Game.m_script.RunLevel();
            Game.m_input.OnAccept += M_input_OnAccept;
            
        }

        private void M_input_OnAccept()
        {
            m_stateMachine.AddParameter("Gameplay", true);
        }

        public override void StopState()
        {
            base.StopState();
            Game.m_introController.Hide();
            Game.m_input.OnAccept -= M_input_OnAccept;
        }
    }
}
