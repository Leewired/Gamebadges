using System;
using UnityEngine;


namespace StateMachine.Core
{
    public class BaseParameter
    {

        public string m_name = "";
        public Guid m_guid = Guid.Empty;

        public BaseParameter(string name)
        {
            m_name = name;
            m_guid = Guid.NewGuid();
            Debug.Log(string.Format("Created {0} with a guid: {1}", this.GetType(), m_guid.ToString()));
        }

    }

    public class ParameterString : BaseParameter
    {
        public string m_value;
        public string m_name;

        public ParameterString(string name, string value) : base(name)
        {
            m_name = name;
            m_value = value;
        }
    }

    public class ParameterBool : BaseParameter
    {
        public bool m_value;
        public string m_name;

        public ParameterBool(string name, bool value) : base(name)
        {
            m_name = name;
            m_value = value;
        }
    }
}

