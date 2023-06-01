using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSurface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = this.transform;
            //Debug.Log("<MUEVETE HIJO DE PUTA>");
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            other.gameObject.transform.parent = null;
        }
    }
}
