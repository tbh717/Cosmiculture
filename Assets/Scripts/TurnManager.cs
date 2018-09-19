using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public int turn;
    public int maxTurns;

	// Use this for initialization
	void Start() {
		turn = 0;
        maxTurns = 20;
	}
	
	// Update is called once per frame
	void Update() {
		
	}

    void NextTurn() {
        turn++;
        if(turn <= maxTurns) {
            
        }
        else {
            EndGame();
        }
    }

    void EndGame() {

    }
}
