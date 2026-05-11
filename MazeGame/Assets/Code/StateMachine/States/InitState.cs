using UnityEngine;
using StateMachine.Core;
using MazeGame.Core;
using System.IO;

namespace StateMachine.States
{

    public class InitState : BaseState
    {

        public InitState(Core.StateMachine fsm) : base(fsm)
        {

        }

        public override void StartState()
        {
            base.StartState();
            Game.m_gameData = new GameData();
            Game.m_gameStateMachine.AddParameter("Load", true); //load parameter true makes state move on after init

            if (File.Exists(Application.persistentDataPath + "/settings.xml"))
            {
                Settings set = Settings.Read();
                Debug.Log(set.m_language);
                Debug.Log(set.m_color);
            }
            else
            {
                Settings set = Settings.CreateInstance();
                set.Write();
            }

            
            
        }


    }

}
