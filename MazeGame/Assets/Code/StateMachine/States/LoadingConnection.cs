using UnityEngine;
using StateMachine.Core;

namespace StateMachine.Core
{

    public class LoadingConnection : BaseConnection
    {

        public LoadingConnection(Core.StateMachine fsm) :base(fsm) //calls base class constructor
        {

        }

        public override bool Condition()
        {
            ParameterBool p = (ParameterBool)this.m_fsm.GetParameter("Load");
            if (p != null)
            {
                return p.m_value;
            }
            return false;
        }

    }

}
