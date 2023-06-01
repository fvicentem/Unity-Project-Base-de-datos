using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

public class Timer1 : MonoBehaviour
{
    public float timeValue = 90;
    public Text timeText;
    public string myName = "timer";

    // Update is called once per frame
    void Update()
    {

        if (SceneManager.GetActiveScene().name.Equals("MainMenú 1"))
            return;

        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
        }
        DisplayTime(timeValue);
        if (timeValue <= 0)
        {
            SceneManager.LoadScene("Game Over 1");
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);


    }

    public class TimerData
    {
        //Variables para serializar
        public float timeValue;
        public string timeText;


        //Constructor de la clase
        public TimerData(float timeValue, string timeText)
        {
            //Rellenamos las variables con las que le pasamos por parámetro
            this.timeText = timeText;
            this.timeValue = timeValue;
        }
    }

    //Crearemos un objeto serializable capaz de ser guardado
    public JObject Serialize()
    {
        //Instanciamos la clase anidada pasándole por parámetro las variables que queremos guardar
        TimerData data = new TimerData(timeValue, timeText.text);

        //Creamos un string que guardará el jSon
        string jsonString = JsonUtility.ToJson(data);
        //Creamos un objeto en el jSon
        JObject retVal = JObject.Parse(jsonString);
        //Al ser un método de tipo, debe devolver este tipo
        return retVal;
    }

    //Tendremos que deserializar la información recibida
    public void Deserialize(string jsonString)
    {
        TimerData data = new TimerData(timeValue, timeText.text);
        //La información recibida del archivo de guardado sobreescribirá los campos oportunos del jsonString
        JsonUtility.FromJsonOverwrite(jsonString, data);

        // Actualizamos los datos del enemigo con los datos del archivo de guardado
        timeText.text = data.timeText;
        timeValue = data.timeValue;
    }
}
