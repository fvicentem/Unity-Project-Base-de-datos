using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;//Librería añadida para trabajar con la DB
using System.IO;//Librería para poder abrir archivos
using Mono.Data.Sqlite;//Librería para trabajar con SQLite

public class DatabaseController : MonoBehaviour
{
    //Variable donde guardar la dirección de la Base de Datos
    string rutaDB;
    string strConexion;
    //Nombre de la base de datos con la que vamos a trabajar
    string DBFileName = "ZapatosJuanAlbertoDB.db";

    //Referencia que necesitamos para poder crear una conexión 
    IDbConnection dbConnection;
    //Referencia que necesitamos para poder ejecutar comandos
    IDbCommand dbCommand;
    //Referencia que necesitamos para leer datos
    IDataReader reader;

    void OpenDB(string DatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
            // check if file exists in Application.persistentDataPath
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);
       
            if (!File.Exists(filepath))
            {
                Debug.Log("Database not in Persistent path");
                // if it doesn't ->
                // open StreamingAssets directory and load the db ->
           
#if UNITY_ANDROID
                var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
                while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                // then save to Application.persistentDataPath
                File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
           
#elif UNITY_WINRT
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
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
