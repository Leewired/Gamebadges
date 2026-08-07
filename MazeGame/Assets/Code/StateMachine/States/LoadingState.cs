using UnityEngine;
using StateMachine.Core;
using UnityEngine.SceneManagement;
using MazeGame.Core;
using MazeGame.Components.Controllers;
using MazeGame.Components;
using MazeGame.Maze;

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
            Game.m_dialogueDatabase = new DialogueDatabase();
            SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Level", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("End", LoadSceneMode.Additive);
        }

        public override void UpdateState()
        {
            Scene s1 = SceneManager.GetSceneByName("Intro");
            Scene s2 = SceneManager.GetSceneByName("Level");
            Scene s3 = SceneManager.GetSceneByName("Pause");
            Scene s4 = SceneManager.GetSceneByName("End"); //TODO: move end loading to later stage

            //TODO: Add camera to intro

            if (s1.isLoaded && s2.isLoaded && s3.isLoaded && s4.isLoaded)
            {
                Game.m_introController = (IntroController)Game.GetController(s1);
                Game.m_levelController = (LevelController)Game.GetController(s2);
                Game.m_pauseController = (PauseController)Game.GetController(s3);
                Game.m_endController = (EndController)Game.GetController(s4);

                if (Game.m_levelController != null)
                {
                    PlayerComponent pc = GameObject.FindAnyObjectByType<PlayerComponent>();
                    Game.m_player = new Player(pc);

                    EnemyComponent ec  = GameObject.FindAnyObjectByType<EnemyComponent>();
                    Game.m_enemy = new MazeGame.Core.Enemy(ec);

                    Game.m_introController.Deactivate();
                    Game.m_levelController.Deactivate();
                    Game.m_pauseController.Deactivate();
                    Game.m_endController.Deactivate();

                    SceneManager.SetActiveScene(s2);

                    //partial recreation of the maze due to not having acces to it anymore
                    Maze m = Game.m_levelController.m_visualRoot.GetComponentInChildren<Maze>();
                    if (m != null)
                    {
                        Game.m_maze = m;
                        CellComponent[] comps = Game.m_levelController.m_visualRoot.GetComponentsInChildren<CellComponent>();
                        Cell[] cells = new Cell[comps.Length];
                        for (int i=0; i<comps.Length; i++)
                        {
                            if (comps[i].m_floor)
                            {
                                cells[i] = new RoomCell();                                
                            }
                            else
                            {
                                cells[i] = new WallCell();
                            }
                            cells[i].m_instancedGameObject = comps[i].gameObject;
                        }
                        MeshRenderer mr = comps[0].gameObject.GetComponentInChildren<MeshRenderer>(); //setting on single wall affects all
                        mr.sharedMaterial.SetColor("_Color", Settings.CreateInstance().m_color); //load color from settings
                        Game.m_maze.m_maze = cells;
                        Game.m_maze.m_indicesCount = cells.Length;

                    }

                    m_stateMachine.AddParameter("Intro", true);

                }
            }
        }

    }

}
