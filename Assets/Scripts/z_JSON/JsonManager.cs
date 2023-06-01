using System.Collections;
using System.Collections.Generic;
using System.IO; //Usar StreamWriter y StreamReader
using Newtonsoft.Json.Linq; //Para poder usar Json.net y estructuras de datos
using System.Security.Cryptography; //Liber�a para encriptaci�n y desencriptaci�n de informaci�n
using UnityEditor;
using UnityEngine;

public static class JsonManager {

#if UNITY_EDITOR
    [MenuItem("Tools/JSON/Delete save")]
    public static void DeleteSave() {
        string saveFilePath = Application.persistentDataPath + "/jsonSavegame.sav";
        if (File.Exists(saveFilePath)) {
            File.Delete(saveFilePath);
        }
    }
#endif

    public static void SaveGame(Timer1 curtimer) {
        //Generamos un jObject para guardar la informaci�n serializada m�s abajo
        JObject jSaveGame = new JObject();

        
        /// Timer

        //Generamos un jObject pasandole el enemigo concreto serializado
        JObject serializedPlayer = curtimer.Serialize();
        //En el objecto jSon archivo de guardado, a�adimos la informaci�n que queremos de los objetos serializados
        jSaveGame.Add(curtimer.myName, serializedPlayer);

        //Ruta donde queremos guardar la informaci�n
        string saveFilePath = Application.persistentDataPath + "/jsonSavegame.sav";
        

        //Creamos un array de bytes para guardar el array que nos devuelve el m�todo Encrypt para que pueda ser usado
        byte[] encryptSavegame = Encrypt(jSaveGame.ToString());
        //Escribimos esta informaci�n en el archivo de guardado, ya encriptada la informaci�n en su ruta 
        File.WriteAllBytes(saveFilePath, encryptSavegame);
        //Muestra la ruta del archivo por consola
        Debug.Log("Saving to: " + saveFilePath);
    }

    public static void LoadGame(Timer1 curtimer) {
        //Ruta de donde queremos leer la informaci�n
        string saveFilePath = Application.persistentDataPath + "/jsonSavegame.sav";
        //Muestra la ruta del archivo por consola
        Debug.Log("Loading from: " + saveFilePath);

        if (File.Exists(saveFilePath)) {
            //Creamos un array con la informaci�n encriptada recibida
            byte[] decryptedSavegame = File.ReadAllBytes(saveFilePath);
            //Creamos un array donde guardar la informaci�n desencriptada recibida
            string jsonString = Decrypt(decryptedSavegame);

            //Generamos un jObject al que le pasamos la informaci�n del jSon
            JObject jSaveGame = JObject.Parse(jsonString);

            

            /// Timer

            //Generamos un string para cargar la informaci�n sacada del archivo de guardado para esa instancia
            string playerJsonString = jSaveGame[curtimer.myName].ToString();
            //Llamamos al m�todo que deserializa la informaci�n obtenida
            curtimer.Deserialize(playerJsonString);
        }
        else {
            Debug.Log("Save file not found in " + saveFilePath);
        }

    }

    /*
     * PARA ENCRIPTAR Y DESENCRIPTAR LA INFORMACI�N DEL ARCHIVO DE GUARDADO
     */

    //Clave generada para la encriptaci�n en formato bytes, 16 posiciones
    static byte[] _key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    //Vector de inicializaci�n para la clave
    static byte[] _initializationVector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    //Encriptamos los datos del archivo de guardado que le pasaremos en un string
    static byte[] Encrypt(string message) {
        //Usamos esta librer�a que nos permitir� a trav�s de una referencia crear un encriptador de la informaci�n
        AesManaged aes = new AesManaged();
        //Para usar este encriptador le pasamos tanto la clave como el vector de inicializaci�n que hemos creado nosotros arriba
        ICryptoTransform encryptor = aes.CreateEncryptor(_key, _initializationVector);
        //Lugar en memoria donde guardamos la informaci�n encriptada
        MemoryStream memoryStream = new MemoryStream();
        //Con esta referencia podremos escribir en el MemoryStream de arriba la informaci�n ya encriptada usando el encriptador con sus claves que ya hab�amos creado
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        //Con el StreamWriter podemos escribir en el archivo la informaci�n encriptada, que se habr� guardado en el MemoryStream
        StreamWriter streamWriter = new StreamWriter(cryptoStream);

        //Usando todo lo anterior, guardamos en el archivo de guardado el json que le pasamos por par�metro, haciendo el siguiente proceso: recibimos el string, lo encriptamos, queda guardado en la memoria reservada para la encriptaci�n
        streamWriter.WriteLine(message);

        //Una vez hemos usado estas referencias las cerramos para evitar problemas de guardado o corrupci�n del archivo o de la propia encriptaci�n
        streamWriter.Close();
        cryptoStream.Close();
        memoryStream.Close();

        //Por �ltimo el m�todo devolver� esta informaci�n que reside en el hueco de memoria con la informaci�n encriptada, convertida esta informaci�n en array de bytes
        return memoryStream.ToArray();
    }

    //Generamos un m�todo que nos devuelva la informaci�n del archivo de guardado desencriptada
    static string Decrypt(byte[] message) {
        //Usamos esta librer�a que nos permitir� a trav�s de una referencia crear un desencriptador de la informaci�n
        AesManaged aes = new AesManaged();
        //Para usar este desencriptador le pasamos tanto la clave como el vector de inicializaci�n que hemos creado nosotros arriba
        ICryptoTransform decrypter = aes.CreateDecryptor(_key, _initializationVector);
        //Lugar en memoria donde guardamos la informaci�n desencriptada
        MemoryStream memoryStream = new MemoryStream(message);
        //Con esta referencia podremos escribir en el MemoryStream de arriba la informaci�n ya desencriptada usando el desencriptador con sus claves que ya hab�amos creado
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Read);
        //Con el StreamReader podemos leer del archivo la informaci�n desencriptada, que se habr� guardado en el MemoryStream
        StreamReader streamReader = new StreamReader(cryptoStream);

        //Usando todo lo anterior, cargamos del archivo de guardado el json que le pasamos por par�metro, haciendo el siguiente proceso: recibimos el string, lo desencriptamos, queda guardado en la memoria reservada para la desencriptaci�n
        string decryptedMessage = streamReader.ReadToEnd();

        //Una vez hemos usado estas referencias las cerramos para evitar problemas de guardado o corrupci�n del archivo o de la propia encriptaci�n
        streamReader.Close();
        cryptoStream.Close();
        memoryStream.Close();

        //Por �ltimo el m�todo devolver� esta informaci�n que reside en el hueco de memoria con la informaci�n desencriptada, convertida esta en un string
        return decryptedMessage;
    }
}
