using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Used in final scene to display score */

public class EndGame : MonoBehaviour {

    int score;

	// Use this for initialization
	void Start () {
		score = PlayerPrefs.GetInt("score");
        GameObject.Find("ScoreText").GetComponent<Text>().text = score.ToString();
        StartCoroutine(GameControl());
	}

    IEnumerator GameControl() {
        while(true) {
            if(Input.GetKey(KeyCode.Escape)) {
                SceneManager.LoadScene("StartScreen");
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
