using Newtonsoft.Json.Linq;
using UnityEngine;


public class Player : MonoBehaviour {

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private Animator anim;

    public class PlayerData {
        //Variables para serializar
        public float currentHealth;
        public float speed;
        public Vector3 pos;

        //Constructor de la clase
        public PlayerData(Transform transform, float currentHealth, float speed) {
            //Rellenamos las variables con las que le pasamos por par�metro
            pos = transform.position;
            this.currentHealth = currentHealth;
            this.speed = speed;
        }
    }

    //Crearemos un objeto serializable capaz de ser guardado
    public JObject Serialize() {
        //Instanciamos la clase anidada pas�ndole por par�metro las variables que queremos guardar
        PlayerData data = new PlayerData(transform, currentHealth, speed);

        //Creamos un string que guardar� el jSon
        string jsonString = JsonUtility.ToJson(data);
        //Creamos un objeto en el jSon
        JObject retVal = JObject.Parse(jsonString);
        //Al ser un m�todo de tipo, debe devolver este tipo
        return retVal;
    }

    //Tendremos que deserializar la informaci�n recibida
    public void Deserialize(string jsonString) {
        PlayerData data = new PlayerData(transform, currentHealth, speed);
        //La informaci�n recibida del archivo de guardado sobreescribir� los campos oportunos del jsonString
        JsonUtility.FromJsonOverwrite(jsonString, data);

        // Actualizamos los datos del enemigo con los datos del archivo de guardado
        transform.position = data.pos;
        currentHealth = data.currentHealth;
        speed = data.speed;
    }
}
