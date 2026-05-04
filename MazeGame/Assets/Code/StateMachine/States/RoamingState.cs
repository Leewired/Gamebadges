using UnityEngine;
using System.Collections;
using StateMachine.Core;
using MazeGame.Core;

namespace StateMachine.States_Enemy
{
	public class RoamingState: BaseState
	{
		private bool m_isMoving = false;

		public RoamingState(StateMachine.Core.StateMachine fsm) : base(fsm)
		{
			m_stateMachine = fsm;
		}

        public override void UpdateState()
        {
            if (!m_isMoving)
			{
				Vector3 rpos = Game.m_maze.GetRandomCell(Time.renderedFrameCount);
				Game.m_enemy.m_comp.m_agent.destination = rpos;
				m_isMoving = true;
			}
			if (Game.m_enemy.m_comp.m_agent.remainingDistance < 0.01f)
			{

			}
        }

    }
}