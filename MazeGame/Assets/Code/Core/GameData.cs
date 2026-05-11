using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace MazeGame.Core
{
    public class GameData
    {
        public Scene m_scene;
        public Vector3 m_playerPosition = Vector3.zero;
        public Quaternion m_playerRotation = Quaternion.identity;
        public bool m_playerHaveKey = false;
        public Vector3 m_enemyPosition = Vector3.zero;
        public Vector3 m_enemyDestination = Vector3.zero;

        public void WriteData()
        {
            string filePath = Application.persistentDataPath + "/quicksave.sav";
            FileStream fileStream = new FileStream(
                filePath,
                FileMode.OpenOrCreate,
                FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fileStream, Encoding.ASCII);
            bw.Write(m_scene.name);
            bw.Write(m_playerPosition.x);
            bw.Write(m_playerPosition.y);
            bw.Write(m_playerPosition.z);
            bw.Write(m_playerRotation.x);
            bw.Write(m_playerRotation.y);
            bw.Write(m_playerRotation.z);
            bw.Write(m_playerRotation.w);
            bw.Write(m_playerHaveKey);
            bw.Write(m_enemyPosition.x);
            bw.Write(m_enemyPosition.y);
            bw.Write(m_enemyPosition.z);
            bw.Write(m_enemyDestination.x);
            bw.Write(m_enemyDestination.y);
            bw.Write(m_enemyDestination.z);

            bw.Flush(); //flush buffer and write to disk            
            bw.Close();

            fileStream.Close();
        }

        public void LoadData()
        {
            string filePath = Application.persistentDataPath + "/quicksave.sav";
            FileStream fileStream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read);
            BinaryReader br = new BinaryReader(fileStream, Encoding.ASCII);
            string sceneName = br.ReadString();
            float pposx = br.ReadSingle();
            float pposy = br.ReadSingle();
            float pposz = br.ReadSingle();
            m_playerPosition = new Vector3(pposx, pposy, pposz);
            float protx = br.ReadSingle();
            float proty = br.ReadSingle();
            float protz = br.ReadSingle();
            float protw = br.ReadSingle();
            m_playerRotation = new Quaternion(protx, proty, protz, protw);
            float eposx = br.ReadSingle();
            float eposy = br.ReadSingle();
            float eposz = br.ReadSingle();
            m_enemyPosition = new Vector3(eposx, eposy, eposz);
            float edesx = br.ReadSingle();
            float edesy = br.ReadSingle();
            float edesz = br.ReadSingle();
            m_enemyDestination = new Vector3(edesx, edesy, edesz);

            br.Close();
            fileStream.Close();
            Debug.Log("Loaded Data."); //add breakpoint to view values here
        }

    }
}

