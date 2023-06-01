using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Método que va a detectar cuando entramos en el trigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si es el jugador el que entra en el trigger
        if (collision.tag == "Player")
        {
            //Debug.Log("Hit");
            //Una manera de llamar al método que queremos del script deseado
            //FindObjectOfType<PlayerHealthController>().DealWithDamage();
            //Del PlayerHealthController uso la instancia que contiene todo el código, y de este saco el método que necesito
            PlayerHealthController.instance.DealWithDamage();
        }
    }
}
