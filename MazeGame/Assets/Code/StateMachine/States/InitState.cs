using UnityEngine;
using StateMachine.Core;
using MazeGame.Core;

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
            Game.m_gameStateMachine.AddParameter("Load", true); //load parameter true makes state move on after init
        }


    }

}
