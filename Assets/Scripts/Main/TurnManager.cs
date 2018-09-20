using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {

    public int turn;
    public int maxTurns;

    public Text scoreText;

	// Use this for initialization
	void Start() {
		turn = 0;
        maxTurns = 20;
        
        scoreText = GameObject.Find("TurnText").GetComponent<Text>();
        UpdateScoreText();
	}
	
	// Update is called once per frame
	void Update() {
		
	}

    void NextTurn() {
        turn++;
        UpdateScoreText();
        
        if(turn <= maxTurns) {
            
        }
        else {
            EndGame();
        }
    }

    void EndGame() {

    }

    public int GetTurn() {
        return turn;
    }

    private void UpdateScoreText() {
        scoreText.text = turn.ToString();
    }
}
