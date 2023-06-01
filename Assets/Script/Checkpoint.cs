using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //Variables control sprite checkpoint activo/desactivado
    public Sprite cpOn, cpOff;

    //Variable para acceder al Sprite Renderer
    public SpriteRenderer theSR;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Al entrar en la zona checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Se compara si el objeto que entra al trigger tiene por etiqueta "Player"
        if (collision.CompareTag("Player"))
        {
            //Desactiva los checkpoints
            CheckpointController.instance.DeactivateCheckpoints();
            //Ponemos la imagen del checkpoint activado
            theSR.sprite = cpOn;
            //Haremos el checkpoint en el que estamos, el nuevo punto de spawn
            CheckpointController.instance.SetSpawnPoint(transform.position); //le pasamos la posición del checkpoint
        }
    }

    //Método para cambiar la imagen de los checkpoints activos
    public void ResetCheckpoint()
    {
        //Ponemos la imagen del checkpoint desactivado
        theSR.sprite = cpOff;
    }
}
