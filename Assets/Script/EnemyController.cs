using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Variable velocidad enemigo
    public float moveSpeed;

    //Puntos entre los que se moverá el enemigo
    public Transform leftPoint, rightPoint;

    //Variable para saber si el enemigo se mueve a la derecha o a la izquierda
    private bool movingRight;

    //El rigidbody del enemigo para que podamos moverlo
    private Rigidbody2D theRB;
    //El SpriteRenderer del enemigo para que podamos cambiar sus sprites
    public SpriteRenderer theSR;
    //El componente de animator del enemigo
    private Animator anim;

    //Cuanto tiempo se debe mover el enemigo, y cuanto debe esperar antes de volver a moverse
    public float moveTime, waitTime;
    //Valores que usaremos para contar el tiempo de movimiento y de espera
    private float moveCount, waitCount;


    // Start is called before the first frame update
    void Start()
    {
        //Inicializamos el rigidbody
        theRB = GetComponent<Rigidbody2D>();
        //Inicializamos el animator
        anim = GetComponent<Animator>();

        //LeftPoint y RightPoint ya no serán hijos de este enemigo, así evitamos que se mueva con respecto a la posición de este
        leftPoint.parent = null;
        rightPoint.parent = null;

        //Al empezar el juego se mueve hacia la derecha
        movingRight = true;

        //El contador será igual al tiempo de movimiento
        moveCount = moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Si el contador de tiempo de movimiento está lleno
        if (moveCount > 0)
        {
            //El contador empieza a disminuir
            moveCount -= Time.deltaTime;

            //Si se mueve hacia la derecha
            if (movingRight)
            {
                //Se aplica la velocidad de movimiento en X y mantiene la que tuviera en Y
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);

                //Volteamos el gráfico del enemigo para que mire a la derecha
                theSR.flipX = true;

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
                theSR.flipX = false;

                //Si la posición del enemigo es menor que la del punto máximo en la izquierda
                if (transform.position.x < leftPoint.position.x)
                {
                    //Ya no nos movemos a la izquierda
                    movingRight = true;
                }
            }

            //Si el contador de tiempo de movimiento está vacío
            if (moveCount <= 0)
            {
                //El contador de espera será
                waitCount = Random.Range(waitTime * .75f, waitTime * 1.25f);
            }

            //Hacemos que la animación del enemigo moviéndose se active
            anim.SetBool("isMoving", true);
        }
        //Si el contador de tiempo de espera está lleno
        else if (waitCount > 0)
        {
            //El contador empieza a disminuir
            waitCount -= Time.deltaTime;

            //Paramos al jugador en el sitio
            theRB.velocity = new Vector2(0f, theRB.velocity.y);

            //Si el contador de tiempo de espera está vacío
            if (waitCount <= 0)
            {
                //El contador de tiempo de movimiento será aleatorio entre unos valores
                moveCount = Random.Range(moveTime * .75f, waitTime * .75f);
            }

            //Hacemos que la animación del enemigo moviéndose se desactive
            anim.SetBool("isMoving", false);
        }
        
    }
}
