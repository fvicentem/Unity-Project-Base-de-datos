using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Librería para poder hacer cambios entre niveles
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Hacemos un Singleton del script. Es decir, hacemos que solamente pueda haber un script de este tipo. Podremos acceder con esta instance a este script desde cualquier lugar
    public static LevelManager instance;

    //Variable para controlar el tiempo de espera antes de volver a spawnear
    public float waitToRespawn;

    //Variable para llevar un conteo de gemas que hemos cogido
    public int gemCollected;

    public int GemCollected
    {
        get { return gemCollected; }
        set 
        { 
            gemCollected = value;
        }
    }



    //Variable donde guardar el tiempo de completar nivel
    public float timeInLevel;

    //Método que se ejecuta antes de empezar el juego
    private void Awake()
    {
        //Significa que la instance del UIController va a ser este propio script
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Inicializamos el tiempo que llevamos en el nivel
        timeInLevel = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Vamos aumentado el tiempo que llevamos jugando el nivel
        timeInLevel += Time.deltaTime;
    }

    //Método para hacerle respawn al jugador
    public void RespawnPlayer()
    {
        //Llamamos a la corutina
        StartCoroutine(RespawnCo());
    }

    //Corutina
    private IEnumerator RespawnCo()
    {
        //Desactivamos al jugador
        PlayerController.instance.gameObject.SetActive(false);
        
        
        //Reproducimos el efecto de sonido que queremos 
        //AudioManager.instance.PlaySFX(8);
        
        
        //Espera este tiempo dado
        yield return new WaitForSeconds((1f / UIController.instance.fadeSpeed) - .2f);
        
        
        //Hacemos fundido a negro
        UIController.instance.FadeToBlack();
        
        
        //Espera este tiempo dado
        yield return new WaitForSeconds(waitToRespawn - (1f / UIController.instance.fadeSpeed));
        
        
        //Hacemos fundido a transparente
        UIController.instance.FadeFromBlack();
        
        
        //Activamos al jugador
        PlayerController.instance.gameObject.SetActive(true);
        PlayerController.instance.canDash = true;
        
        
        //Movemos al personaje al punto de spawneo
        PlayerController.instance.transform.position = CheckpointController.instance.spawnPoint;
        
        
        //Reseteamos la vida al máximo y la mostramos por la interfaz
        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
        UIController.instance.UpdateHealthDisplay();
    }

  
}
