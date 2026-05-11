using UnityEngine;
using StateMachine.Core;
using MazeGame.Core;

namespace StateMachine.States.Enemy
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
			base.UpdateState();
            if (!m_isMoving)
			{
				Vector3 rpos = Game.m_maze.GetRandomCell(Time.renderedFrameCount);
				Game.m_enemy.m_comp.m_agent.destination = rpos;
				m_isMoving = true;
			}
			if (Game.m_enemy.m_comp.m_agent.remainingDistance < 0.01f)
			{
				m_isMoving = false;
			}
			Game.m_gameData.m_enemyPosition = Game.m_enemy.m_comp.m_agent.transform.position;
			Game.m_gameData.m_enemyDestination = Game.m_enemy.m_comp.m_agent.destination;
        }

    }
}