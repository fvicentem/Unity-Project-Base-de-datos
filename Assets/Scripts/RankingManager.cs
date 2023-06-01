using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Librerías que necesitamos
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

public class RankingManager : MonoBehaviour
{
    //Variable para controlar la ruta de la base de datos, constructor de la ruta, y el nombre de la base de datos
    string rutaDB;
    string strConexion;
    string DBFileName = "RankingFernando.db";

    //Variable para trabajar con las conexiones
    IDbConnection dbConnection;
    //Para poder ejecutar comandos
    IDbCommand dbCommand;
    //Variable para leer
    IDataReader reader;

    //Lista para el Ranking
    private List<Ranking> rankings = new List<Ranking>();

    //Variables para almacenar el prefab y la posición del padre
    public GameObject puntosPrefab;
    public Transform puntosPadre;

    //Límite de datos que aparecerá en la UI
    public int topRank;
    //Limitamos el ranking que podemos tener
    public int limiteRanking;

    // Start is called before the first frame update
    void Start()
    {
        //InsertarPuntos("María", 25);
        //BorrarPuntos(3);
        //ObtenerRanking();
        BorrarPuntosExtra();
        MostrarRanking();
    }

    //Método para abrir la DB
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

    //Método para obtener el Ranking de la DB
    void ObtenerRanking()
    {
        //Primero dejamos la lista de Rankings limpia
        rankings.Clear();
        //Abrimos la DB
        OpenDB(DBFileName);
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "SELECT * FROM ranking";
        dbCommand.CommandText = sqlQuery;

        // Leer la base de datos
        reader = dbCommand.ExecuteReader();
        while (reader.Read())
        {
            rankings.Add(new Ranking(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDateTime(3)));
        }
        reader.Close();
        reader = null;
        //Cerramos la DB
        CerrarDB();
        //Ordenamos la lista
        rankings.Sort();
    }

    //Método para insertar puntos en la DB
    public void InsertarPuntos(string n, int s)
    {
        //Abrimos la DB
        OpenDB(DBFileName);
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = String.Format("INSERT INTO ranking(Name, Score) values(\"{0}\",\"{1}\")", n, s);
        dbCommand.CommandText = sqlQuery;
        dbCommand.ExecuteScalar();
        //Cerramos la DB
        CerrarDB();
    }

    //Método para borrar puntos de la DB
    void BorrarPuntos(int id)
    {
        //Abrimos la DB
        OpenDB(DBFileName);
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "DELETE FROM Ranking WHERE PlayerId = \"" + id + "\"";
        dbCommand.CommandText = sqlQuery;
        dbCommand.ExecuteScalar();
        //Cerramos la DB
        CerrarDB();
    }

    //Método para mostrar el ranking en la UI
    void MostrarRanking()
    {
        //Obtener el ranking de la DB
        ObtenerRanking();
        //Hacemos una pasada por la lista para ir posicionando los puntos en la UI
        for (int i = 0; i < topRank; i++)
        {
            //Si siguen habiendo elementos en la lista
            if (i < rankings.Count)
            {
                //Instanciamos el objeto Puntaje
                GameObject tempPrefab = Instantiate(puntosPrefab);
                //Hacemos este objeto hijo de Puntos
                tempPrefab.transform.SetParent(puntosPadre);
                //Le ponemos la escala para que se adapte a la resolución actual de la UI
                tempPrefab.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                //Posición en la lista
                Ranking rankTemp = rankings[i];
                //Llamamos al método que pone los puntos
                tempPrefab.GetComponent<RankingScript>().PonerPuntos("#" + (i + 1).ToString(),
                                                        rankTemp.Name, rankTemp.ScoreTime.ToString());
            }
        }
    }

    //Método para borrar los puntos extra
    void BorrarPuntosExtra()
    {
        //Obtener el Ranking
        ObtenerRanking();
        //Comprobar que el ranking sea mas grande que el limite
        if (limiteRanking <= rankings.Count)
        {
            //Le damos la vuelta a la lista para borrar las menores puntuaciones
            rankings.Reverse();
            //obtenemos la diferencia entre el ranking y el limite, para ver cuantos registros nos sobran
            int diferencia = rankings.Count - limiteRanking;
            //Abrimos DB
            OpenDB(DBFileName);
            //Creo Comando
            dbCommand = dbConnection.CreateCommand();
            //Bucle con la diferencia
            for (int i = 0; i < diferencia; i++)
            {
                //Borrar por ID en la posicion del ranking
                string sqlQuery = "DELETE FROM Ranking WHERE PlayerId = \"" + rankings[i].Id + "\"";
                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
            }
            //Cerrar DB
            CerrarDB();

        }
    }

    //Método para cerrar la DB
    void CerrarDB()
    {
        // Cerrar las conexiones
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }
}
