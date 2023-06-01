using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSavingObjects : MonoBehaviour {

    [SerializeField] private Timer1 curtime;

    private void Start() {
        if (SceneManager.GetActiveScene().name.Equals("MainMenú 1"))
            Load();
    }

    public void Save() {
        JsonManager.SaveGame(curtime);
    }

    public void Load() {
        JsonManager.LoadGame(curtime);
    }

}
