using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Hacemos un Singleton del script. Es decir, hacemos que solamente pueda haber un script de este tipo. Podremos acceder con esta instance a este script desde cualquier lugar
    public static PlayerController instance;

    /*VARIABLES MOVIMIENTO + SALTO*/
    //Velocidad de movimiento del jugador
    public float moveSpeed;
    //RigidBody del jugador
    public Rigidbody2D theRB;
    //Variable para fuerza de salto
    public float jumpForce;

    [SerializeField] private float m_DashForce = 25f;
    public float cooldownDash = 0.5f;
    public float tiempoDash = 0.1f;
    public bool canDash = true;
    private bool isDashing = false; //If player is dashing
                                    //Fin de valores básicos del Dash

    [SerializeField]
    private Collider2D colliderSoloEnemigos;
    [SerializeField]
    private Collider2D colliderSoloEntorno;
    private Collider2D colliderGeneral;

    /*VARIABLES PARA DETECTAR SI HAY SUELO DEBAJO*/
    //Variable para saber si estamos en el suelo
    private bool isGrounded;
    //Hacemos una variable de tipo Transform(x,y) con la que analizaremos si en ese punto hay suelo
    public Transform groundCheckPoint;
    //Una variable para saber que Layer es el suelo
    public LayerMask whatIsGround;

    //Variable para ver si podemos ejecutar doble salto
    private bool canDoubleJump;

    /*VARIABLES PARA ANIMACIÓN*/
    //Variable para acceder al controlador de animaciones
    private Animator anim;
    //Variable que nos permite acceder al SpriteRenderer de nuestro jugador
    private SpriteRenderer theSR;

    //Variable para el control del Knockback, su longitud y su fuerza
    public float knockBackLength, knockBackForce;
    //Contador para el tiempo de KnockBack
    private float knockBackCounter;

    //Variable fuerza de rebote en enemigos
    public float bounceForce;

    //Variable para controlar si podemos seguir pulsando teclas o no
    public bool stopInput;

    //Método que se ejecuta antes de empezar el juego
    private void Awake()
    {
        colliderGeneral = this.GetComponent<Collider2D>(); 
        colliderSoloEntorno.enabled = false;

        //Significa que la instance del UIController va a ser este propio script
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Inicialización del animator de nuestro Player
        anim = GetComponent<Animator>(); //vete al GO donde está este Script metido, y coge el componente de Animator Controller
        //Inicialización del SpriteRenderer de nuestro Player
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Si el contador para poder volver a movernos ha llegado a 0
        if (knockBackCounter <= 0)
        {

            if (SimpleInput.GetButtonDown("Dash"))
            {
                //Debug.Log("Entra a dash");
                if (canDash)
                {
                    Debug.Log("Va a hacer la corrutina");
                    
                    StartCoroutine(DashCooldown()); 
                }
                // If crouching, check to see if the character can stand up

            }

            
            if (isDashing)
            {
                if (!theSR.flipX)
                    theRB.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
                else
                    theRB.velocity = new Vector2(-transform.localScale.x * m_DashForce, 0);
            }
            else
                //Accede al RigidBody y cambia su velocidad. Para ello le pasamos un vector de dos dimensiones dónde: el movimiento en X será la velocidad(multiplicado por la pulsación de una tecla), y el movimiento en Y será la gravedad
                theRB.velocity = new Vector2(moveSpeed * SimpleInput.GetAxis("Horizontal"), theRB.velocity.y);

            //isGrounded variará entre verdadero o falso, si detectamos algo bajo los pies o no
            //Se crea un círculo bajo los pies del jugador a través del objeto Ground Point, que es capaz de detectar si está en colisión con algo
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround); //Al círculo se le pasa la posición de origen donde tiene que dibujarse, y el radio de este, y el layer con el que queremos que interaccione

            //Si estamos en el suelo, se resetea el doble salto, haciendo la variable verdadera
            if (isGrounded)
            {
                canDoubleJump = true;
            }

            //Si pulso la tecla correspondiente al salto
            if (SimpleInput.GetButtonDown("Jump"))
            {
                //Si estoy en el suelo, cuando pulse la tecla de saltar, el jugador saltará, sino no ocurrirá nada
                if (isGrounded)
                {
                    //Accede al RigidBody y cambia su velocidad. En X coge la que ya lleve, y en Y le aplicamos la fuerza de salto que hemos creado
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                    anim.SetBool("Jump", false);
                    //Reproducimos el efecto de sonido que queremos 
                   // AudioManager.instance.PlaySFX(0);
                }
                else //Si no estoy en el suelo, quizás pueda ejecutar doble salto
                {
                    //Si puedo hacer doble salto
                    if (canDoubleJump)
                    {
                        //Accede al RigidBody y cambia su velocidad. En X coge la que ya lleve, y en Y le aplicamos la fuerza de salto que hemos creado
                        theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                        //No puedo volver a repetir salto, luego esa variable ahora es falsa
                        canDoubleJump = false;
                        //Reproducimos el efecto de sonido que queremos 
                        //AudioManager.instance.PlaySFX(1);

                    }
                }

                anim.SetBool("Jump", true);
            }

            //Si me estoy moviendo hacia la izquierda
            if (theRB.velocity.x < 0)
            {
                //Se va al SpriteRenderer del jugador y le hace un Flip en el eje X
                theSR.flipX = true;
                //Si me estoy moviendo hacia la derecha
            }
            else if (theRB.velocity.x > 0)
            {
                theSR.flipX = false;
            }

            anim.SetBool("isGrounded", isGrounded);

        }
        //Si el contador no es 0
        else
        {
            //Le resto el tiempo equivalente a un frame, cada frame, hasta llegar a 0 
            knockBackCounter -= Time.deltaTime;
            //Si estamos mirando a la izquierda
            if (theSR.flipX)
            {
                //Hacemos que salga despedido en la dirección contraria (hacia la derecha)
                theRB.velocity = new Vector2(knockBackForce, theRB.velocity.y);
            }
            //Si estamos mirando a la derecha
            else
            {
                //Hacemos que salga despedido en la dirección contraria (hacia la izquierda)
                theRB.velocity = new Vector2(-knockBackForce, theRB.velocity.y);
            }

        }

        //Cambia el valor de moveSpeed dependiendo de la velocidad del jugador a izquierda o derecha
        anim.SetFloat("moveSpeed", Mathf.Abs(theRB.velocity.x)); //Mathf.Abs convierte el número negativo si vamos hacia la izquierda en positivo
        //Cambia el valor de isGrounded para ver si estamos saltando o no
        anim.SetBool("isGrounded", isGrounded);

    }

    //Método para controlar el Knockback al ser golpeados o dañados
    public void KnockBack()
    {
        //Inicializamos el contador de tiempo del knockback
        knockBackCounter = knockBackLength;
        //Cambia el valor de hurt para mostrar su animación de daño
        anim.SetTrigger("hurt");
    }

    //Método para que el personaje rebote sobre los enemigos
    public void Bounce()
    {
        //Aplicamos la fuerza de rebote
        theRB.velocity = new Vector2(theRB.velocity.x, bounceForce);
    }



    IEnumerator DashCooldown()
    {
        //Reproducimos el efecto de sonido que queremos 
       // AudioManager.instance.PlaySFX(2);
        //animator.SetBool("IsDashing", true);
        isDashing = true;
        canDash = false;
        theRB.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        flipColliders();
        yield return new WaitForSeconds(tiempoDash);
        flipColliders();
        theRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        isDashing = false;
        yield return new WaitForSeconds(cooldownDash);
        canDash = true;
    }

    //Otra parte del Dash
    private void flipColliders()
    {
        colliderGeneral.enabled = !colliderGeneral.enabled;
        colliderSoloEntorno.enabled = !colliderSoloEntorno.enabled;
    }

}