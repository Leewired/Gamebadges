using System.Collections.Generic;
using System;
using UnityEngine;

namespace StateMachine.Core
{

    public class BaseState
    {
        public Guid m_guid = Guid.Empty;
        public List<BaseConnection> m_connections = null;
        public StateMachine m_stateMachine = null;

        public BaseState(StateMachine fsm)
        {
            m_guid = Guid.NewGuid();
            Debug.Log(string.Format("{0} created with a guid: {1}", this.GetType(), this.m_guid.ToString()));
            m_connections = new List<BaseConnection>();
            m_stateMachine = fsm;
        }

        public void AddOutputConnection(BaseConnection con)
        {
            this.m_connections.Add(con);
            Debug.Log(string.Format("{0} connection added with a guid: {1}", con.GetType(), con.m_guid.ToString()));
        }

        virtual public void StartState()
        {
            Debug.Log(string.Format("{0} with a guid {1} has started state.", this.GetType(), this.m_guid.ToString()));
        }

        virtual public void StopState()
        {
            Debug.Log(string.Format("{0} with a guid: {1} has stopped state", this.GetType(), this.m_guid.ToString()));
        }

        virtual public void UpdateState()
        {
            Debug.Log(string.Format("{0} with a guid: {1} has updated its state", this.GetType(), this.m_guid.ToString()));
        }

    }
}