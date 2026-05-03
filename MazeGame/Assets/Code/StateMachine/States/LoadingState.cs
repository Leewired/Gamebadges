using UnityEngine;
using StateMachine.Core;
using UnityEngine.SceneManagement;
using MazeGame.Core;
using MazeGame.Components.Controllers;

namespace StateMachine.States
{

    public class LoadingState : BaseState
    {

        public LoadingState(Core.StateMachine fsm): base(fsm)
        {

        }

        public override void StartState()
        {
            base.StartState();
            SceneManager.LoadSceneAsync("Level", LoadSceneMode.Additive);
        }

        public override void UpdateState()
        {
            Scene s = SceneManager.GetSceneByName("Level");
            if (s.isLoaded)
            {
                Game.m_levelController = (LevelController)Game.GetController(s);
                if (Game.m_levelController != null)
                {
                    PlayerComponent pc = GameObject.FindAnyObjectByType<PlayerComponent>();
                    Game.m_player = new Player(pc);
                    Game.m_levelController.Deactivate();
                    m_stateMachine.AddParameter("Gameplay", true);
                    
                }
            }
        }

    }

}
