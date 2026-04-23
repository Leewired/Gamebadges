using System;
using UnityEngine;

namespace StateMachine.Core
{

    public class BaseConnection
    {
        public Guid m_guid = Guid.Empty;
        public BaseState m_state = null;
        public StateMachine m_fsm = null;

        public BaseConnection(StateMachine fsm)
        {
            m_fsm = fsm;
            m_guid = Guid.NewGuid();
            Debug.Log(string.Format("{0} created with a guid: {1}.", this.GetType(), m_guid.ToString()));
        }

        virtual public bool Condition()
        {
            return false;
        }

    }
}