using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //Hacemos un Singleton del script. Es decir, hacemos que solamente pueda haber un script de este tipo. Podremos acceder con esta instance a este script desde cualquier lugar
    public static CameraController instance;

    //Variable de tipo transform para que la cámara pueda seguir una posición
    public Transform target;
    //Variables para control Parallax


    //COMENTADO PARA QUE NO SALGAN ERRORES
    public Transform farBackground;
    //Variable para hacer un seguimiento del movimiento, nos dará la última posición en el eje X y en el eje Y en la que estábamos
    private Vector2 lastPos;
    //Variables para controlar la altura mínima y máxima que la cámara puede alcanzar
    public float minHeight, maxHeight;

    //Variable para controlar si la cámara sigue o no al jugador
    public bool stopFollow;

    // Start is called before the first frame update
    void Start()
    {
        //La última posición en la que estuvimos al empezar el juego, fué en la posición de inicio en X y en Y de la cámara
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*//Movimiento de la cámara, que sigue al jugador en X e Y, no en Z
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        //Creamos una variable para controlar la subida y bajada de la cámara
        float clampedY = Mathf.Clamp(transform.position.y, minHeight, maxHeight); //Clamp: valor que quieres cambiar, minimoValor, maximoValor

        //La cámara sólo se podrá mover siguiendo al jugador, sin salirse de los valores mínimo y máximo de altura
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);*/

        //Esta línea equivale a todo lo comentado arriba
        transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);
        farBackground.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), farBackground.position.z);

        //Cantidad de movimiento en X y en Y que debe hacer la cámara
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

        //Las nubes se mueven conforme a la cámara
        //farBackground.position = farBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0f);
        //Los arboles se mueven a la mitad de velocidad de la cámara
        //middleBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .5f; //Es lo mismo new Vector3(amountToMove.x * .5f, amountToMove.y * .5f, 0f);
        //Actualizamos la última posición
        lastPos = transform.position;
    }
}
