using MazeGame.Core;
using StateMachine.Core;
using UnityEngine;

namespace StateMachine.States
{
    public class PauseState : BaseState
    {
        private ParameterBool m_gameplayParameter = null;

        public PauseState(Core.StateMachine fsm) : base(fsm)
        {
        }

        public override void StartState()
        {
            base.StartState();
            Game.m_pauseController.ShowGamePause();
            Game.m_input.OnPause += M_input_OnPause;
            m_gameplayParameter = m_stateMachine.GetParameter("Gameplay") as ParameterBool; // state machine gives as base parameter so we cast to bool which it is.
            m_gameplayParameter.m_value = false;
        }

        private void M_input_OnPause()
        {
            m_gameplayParameter.m_value = true;
        }

        public override void StopState()
        {
            base.StopState();
            Game.m_pauseController.Hide();
            Game.m_input.OnPause -= M_input_OnPause;
        }
    }
}
