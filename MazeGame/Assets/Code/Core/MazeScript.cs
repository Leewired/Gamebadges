using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;
using System;


namespace MazeGame.Core
{
    public class MazeScript
    {

        public Script m_script = null; //store instance
        private string m_startupScript = "";
        private string m_levelScript = "";
        private string m_vector3Script = "";

        public MazeScript()
        {
            Debug.Log(string.Format("MazeScript {0} created", this.GetType().Name));
            m_script = new Script();
        }

        public void LoadScript()
        {
            Debug.Log("Loading Scripts.");
            string startupFile = Application.dataPath + "/Resources/Lua/startup.lua";
            m_startupScript = File.ReadAllText(startupFile);

            string levelFile = Application.dataPath + "/Resources/Lua/level.lua";
            m_levelScript = File.ReadAllText(levelFile);

            string vector3File = Application.dataPath + "/Resources/Lua/vector3.lua";
            m_vector3Script = File.ReadAllText(vector3File);

            m_script.Options.DebugPrint = s => { Debug.Log(s); }; //Delegate prints to debug.log
            m_script.Globals["GetDialogueLine"] =
                (Func<int, string>)GetDialogueLine;
            m_script.Globals["SetIntroText"] =
                (Action<string>)SetIntroText;
            m_script.Globals["SetPauseText"] =
                (Action<string>)SetPauseText;
            m_script.Globals["SetEndText"] =
                (Action<string>)SetEndText;
            //m_script.Globals["Vector3.PrintVector3Length"] = //This does not find the method
                //(Action<string>)PrintVector3Length;
        }

        

        public void RunStartup()
        {
            m_script.DoString(m_startupScript); //run startup script
        }

        public void RunLevel()
        {
            m_script.DoString(m_levelScript);
            DynValue v = m_script.Globals.Get("OnDialogue");
            DynValue c = m_script.Call(v, DynValue.NewNumber(1)); //call OnDialogue function with id 1
            /*
            m_script.DoString(m_vector3Script);
            Table v3class = m_script.Globals.Get("Vector3").Table;
            DynValue vector3instance = m_script.Call(
                v3class.Get("new"),
                DynValue.Nil,
                DynValue.NewNumber(3),
                DynValue.NewNumber(5),
                DynValue.NewNumber(10)
                );
            m_script.Call(v3class.Get("PrintVector3Length"), vector3instance);
            */
        }

        private static string GetDialogueLine(int id)
        {
            string s = Game.m_dialogueDatabase.ReadDialogueLine(id);
            return s;
        }

        private static void SetIntroText(string text)
        {
            Game.m_introController.SetIntroText(text);
        }

        private static void SetPauseText(string text)
        {
            Game.m_pauseController.SetPauseText(text);
        }

        private static void SetEndText(string text)
        {
            Game.m_endController.SetEndText(text);
        }


        /*private static void PrintVector3Length(string text)
        {
            Debug.Log(text);
        }
        */
    }

}
