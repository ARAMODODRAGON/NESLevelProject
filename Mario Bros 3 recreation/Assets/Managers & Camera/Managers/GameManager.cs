using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager inst;

    private void Awake() {
        if (inst == null) {
            inst = this;
        } else {
            Debug.LogError("There is already a GameManager in the game");
        }

        DontDestroyOnLoad(gameObject);
    }

    public static void LoadLevel(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

}
