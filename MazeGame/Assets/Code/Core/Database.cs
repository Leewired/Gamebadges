using UnityEngine;
using Mono.Data.Sqlite;

namespace MazeGame.Core
{

    public class BaseDatabase
    {
        private string m_dbName = "";
        private string m_dbFile = "";
        private SqliteConnection m_connection = null;

        public BaseDatabase(string dbName, string dbFile)
        {
            m_dbName = dbName;
            m_dbFile = dbFile;
            m_connection = new SqliteConnection(dbFile);
        }

        public void ExecuteSQL(string sql)
        {
            m_connection.Open();
            using (SqliteCommand cmd = m_connection.CreateCommand())
            {
                cmd.CommandText = sql;
                using (SqliteDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        Debug.Log(rdr["fin"]);
                    }
                }
            }
            m_connection.Close();
        }
    }


    public class DialogueDatabase: BaseDatabase
    {
        public DialogueDatabase(
            string dbName = "DialogueDatabase",
            string dbFile = "URI=file:Assets/Resources/Dialogue.db") : base(dbName, dbFile)
        {
        }

    }
}
