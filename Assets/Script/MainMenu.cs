using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Text monedatext;
    public Text tiempotext;

    private void Start()
    {
        monedatext.text = DataBaseManager.LoadMonedas().ToString();
    }

    public void EscenaJuego1()
    {
        SceneManager.LoadScene("Nivel 1");
    }

    public void EscenaTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void CargarNivel(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
