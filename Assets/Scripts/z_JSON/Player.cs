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
            //Rellenamos las variables con las que le pasamos por parámetro
            pos = transform.position;
            this.currentHealth = currentHealth;
            this.speed = speed;
        }
    }

    //Crearemos un objeto serializable capaz de ser guardado
    public JObject Serialize() {
        //Instanciamos la clase anidada pasándole por parámetro las variables que queremos guardar
        PlayerData data = new PlayerData(transform, currentHealth, speed);

        //Creamos un string que guardará el jSon
        string jsonString = JsonUtility.ToJson(data);
        //Creamos un objeto en el jSon
        JObject retVal = JObject.Parse(jsonString);
        //Al ser un método de tipo, debe devolver este tipo
        return retVal;
    }

    //Tendremos que deserializar la información recibida
    public void Deserialize(string jsonString) {
        PlayerData data = new PlayerData(transform, currentHealth, speed);
        //La información recibida del archivo de guardado sobreescribirá los campos oportunos del jsonString
        JsonUtility.FromJsonOverwrite(jsonString, data);

        // Actualizamos los datos del enemigo con los datos del archivo de guardado
        transform.position = data.pos;
        currentHealth = data.currentHealth;
        speed = data.speed;
    }
}
