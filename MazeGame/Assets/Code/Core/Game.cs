using UnityEngine;
using UnityEngine.SceneManagement;
using StateMachine.Core;
using MazeGame.Components.Controllers;

namespace MazeGame.Core
{

    public static class Game
    {

        public static MazeGame.Core.Input m_input = null;
        public static StateMachine.Core.StateMachine m_gameStateMachine = null;
        public static GameController m_gameController = null;
        public static LevelController m_levelController = null;

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

        /*public static GameObject GetController()
        {

        }
        */
    }
}