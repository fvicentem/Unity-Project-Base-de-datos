using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour
{
    [SerializeField] public string sceneName;
    GameObject rankingGO;
    public RankingManager puntuación;

    public ManagerSavingObjects jsonsave;

    private void Start()
    {
        jsonsave = FindObjectOfType<ManagerSavingObjects>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //manda la puntuación obtenida a la base de datos
        //puntuación.InsertarPuntos();
        //rankingGO.GetComponent<RankingManager>().InsertarPuntos(Timer.)
        DataBaseManager.SaveMonedas(LevelManager.instance.GemCollected);

        if (collision.gameObject.tag == "Player")
        {
            jsonsave.Save();
            SceneManager.LoadScene(sceneName);
        }
    }
}
