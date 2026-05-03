using UnityEngine;
using StateMachine.Core;

namespace StateMachine.States
{
	public class GameplayConnection : BaseConnection
	{

		public GameplayConnection(StateMachine.Core.StateMachine fsm) : base(fsm)
		{ }


        public override bool Condition()
		{

			ParameterBool p = (ParameterBool)this.m_fsm.GetParameter("Gameplay");
			if (p == null)
			{
				return false;
			}
			return p.m_value;
		}
	}
}