using UnityEngine;
using MazeGame.Core;
using StateMachine.Core;

namespace StateMachine.States
{
	public class GameplayState: BaseState
	{

        public GameplayState(Core.StateMachine fsm): base(fsm)
            {
            
            }

        public override void StartState()
        {
            base.StartState();
            Game.m_input.OnMove += M_input_OnMove; //Assign delegate to new function
            Game.m_input.OnLook += M_input_OnLook;
            Game.m_levelController.Activate();
        }
        
        public override void StopState()
        {
            base.StopState();
            Game.m_input.OnMove -= M_input_OnMove;
            Game.m_input.OnLook -= M_input_OnLook;
        }

        private void M_input_OnLook(Vector2 v)
        {

            Game.m_player.TurnPlayer(v);

        }

        private void M_input_OnMove(Vector2 v)
        {

            Game.m_player.MovePlayer(v);

        }

	}
}