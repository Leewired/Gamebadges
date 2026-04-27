using System.Collections.Generic;
using UnityEngine;


namespace StateMachine.Core
{
    public class StateMachine
    {
        private bool m_update = false;
        private List<BaseParameter> m_parameters = null;
        public BaseState m_currentState = null;
        public BaseState m_previousState = null;

        public StateMachine()
        {
            
            Debug.Log(string.Format("{0} created.", this.GetType()));
            m_parameters = new List<BaseParameter>();

        }

        public void AddParameter(string name, object value)
        {

            if (value.GetType() == typeof(string))
            {

                ParameterString p = new ParameterString(name, (string)value);
                m_parameters.Add(p);

            }
            if (value.GetType() == typeof(bool))
            {

                ParameterBool p = new ParameterBool(name, (bool)value);
                m_parameters.Add(p);

            }

        }

        public BaseParameter GetParameter(string name)
        {
            foreach(BaseParameter p in m_parameters)
            {
                if (p.m_name.ToLower() == name.ToLower())
                {
                    return p;
                }
            }
            return null;
        }


        public void StartStateMachine()
        {

            m_update = true;

        }

        public void StopStateMachine()
        {

            m_update = false;

        }

        public void UpdateStateMachine()
        {

            if (m_update)
            {
                foreach(BaseConnection connection in m_currentState.m_connections)
                {
                    if (connection.Condition())
                    {
                        m_currentState = connection.m_state;
                        if (m_previousState != null)
                        {
                            m_previousState.StopState();
                        }
                    }
                }

                if (m_previousState == m_currentState) //same state continues: update
                {
                    m_currentState.UpdateState();
                }
                else
                {
                    m_previousState = m_currentState;
                    m_currentState.StartState();
                }
            }
        }
    }
}
