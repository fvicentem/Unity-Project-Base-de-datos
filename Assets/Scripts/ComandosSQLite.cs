using System.Collections;
using System.Collections.Generic;
//Declaramos las librerías necesarias
using UnityEngine;
using System.Data;//Librería añadida para trabajar con la DB
using System.IO;//Librería para poder abrir archivos
using Mono.Data.Sqlite;//Librería para trabajar con SQLite
using UnityEngine.UI; //Para usar el Canvas de Unity
using System; //Para por ejemplo mandar mensajes del sistema (error/aviso...)

public class ComandosSQLite : MonoBehaviour
{
    //Variable donde guardar la dirección de la Base de Datos
    string rutaDB;
    string strConexion;
    //Nombre de la base de datos con la que vamos a trabajar
    string DBFileName = "chinook.db";
    //Variable texto UI
    //public Text myText;

    //Referencia que necesitamos para poder crear una conexión 
    IDbConnection dbConnection;
    //Referencia que necesitamos para poder ejecutar comandos
    IDbCommand dbCommand;
    //Referencia que necesitamos para leer datos
    IDataReader reader;

    // Start is called before the first frame update
    void Start()
    {
        //Llamamos al método que abre las conexiones
        AbrirDB();
        ComandoSelect("*", "albums");
        //ComandoWhere("*", "albums", "AlbumId", "=", "33");
        //ComandoWhere_And("CustomerId", "FirstName", "LastName", "Country", "customers", "Country", "=", "Brazil", "CustomerId", ">", "10");
        //ComandoWHERE_AND_ORDER_BY("CustomerId", "FirstName", "LastName", "Country", "customers", "Country", "=", "Brazil", "CustomerId", ">", "10", "FirstName", "ASC");
        //ComandoIN("CustomerId", "FirstName", "LastName", "Country", "customers", "Country", "Brazil", "USA");
        //FuncCount();
        //FuncAVG();
        //INSERT("image");
        //UPDATE();
        //DELETE();
        CerrarDB();
    }

    void AbrirDB()
    {
        // Crear y abrir la conexión
        //Compuebo en que plataforma estamos
        //Si estamos en PC mantenemos la ruta
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            rutaDB = Application.dataPath + "/StreamingAssets/" + DBFileName;
        }
        //Si estamos en Android
        else if (Application.platform == RuntimePlatform.Android)
        {
            rutaDB = Application.persistentDataPath + "/" + DBFileName;
            //Comprobar si el archivo se encuentra almacenado en persistant data
            if (!File.Exists(rutaDB))
            {
                //Almaceno el archivo en load db
                //Copio el archivo a persistant data
                WWW loadDB = new WWW("jar;file://" + Application.dataPath + DBFileName);
                while (!loadDB.isDone)
                {

                }
                File.WriteAllBytes(rutaDB, loadDB.bytes);
            }
        }

        strConexion = "URI=file:" + rutaDB;
        dbConnection = new SqliteConnection(strConexion);
        dbConnection.Open();
    }

    //Cerrar la conexión
    void CerrarDB()
    {
        // Cerrar las conexiones
        reader.Close();
        reader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    //Comando Select
    void ComandoSelect(string item, string tabla)
    {
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "SELECT " + item + " FROM " + tabla;
        dbCommand.CommandText = sqlQuery;

        // Leer la base de datos
        reader = dbCommand.ExecuteReader();
        while (reader.Read())
        {
            try
            {
                Debug.Log(reader.GetInt32(0) + " - " + reader.GetString(1) + " - " + reader.GetInt32(2));
            }
            catch (FormatException fe)
            {
                Debug.Log(fe.Message);
                continue;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                continue;
            }
        }
    }

    //Comando WHERE
    void ComandoWhere(string item, string tabla, string campo, string comparador, string dato)
    {
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "select " + item + " from " + tabla + " where " + campo + " " + comparador + " " + dato;
        dbCommand.CommandText = sqlQuery;

        // Leer la base de datos
        reader = dbCommand.ExecuteReader();
        while (reader.Read())
        {
            try
            {
                Debug.Log(reader.GetInt32(0) + " - " + reader.GetString(1) + " - " + reader.GetInt32(2));
            }
            catch (FormatException fe)
            {
                Debug.Log(fe.Message);
                continue;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                continue;
            }
        }
    }

    //Comando WHERE_AND
    void ComandoWhere_And(string item, string item2, string item3, string item4,
    string tabla,
    string campo, string comparador, string dato,
    string campo2, string comparador2, string dato2)
    {
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "select " + item + "," + item2 + "," + item3 + "," + item4 +
                          " from " + tabla +
                          " where " + campo + " " + comparador + " " + "'" + dato + "'" +
                          "and" + " " + campo2 + " " + comparador2 + dato2;
        dbCommand.CommandText = sqlQuery;

        // Leer la base de datos
        reader = dbCommand.ExecuteReader();
        while (reader.Read())
        {
            try
            {
                Debug.Log(reader.GetInt32(0) + " - " + reader.GetString(1) + " - " + reader.GetString(2) + "-" + reader.GetString(3));
            }
            catch (FormatException fe)
            {
                Debug.Log(fe.Message);
                continue;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                continue;
            }
        }
    }

    //Comando WHERE_AND_ORDER_BY
    void ComandoWHERE_AND_ORDER_BY(string item, string item2, string item3, string item4,
    string tabla,
    string campo, string comparador, string dato,
    string campo2, string comparador2, string dato2,
    string campo3, string orden)
    {
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "select " + item + "," + item2 + "," + item3 + "," + item4 +
                          " from " + tabla +
                          " where " + campo + " " + comparador + " " + "'" + dato + "'" +
                          "and" + " " + campo2 + " " + comparador2 + dato2 +
                           " order by " + campo3 + " " + orden;
        dbCommand.CommandText = sqlQuery;

        // Leer la base de datos
        reader = dbCommand.ExecuteReader();
        while (reader.Read())
        {
            try
            {
                Debug.Log(reader.GetInt32(0) + " - " + reader.GetString(1) + " - " + reader.GetString(2) + "-" + reader.GetString(3));
            }
            catch (FormatException fe)
            {
                Debug.Log(fe.Message);
                continue;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                continue;
            }
        }
    }

    //Comando IN_OR
    void ComandoIN(string item, string item2, string item3, string item4,
    string tabla,
    string campo, string dato, string dato2)
    {
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "select " + item + "," + item2 + "," + item3 + "," + item4 +
                          " from " + tabla +
                          " where " + campo + " " + "IN" + " " + "('" + dato + "'" + ",'" + dato2 + "')";
        dbCommand.CommandText = sqlQuery;

        // Leer la base de datos
        reader = dbCommand.ExecuteReader();
        while (reader.Read())
        {
            try
            {
                Debug.Log(reader.GetInt32(0) + " - " + reader.GetString(1) + " - " + reader.GetString(2) + "-" + reader.GetString(3));
            }
            catch (FormatException fe)
            {
                Debug.Log(fe.Message);
                continue;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                continue;
            }
        }
    }

    //Función Count
    void FuncCount()
    {
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "SELECT COUNT(*) FROM customers WHERE Country = \"Canada\""; //Contrabarra es igual a poner "'"
        dbCommand.CommandText = sqlQuery;
        //Creamos la variable para el conteo
        int filaCont = 0;
        //Para poder contar datos de las filas tenemos que poner
        filaCont = Convert.ToInt32(dbCommand.ExecuteScalar());
        //ExecuteScalar me devuelve un objeto no un número, por lo tanto lo debo convertir a INT
        Debug.Log(filaCont);
        //Cerramos la DB
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    //Función Media
    void FuncAVG()
    {
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "SELECT AVG(total) FROM invoices";
        dbCommand.CommandText = sqlQuery;
        //Creamos la variable para la media
        Double filaAVG = 0;
        //Para poder contar datos de las filas tenemos que poner
        filaAVG = Convert.ToDouble(dbCommand.ExecuteScalar());
        //ExecuteScalar me devuelve un objeto no un número, por lo tanto lo debo convertir a INT
        Debug.Log(filaAVG);
        //Cerramos la DB
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    //INSERT INTO media_types(Name) values("image")
    void INSERT(string dato)
    {
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = String.Format("INSERT INTO media_types(Name) values(\"{0}\")", dato); //String.Format podemos pasarle datos a una cadena de caracteres para que tome el argumento
        dbCommand.CommandText = sqlQuery;
        //Para poder contar datos de las filas tenemos que poner
        dbCommand.ExecuteScalar();
        //ExecuteScalar me devuelve un objeto 
        //Cerramos la DB
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    //UPDATE media_types SET Name = "Luis" WHERE MediaTypeId = 7;
    void UPDATE()
    {
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "UPDATE media_types SET Name = \"Luis\" WHERE MediaTypeId = 7";
        dbCommand.CommandText = sqlQuery;
        //Para poder contar datos de las filas tenemos que poner
        dbCommand.ExecuteScalar();
        //ExecuteScalar me devuelve un objeto 
        //Cerramos la DB
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    //DELETE FROM media_types WHERE MediaTypeId = 7;
    void DELETE()
    {
        // Crear la consulta
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "DELETE FROM media_types WHERE MediaTypeId = 7";
        dbCommand.CommandText = sqlQuery;
        //Para poder contar datos de las filas tenemos que poner
        dbCommand.ExecuteScalar();
        //ExecuteScalar me devuelve un objeto 
        //Cerramos la DB
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

}
