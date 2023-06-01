using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogerItem : MonoBehaviour
{
    //Variable para comprobar si el item que hemos cogido es una gema o una cura
    public bool isGem;
    //Variable para evitar que el mismo item se coja 2 veces
    private bool isCollected;

    //Variable para guardar el objeto que queremos instanciar al coger un item
    public GameObject pickUpEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el objeto que ha colisionado con el item es el jugador y este item no ha sido cogido antes
        if (collision.CompareTag("Player") && !isCollected)
        {
            //Si el item es una gema
            if (isGem)
            {
                //Aumentamos en 1 la cantidad de gemas coleccionadas
                LevelManager.instance.GemCollected++;
                //La gema ya ha sido cogida
                isCollected = true;
                //Tras coger la gema el objeto se destruye
                Destroy(gameObject);

                //Actualizamos el contador de gemas
                UIController.instance.UpdateGemCount();

                //Reproducimos el efecto de sonido que queremos 
                //AudioManager.instance.PlaySFX();
            }

        }
    }
}
