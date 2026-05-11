using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Mono.Data.Sqlite;
using Codice.Client.BaseCommands.BranchExplorer;


namespace MazeGame.Editor
{

    public class CSVToDBConverter
    {
        [MenuItem("GameBadges/Convet CSV to DB")]
        public static void ConvertCSV()
        {
            string csvDialogueFile = "Assets/Resources/Dialogue.csv";
            string dbDialogueFile = "URI=file:Assets/Resources/Dialogue.db";
            string dbName = "Dialogue";

            string[] lines = File.ReadAllLines(csvDialogueFile);
            Debug.Log("Reading lines!");
            SqliteConnection con = new SqliteConnection(dbDialogueFile);
            con.Open();

            string[] headers = lines[0].Split(";");
            string[] types = lines[1].Split(";");
            string[] sqlTypes = new string[types.Length]; //figure out types from types
            string sql = string.Format("CREATE TABLE IF NOT EXISTS {0} (", dbName);

            for (int i = 0; i < types.Length; i++)
            {
                sqlTypes[i] = GetSQLType(types[i]);
                sql = string.Format("{0} {1} {2}", sql, headers[i], sqlTypes[i]);
                if (i < types.Length - 1)
                {
                    sql = string.Format("{0},", sql);
                }

            }

            sql = string.Format("{0});", sql);
            using (SqliteCommand cmd = con.CreateCommand()) //write to file
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }

            string[] values = new string[lines.Length - 1];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = lines[i + 1];
            }

            for (int j = 0; j < values.Length; j++) //TODO: check formatting
            {
                sql = string.Format("INSERT INTO {0} (", dbName);
                for (int i = 0; i < headers.Length; i++)
                {
                    sql = string.Format("{0} {1}", sql, headers[i]);
                    if (i < types.Length -1)
                    {

                        sql = string.Format("{0},", sql);
                    }

                }
                sql = string.Format("{0}) VALUES (", sql);
                string[] v = values[j].Split(";");
                for (int i = 0; i < v.Length; i++)
                {
                    if (types[i] == "int")
                    {
                        sql = string.Format("{0} \"{1}\"", sql, Convert.ToInt32(v[i]));
                    }
                    else
                    {
                        sql = string.Format("{0} \"{1}\"", sql, Convert.ToString(v[i]));
                    }
                    if (i < types.Length - 1)
                    {
                        sql = string.Format("{0},", sql);
                    }
                }
                sql = string.Format("{0});", sql);
                using (SqliteCommand cmd = con.CreateCommand()) //write to file
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }

            Debug.Log(sql);

            con.Close();
        }

        private static string GetSQLType(string sv)
        {
            try
            {
                int v = Convert.ToInt32(sv);
                return "int";
            }
            catch
            {
                return "varchar(255)"; //TODO: Check lesson 10
            }
        }
    }
}
