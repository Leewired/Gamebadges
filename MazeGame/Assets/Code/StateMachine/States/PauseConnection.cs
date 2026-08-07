

using StateMachine.Core;

namespace StateMachine.States
{
    public class PauseConnection : BaseConnection
    {

        public PauseConnection(Core.StateMachine fsm) : base(fsm)
        {
        }

        public override bool Condition()
        {
            ParameterBool p = (ParameterBool)this.m_fsm.GetParameter("Pause");
            if (p == null)
            {
                return false;
            }
            return p.m_value;
        }


    }
}
