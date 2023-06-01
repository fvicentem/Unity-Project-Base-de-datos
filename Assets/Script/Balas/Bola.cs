using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bola : MonoBehaviour
{

    public float velX, velY;

    public float time;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(velX, velY);
        StartCoroutine(DestruirCo());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("AH ME HA DADO");
            //Inicializar explosi�n o algo
            Destroy(gameObject);
            //Damage Player
            PlayerHealthController.instance.DealWithDamage();

        }

    }


    IEnumerator DestruirCo()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
