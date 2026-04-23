using StateMachine.States;
using StateMachine.Core;
using MazeGame.Core;
using UnityEngine;


namespace StateMachine
{
    public class StateMachineComponent : MonoBehaviour
    {
        private StateMachine.Core.StateMachine m_stateMachine = null; //m = member, member of the class

        private void Start()
        {
            SetupStateMachine();
        }

        private void Update() //use fixed update for reals
        {
            m_stateMachine.UpdateStateMachine();
        }

        private void SetupStateMachine()
        {
            m_stateMachine = new Core.StateMachine(); //no singletons, because we want to nest stuff
            Game.m_gameStateMachine = m_stateMachine; //pass state machine to Game
            InitState initState = new InitState();
            LoadingState loadingState = new LoadingState();
            LoadingConnection loadingConnection = new LoadingConnection(m_stateMachine);
            loadingConnection.m_state = loadingState;
            initState.AddOutputConnection(loadingConnection);
            

            m_stateMachine.m_currentState = initState;            
            m_stateMachine.StartStateMachine();
        }

    }
}

