using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    //Hacemos un Singleton del script. Es decir, hacemos que solamente pueda haber un script de este tipo. Podremos acceder con esta instance a este script desde cualquier lugar
    public static CheckpointController instance;

    //Hacemos un array donde guardar todos los checkpoints de la escena
    private Checkpoint[] checkpoints;

    //Posición en X, Y, Z del punto de spawn
    public Vector3 spawnPoint;

    //Método que se ejecuta antes de empezar el juego
    private void Awake()
    {
        //Significa que la instance del UIController va a ser este propio script
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Llena el array con los checkpoints que hayan en la escena
        checkpoints = FindObjectsOfType<Checkpoint>(); //Busca todos los objetos del tipo Checkpoint, es decir busca todos los objetos que tienen metido el script Checkpoint

        spawnPoint = PlayerController.instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Método para desactivar los checkpoints
    public void DeactivateCheckpoints()
    {
        //Hago un bucle para recorrer todos los checkpoints
        //Le pasamos una variable que irá cambiando de valor
        //Inicializamos / hasta cuando ocurre el bucle / de cuanto en cuanto
        for (int i = 0; i < checkpoints.Length; i++)
        {
            //Para cada valor de i, resetea el checkpoint
            checkpoints[i].ResetCheckpoint();
        }
    }

    //Método para posicionar el punto de spawneo, al que le pasamos una variable de Spawn
    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        //Hacemos el punto de spawneo igual al punto que le hayamos pasado
        spawnPoint = newSpawnPoint;
    }

}
