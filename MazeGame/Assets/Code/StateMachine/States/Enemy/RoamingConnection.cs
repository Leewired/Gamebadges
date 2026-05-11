using UnityEngine;
using System.Collections;
using StateMachine.Core;

namespace StateMachine.States.Enemy
{
	public class RoamingConnection: BaseConnection
	{

        public RoamingConnection(Core.StateMachine fsm) : base(fsm)
        {

        }

        public override bool Condition()
        {
            ParameterBool p = (ParameterBool)this.m_fsm.GetParameter("EnemyRoaming"); //Cast to parameterBool as it's getting a baseParameter
            if (p == null)
            {
                return false;
            }
            return p.m_value;
        }

    }
}