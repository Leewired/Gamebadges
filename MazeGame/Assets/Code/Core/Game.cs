using UnityEngine;
using UnityEngine.SceneManagement;
using StateMachine.Core;
using MazeGame.Components.Controllers;
using MazeGame.Maze;

namespace MazeGame.Core
{

    public static class Game
    {

        public static Input m_input = null;
        public static StateMachine.Core.StateMachine m_gameStateMachine = null;
        public static GameController m_gameController = null;
        public static LevelController m_levelController = null;
        public static Player m_player = null;
        public static Enemy m_enemy = null;
        public static Maze.Maze m_maze = null;


        public static BaseController GetController(Scene s)
        {
            foreach(GameObject go in s.GetRootGameObjects())
            {
                if (go.name.ToLower() == "controller")
                {
                    BaseController c = go.GetComponent<BaseController>();
                    if (c != null)
                    {
                        return c;
                    }
                    break;
                }
            }
            return null;
        }

        public static GameObject GetGameObject(Scene s, string name)
        {
            foreach (GameObject go in s.GetRootGameObjects())
            {
                if (go.name.ToLower() == name.ToLower())
                {
                    return go;
                }
            }
            return null;
        }

       
    }
}