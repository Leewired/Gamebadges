using UnityEngine;
using StateMachine.Core;
using MazeGame.Components;
using StateMachine.States.Enemy;

namespace MazeGame.Core
{
	public class Enemy: BaseCharacter
	{
		public StateMachine.Core.StateMachine m_stateMachine = null;
		public EnemyComponent m_comp = null;

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
			//RoamingState roamingState = new RoamingState(m_stateMachine);
			//TODO: roamingState
			RoamingConnection roamingConnection = new RoamingConnection(m_stateMachine);
		}

		public void Update()
		{
			m_stateMachine.UpdateStateMachine();
		}
		
	}
}