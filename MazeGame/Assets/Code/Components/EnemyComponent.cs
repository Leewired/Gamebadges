using UnityEngine;
using UnityEngine.AI;


namespace MazeGame.Components
{

    public class EnemyComponent : MonoBehaviour
    {
        [HideInInspector] //Hooray a decorator!
        public NavMeshAgent m_agent = null;

        private void Start()
        {
            m_agent = GetComponent<NavMeshAgent>();
        }

    }

}
