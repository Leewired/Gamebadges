using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;


namespace MazeGame.Core
{
    public class MazeScript
    {

        public Script m_script = null; //store instance
        private string m_startupScript = "";

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
            m_script.Options.DebugPrint = s => { Debug.Log(s); }; //Delegate prints to debug.log
        }

        public void RunStartup()
        {
            m_script.DoString(m_startupScript); //run startup script
        }

    }

}
