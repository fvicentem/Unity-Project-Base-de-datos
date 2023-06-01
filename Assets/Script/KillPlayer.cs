using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Para detectar cuando el jugador entra en la zona de muerte
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si es el jugador el que entra en el trigger
        if (collision.tag == "Player")
        {
            Debug.Log("Entrado");
            //Lo respawneamos, quitándole previamente toda la vida y mostrándolo por pantalla
            PlayerHealthController.instance.currentHealth = 0;
            UIController.instance.UpdateHealthDisplay();
            LevelManager.instance.RespawnPlayer();
        }
    }
}
