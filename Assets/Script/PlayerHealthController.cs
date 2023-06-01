using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    //Hacemos un Singleton del script. Es decir, hacemos que solamente pueda haber un script de este tipo. Podremos acceder con esta instance a este script desde cualquier lugar
    public static PlayerHealthController instance;

    //Variables para controlar la vida actual y también el máximo de vida que podemos tener (esto es debido a que en un momento dado querremos recuperar vida, y no podremos recuperar más del máximo de vida permitido)
    public int currentHealth, maxHealth;

    //Variable control tiempo que el jugador es invencible al sufrir daño
    public float invincibleLength;
    //Contador del tiempo en activo de la invencibilidad
    private float invincibleCounter;

    //Variable para acceder al sprite renderer del jugador
    private SpriteRenderer theSR;

    //Variable para guardar el objeto que queremos instanciar al morir
    public GameObject deathEffect;

    //Método que se ejecuta antes de empezar el juego
    private void Awake()
    {
        //Significa que la instance del PlayerHealthController va a ser este propio script
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Cuando empieza el juego
        currentHealth = maxHealth;
        //Inicializamos el sprite renderer del jugador
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Si el contador de invencibilidad es mayor que 0
        if (invincibleCounter > 0)
        {
            //Le resto 1 a ese contador cada segundo
            invincibleCounter -= Time.deltaTime; //Time.deltaTime hace la condición impuesta en un segundo
            //Si el contador de invencibilidad ya ha llegado a 0
            if (invincibleCounter <= 0)
            {
                //Metemos en el sprite renderer un nuevo color al que le pasamos los valores RGB que ya tenía el jugador, y cambiamos el valor de la opacidad al máximo
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f); //El valor de alpha está entre 0 y 1
            }
        }
        
    }

    //Método para manejar el daño
    public void DealWithDamage()
    {
        //Si el contador para poder volver a hacernos daño ha llegado a 0
        if (invincibleCounter <= 0)
        {
            //Restamos 1 de la vida que tengamos
            currentHealth--; // currentHealth -= 1; currentHealth = currentHealth - 1;

            //Si la vida está en 0 o por debajo (para asegurarnos de tener en cuenta solo valores positivos)
            if (currentHealth <= 0)
            {
                //Si por alguna razón la vida bajara por debajo de 0, la hacemos 0
                currentHealth = 0;
                //Hacemos desaparecer de momento al jugador
                //gameObject.SetActive(false);

                //Instanciamos el objeto que reproduce la animación de muerte
                Instantiate(deathEffect, transform.position, transform.rotation);

                //Respawneamos al jugador
                LevelManager.instance.RespawnPlayer();
            }
            //Estamos aún vivos pero sufriendo daño
            else
            {
                //El contador de tiempo tendrá el mismo valor que el estado de invencibilidad
                invincibleCounter = invincibleLength;
                //Metemos en el sprite renderer un nuevo color al que le pasamos los valores RGB que ya tenía el jugador, y cambiamos el valor de la opacidad a la mitad
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, .5f); //El valor de alpha está entre 0 y 1

                //Llamamos al PlayerController para realizar el knockback
                PlayerController.instance.KnockBack();
            }

            //Llamamos al método que cambia el estado de los corazones
            UIController.instance.UpdateHealthDisplay();
        }
    }

    //Método que nos servirá para curar al jugador
    public void HealPlayer()
    {
        //Si quisieramos curar la vida entera
        //currentHealth = maxHealth;
        //Sumamos 1 a la vida actual del jugador
        currentHealth++;
        //Si al curarnos la vida sobrepasase la vida máxima
        if (currentHealth > maxHealth)
        {
            //Volvemos a la vida máxima
            currentHealth = maxHealth;
        }
        //Actualizamos la UI
        UIController.instance.UpdateHealthDisplay();
    }

}
