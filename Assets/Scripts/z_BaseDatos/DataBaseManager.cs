using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class DataBaseManager {

    static string strConexion;
    static string DBFileName = "RankingFernando.db";

    static IDbConnection dbConnection;
    static IDbCommand dbCommand;
    static IDataReader reader;

    static string tableName1 = "Tabla_Monedas";
    static string tableName2 = "Table_2";
    static string columnName1 = "Monedas";
    static string columnName2 = "column_2";


    public static int LoadMonedas() {
        // Create the select statement.
        string sql = "SELECT " + columnName1 + " FROM " + tableName1;

        return LoadData(sql);
    }

/*    public static int LoadValue2() {
        // Create the select statement.
        string sql = "SELECT " + columnName2 + " FROM " + tableName2;

        return LoadData(sql);
    }*/

    private static int LoadData(string sql) {
        // Open the database.
        OpenDB(DBFileName);

        // Prepare the statement.
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = sql;

        // Execute the statement.
        dbCommand.ExecuteNonQuery();

        // Get the score from the database.
        object tableObject = dbCommand.ExecuteScalar();

        // Close the database.
        CerrarDB();

        // Check if the value is null.
        if (tableObject == null) {
            return 0;
        }

        // Convert the value to an int.
        //Debug.Log($"##scoreObject: {scoreObject}");
        int value = Convert.ToInt32(tableObject);

        return value;
    }


    public static void SaveMonedas(int monedas) {
        // Create the update statement.
        string sql = "UPDATE " + tableName1 + " SET " + columnName1 + " = @value";

        SaveData(monedas, sql);
    }

/*    public static void SaveValue2(int distance) {
        // Create the update statement.
        string sql = "UPDATE " + tableName2 + " SET " + columnName2 + " = @value";

        SaveData(distance, sql);
    }*/

    private static void SaveData(int value, string sql) {
        // Open the database.
        OpenDB(DBFileName);

        // Prepare the statement.
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = sql;

        // Create a SqliteParameter object.
        SqliteParameter parameter = new SqliteParameter("@value", SqlDbType.Int);
        parameter.Value = value;

        // Add the parameter to the command.
        dbCommand.Parameters.Add(parameter);

        // Execute the statement.
        dbCommand.ExecuteNonQuery();

        CerrarDB();
    }

#if UNITY_EDITOR
    [MenuItem("Tools/DataBases/Reset Data")]
    public static void ResetDatabase() {
        SaveMonedas(0);
        //SaveValue2(0);
        PlayerPrefs.DeleteAll();
    }
#endif

    //Método para cerrar la DB
    static void CerrarDB() {
        // Cerrar las conexiones
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    static void OpenDB(string DBFileName) {

#if UNITY_EDITOR
        string dbPath = string.Format(@"Assets/StreamingAssets/{0}", DBFileName);
#else
            // check if file exists in Application.persistentDataPath
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DBFileName);
       
            if (!File.Exists(filepath))
            {
                Debug.Log("Database not in Persistent path");
                // if it doesn't ->
                // open StreamingAssets directory and load the db ->
           
#if UNITY_ANDROID
                var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DBFileName);  // this is the path to your StreamingAssets in android
                while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                // then save to Application.persistentDataPath
                File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                var loadDb = Application.dataPath + "/Raw/" + DBFileName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DBFileName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
           
#elif UNITY_WINRT
                var loadDb = Application.dataPath + "/StreamingAssets/" + DBFileName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#endif
           
                Debug.Log("Database written");
            }
       
            var dbPath = filepath;
#endif
        //_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

        //open db connection
        strConexion = "URI=file:" + dbPath;
        Debug.Log("Stablishing connection to: " + strConexion);
        dbConnection = new SqliteConnection(strConexion);
        dbConnection.Open();
    }
}
