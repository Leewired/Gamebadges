

using StateMachine.Core;

namespace StateMachine.States
{
    public class IntroConnection : BaseConnection
    {

        public IntroConnection(Core.StateMachine fsm) : base(fsm)
        {
        }

        public override bool Condition()
        {
            ParameterBool p = (ParameterBool)this.m_fsm.GetParameter("Intro");
            if (p == null)
            {
                return false;
            }
            return p.m_value;
        }


    }
}
