using UnityEngine;
using Mono.Data.Sqlite;

namespace MazeGame.Core
{

    public class BaseDatabase
    {
        public string m_dbName = "";
        private string m_dbFile = "";
        public SqliteConnection m_connection = null;

        public BaseDatabase(string dbName, string dbFile)
        {
            m_dbName = dbName;
            m_dbFile = dbFile;
            m_connection = new SqliteConnection(dbFile);
        }
      
    }


    public class DialogueDatabase: BaseDatabase
    {
        public DialogueDatabase(
            string dbName = "Dialogue",
            string dbFile = "URI=file:Assets/Resources/Dialogue.db") : base(dbName, dbFile)
        {
        }

        public string ReadDialogueLine(int id)
        {
            string line = "";
            m_connection.Open();

            using (SqliteCommand cmd = m_connection.CreateCommand())
            {
                Settings set = Settings.CreateInstance();
                string l = set.m_language;
                cmd.CommandText = string.Format("Select {0} from {1} where id={2}", l, this.m_dbName, id.ToString());
                using(SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        line = reader[l].ToString();
                    }
                }
            }

            m_connection.Close();            
            return line;
        }

    }
}
