using UnityEngine;
using MazeGame.Core;
using StateMachine.Core;

namespace StateMachine.States
{
	public class GameplayState: BaseState
	{
        private ParameterBool m_pauseParameter = null;

        public GameplayState(Core.StateMachine fsm): base(fsm)
        {
            
        }

        public override void StartState()
        {
            base.StartState();
            Game.m_input.OnMove += M_input_OnMove; //Assign delegate to new function
            Game.m_input.OnLook += M_input_OnLook;
            Game.m_input.OnJump += M_input_OnJump;
            Game.m_input.OnPause += M_input_OnPause;
            m_pauseParameter = m_stateMachine.GetParameter("Pause") as ParameterBool;
            m_pauseParameter.m_value = false; //Reset pause parameter
            Game.m_levelController.Activate();
            Game.m_player.Start();
            Game.m_enemy.Start();
            Game.m_gameData.m_scene = Game.m_levelController.gameObject.scene;
            Game.m_gameData.LoadData();
            //TODO: Add write to a button or game closing etc.
            //TODO: set player data based on load data
            //TODO: do it in a state
        }


        public override void UpdateState()
        {
            base.UpdateState();
            Game.m_enemy.Update();            
        }
        
        public override void StopState()
        {
            base.StopState();
            Game.m_player.Stop();
            Game.m_enemy.Stop();
            Game.m_input.OnMove -= M_input_OnMove;
            Game.m_input.OnLook -= M_input_OnLook;
            Game.m_input.OnJump -= M_input_OnJump;
            Game.m_input.OnPause -= M_input_OnPause;
        }

        private void M_input_OnLook(Vector2 v)
        {
            Game.m_player.TurnPlayer(v);
        }

        private void M_input_OnMove(Vector2 v)
        {
            Game.m_player.MovePlayer(v);
        }

        private void M_input_OnJump()
        {
            Game.m_player.JumpPlayer();
        }

        private void M_input_OnPause()
        {
            m_pauseParameter.m_value = true;
        }

    }
}