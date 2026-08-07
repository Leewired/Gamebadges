using UnityEngine;
using StateMachine.Core;
using MazeGame.Components;
using StateMachine.States.Enemy;
using MazeGame.Core;

namespace MazeGame.Core
{
	public class Enemy: BaseCharacter
	{
		public StateMachine.Core.StateMachine m_stateMachine = null;
		public EnemyComponent m_comp = null;
		
		private Vector3 m_velocity = Vector3.zero;

        public Enemy(EnemyComponent comp)
		{
			m_comp = comp;
			this.m_characterInstance = comp.gameObject;
			this.m_rigidBody = this.m_characterInstance.GetComponent<Rigidbody>();
			SetupStateMachine();
		}

		private void SetupStateMachine()
		{
			m_stateMachine = new StateMachine.Core.StateMachine();
			InitState initState = new InitState(m_stateMachine);
			RoamingState roamingState = new RoamingState(m_stateMachine);

			RoamingConnection roamingConnection = new RoamingConnection(m_stateMachine);
			roamingConnection.m_state = roamingState;
			initState.AddOutputConnection(roamingConnection);

			m_stateMachine.m_currentState = initState;
		}

		public void Start()
        {
            m_stateMachine.StartStateMachine();
            Game.m_enemy.m_comp.m_agent.isStopped = false;
            Game.m_enemy.m_comp.m_agent.velocity = m_velocity;
        }

        public void Update()
		{
			m_stateMachine.UpdateStateMachine();
		}

        public void Stop()
        {
            m_stateMachine.StopStateMachine();
            Game.m_enemy.m_comp.m_agent.isStopped = true;
			m_velocity = Game.m_enemy.m_comp.m_agent.velocity; //store velocity for continued action when resuming
            Game.m_enemy.m_comp.m_agent.velocity = Vector3.zero;
        }

    }
}