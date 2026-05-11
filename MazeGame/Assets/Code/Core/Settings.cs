using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


namespace MazeGame.Core
{

    [Serializable]
    public class Settings
    {
        [NonSerialized] private static Settings instance = null;
        public string m_language = "eng";
        public Color m_color = Color.orange;

        private Settings() //Singleton class
        {
        }

        public void Write()
        {
            //create a schema out of the class and then serialize this instance of the class
            XmlSerializer serializer = new XmlSerializer(typeof(Settings)); //serializing stuff
            using (TextWriter writer = new StreamWriter(Application.persistentDataPath + "/settings.xml")) //for Unity: User/AppData/LocalLow
            {
                serializer.Serialize(writer, this); //write this instance
            }
            Debug.Log(String.Format("Wrote settings to {0} + /settings.xml.", Application.persistentDataPath));
        }

        public static Settings Read()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (TextReader reader = new StreamReader(Application.persistentDataPath + "/settings.xml"))
                {
                    instance = (Settings)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                instance = new Settings();
            }
            return instance;
        }

        public static Settings CreateInstance()
        {
            if (instance == null)
            {
                instance = new Settings();
                Debug.Log("Creating settings instance.");
            }    
            return instance;
        }
    }

}
