using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorPlataformaMóvil : MonoBehaviour
{
    public Transform puntoInicio;
    public Transform plataforma;
    public Transform puntoFinal;

    public float speed;
    public bool pingpong = true;
    public Transform currenttarget;



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(puntoInicio.position, puntoFinal.position);
        Gizmos.DrawSphere(puntoInicio.position, 0.1f);
        Gizmos.DrawSphere(puntoFinal.position, 0.1f);

    }

    private void Start()
    {
        puntoInicio.parent = null;
        puntoFinal.parent = null;
        plataforma.gameObject.SetActive(true);
        plataforma.position = puntoInicio.position;
        currenttarget = puntoFinal;
        
    }


    private void FixedUpdate()
    {
        float timedspeed = speed * Time.deltaTime;
        plataforma.position = Vector3.MoveTowards(plataforma.position, currenttarget.position, timedspeed);


        if (plataforma.position == currenttarget.position)
        {
            if (currenttarget == puntoFinal)
            {
                currenttarget = puntoInicio;
            }
            else
            {
                currenttarget = puntoFinal;
            }
        }
    }

}
