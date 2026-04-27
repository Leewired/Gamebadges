using MazeGame.Core;
using StateMachine.Core;
using StateMachine.States;
using UnityEngine;

//add gamecontroller as 1st


namespace MazeGame.Components.Controllers
{
    public class GameController : BaseController
    {

        private StateMachine.Core.StateMachine m_stateMachine = null; //m = member, member of the class
        private MazeGame.Core.Input m_input = null;

        public override void Start()
        {

            base.Start();
            m_input = MazeGame.Core.Input.GetInstance();
            Game.m_input = m_input;
            SetupStateMachine();
            Game.m_gameController = this;

        }

        private void SetupStateMachine()
        {
            m_stateMachine = new StateMachine.Core.StateMachine(); //no singletons, because we want to nest stuff
            Game.m_gameStateMachine = m_stateMachine; //pass state machine to Game
            InitState initState = new InitState();
            LoadingState loadingState = new LoadingState();
            LoadingConnection loadingConnection = new LoadingConnection(m_stateMachine);
            loadingConnection.m_state = loadingState;
            initState.AddOutputConnection(loadingConnection);

            m_stateMachine.m_currentState = initState;
            m_stateMachine.StartStateMachine();
        }

        private void Update() //use fixed update for reals
        {
            m_stateMachine.UpdateStateMachine();
        }

    }

}
