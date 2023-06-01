using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    public static PlatformMovement instance;

    public float moveSpeed;

    //Puntos entre los que se moverá el enemigo
    public Transform leftPoint, rightPoint;

    //Variable para saber si el enemigo se mueve a la derecha o a la izquierda
    private bool movingRight;

    //Variable para declarar el rigidbody del enemigo para que podamos moverlo

    private Rigidbody2D theRB;



    private void Awake()
    {
        //Significa que la instance del UIController va a ser este propio script
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

        theRB = GetComponent<Rigidbody2D>();

        //LeftPoint y RightPoint ya no serán hijos de este enemigo, así evitamos que se mueva con el padre.
        leftPoint.parent = null;
        rightPoint.parent = null;

        //Al empezar el juego se mueve hacia la derecha.
        movingRight = true;

    }

    // Update is called once per frame
    void Update()
    {
        //Si se mueve hacia la derecha
        if (movingRight)
        {
            //Se aplica la velocidad de movimiento en X y mantiene la que tuviera en Y
            theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);

            //Volteamos el gráfico del enemigo para que mire a la derecha


            //Si la posición del enemigo es mayor que la del punto máximo en la derecha
            if (transform.position.x > rightPoint.position.x)
            {
                //Ya no nos movemos a la derecha
                movingRight = false;
            }
        }
        //Si se mueve hacia la izquierda
        else
        {
            //Se aplica la velocidad de movimiento en X negativa y mantiene la que tuviera en Y
            theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);

            //Volteamos el gráfico del enemigo para que mire a la izquierda


            //Si la posición del enemigo es menor que la del punto máximo en la izquierda
            if (transform.position.x < leftPoint.position.x)
            {
                //Ya no nos movemos a la izquierda
                movingRight = true;
            }
        }



    }
}



