using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGen : MonoBehaviour {

    public GameObject hexPrefab;

    float xOffset = 0.882f;
    float yOffset = 0.764f;

    // Size of map in terms of hex tiles
    int height = 5;
    int width = 5;

	void Start () {
		for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {

                float xPos = x * xOffset;
                
                // On an "odd" row
                if(y % 2 == 1) {
                    xPos += xOffset/2;
                }

                GameObject newTile = (GameObject) Instantiate(hexPrefab, new Vector2(xPos,y * yOffset), Quaternion.identity);
                
                newTile.name = "Hex_" + x + "_" + y;
                newTile.transform.SetParent(this.transform);
            }
        }
	}
	
	void Update () {
		
	}
}
