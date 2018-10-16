using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Used in start screen. Begins and exits game */

public class StartGame : MonoBehaviour {

    AudioDirector ad;

    void Start() {
        ad = GameObject.Find("AudioController").GetComponent<AudioDirector>();
        ad.PlayTheme(true);
    }

	public void BeginGame() {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
