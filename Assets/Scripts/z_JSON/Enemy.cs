using Newtonsoft.Json.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float damage = 10f;

    public class EnemyData {
        //Variables para serializar
        public float currentHealth;
        public float speed;
        public Vector3 pos;

        //Constructor de la clase
        public EnemyData(Transform transform, float currentHealth, float speed) {
            //Rellenamos las variables con las que le pasamos por par�metro
            pos = transform.position;
            this.currentHealth = currentHealth;
            this.speed = speed;
        }
    }

    //Crearemos un objeto serializable capaz de ser guardado
    public JObject Serialize() {
        //Instanciamos la clase anidada pas�ndole por par�metro las variables que queremos guardar
        EnemyData data = new EnemyData(transform, currentHealth, speed);

        //Creamos un string que guardar� el jSon
        string jsonString = JsonUtility.ToJson(data);
        //Creamos un objeto en el jSon
        JObject retVal = JObject.Parse(jsonString);
        //Al ser un m�todo de tipo, debe devolver este tipo
        return retVal;
    }

    //Tendremos que deserializar la informaci�n recibida
    public void Deserialize(string jsonString) {
        EnemyData data = new EnemyData(transform, currentHealth, speed);
        //La informaci�n recibida del archivo de guardado sobreescribir� los campos oportunos del jsonString
        JsonUtility.FromJsonOverwrite(jsonString, data);

        // Actualizamos los datos del enemigo con los datos del archivo de guardado
        transform.position = data.pos;
        currentHealth = data.currentHealth;
        speed = data.speed;
    }
}
