using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Librería para usar la interfaz de Unity

public class UIController : MonoBehaviour
{
    //Hacemos un Singleton del script. Es decir, hacemos que solamente pueda haber un script de este tipo. Podremos acceder con esta instance a este script desde cualquier lugar
    public static UIController instance;

    //Variables para cambiar las imágenes
    public Image heart1, heart2, heart3;
    //Variables de las imágenes que queremos cambiar
    public Sprite heartFull, heartEmpty, heartHalf;
    //Variable del texto que queremos cambiar
    public Text gemText;

    

    //Variable donde guardar el panel de fundido a negro
    public Image fadeScreen;
    //Velocidad de fundido a negro
    public float fadeSpeed;
    //Variables para transcionar entre un estado y otro
    private bool shouldFadeToBlack, shouldFadeFromBlack;

    //Variable para el texto de haber terminado el nivel
    public GameObject levelCompleteText;

    //Método que se ejecuta antes de empezar el juego
    private void Awake()
    {
        //Significa que la instance del UIController va a ser este propio script
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

       

        //Al empezar el juego ponemos el contador de gemas en su número inicial
        UpdateGemCount();
        //Inicializamos el nivel haciendo un fundido a transparente
        FadeFromBlack();
    }

   

    // Update is called once per frame
    void Update()
    {
        //Si hacemos fundido a negro
        if (shouldFadeToBlack)
        {
            //Hacemos el panel opaco
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime)); //MoveTowards nos lleva desde un punto A a un punto B, le pasamos 3 valores, la variable que cambiamos, el punto al que debe llegar, y una velocidad de transición
            //Si la imagen ya es totalmente opaca
            if (fadeScreen.color.a == 1f)
            {
                //Ya no seguimos haciendo el fundido a negro
                shouldFadeToBlack = false;
            }
        }
        //Si volvemos a transparente
        if (shouldFadeFromBlack)
        {
            //Hacemos el panel transparente
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime)); //MoveTowards nos lleva desde un punto A a un punto B, le pasamos 3 valores, la variable que cambiamos, el punto al que debe llegar, y una velocidad de transición
            //Si la imagen ya es totalmente transparente
            if (fadeScreen.color.a == 0f)
            {
                //Ya no seguimos haciendo el fundido a transparente
                shouldFadeFromBlack = false;
            }
        }
    }

    //Método donde actualizar el estado de los corazones
    public void UpdateHealthDisplay()
    {
        //En base a la vida actual que tengamos
        switch (PlayerHealthController.instance.currentHealth)
        {
            //Si la vida actual es 6
            case 6:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;

                break;

            //Si la vida actual es 5
            case 5:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;

                break;

            //Si la vida actual es 4
            case 4:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;

                break;

            //Si la vida actual es 3
            case 3:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;

                break;

            //Si la vida actual es 2
            case 2:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;

                break;

            //Si la vida actual es 1
            case 1:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;

                break;

            //Si la vida actual es 0
            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;

                break;

            //Si la vida actual es cualquier otro valor
            default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;

                break;
        }
    }

    //Método para actualizar la cuenta de las gemas recogidas
    public void UpdateGemCount()
    {
        //Cambiamos el texto de las gemas
        gemText.text = LevelManager.instance.gemCollected.ToString(); //ToString() Convierte un número en letras
        //gemText.text = DataBaseManager.LoadMonedas().ToString();
    }

    //Método para hacer fundido a negro
    public void FadeToBlack()
    {
        //Hacemos verdadero el fundido a negro
        shouldFadeToBlack = true;
        //Hacemos falso el fundido a transparente
        shouldFadeFromBlack = false;
    }

    //Método para hacer fundido a transparente
    public void FadeFromBlack()
    {
        //Hacemos verdadero el fundido a transparente
        shouldFadeFromBlack = true;
        //Hacemos falso el fundido a negro
        shouldFadeToBlack = false;
    }
}
