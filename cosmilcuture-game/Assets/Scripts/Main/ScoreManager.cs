using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    // Only accounts for one-time bonuses, such as harvesting crops or powerups.
    public int score;
    
	void Start () {
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddToScore(int s) {
        score += s;
    }

    public int GetScore() {
        return score;
    }
}
