

using StateMachine.Core;

namespace StateMachine.States
{
    public class EndConnection : BaseConnection
    {

        public EndConnection(Core.StateMachine fsm) : base(fsm)
        {
        }

        public override bool Condition()
        {
            ParameterBool p = (ParameterBool)this.m_fsm.GetParameter("end");
            if (p == null)
            {
                return false;
            }
            return p.m_value;
        }


    }
}
