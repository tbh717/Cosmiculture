using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteraction : MonoBehaviour {

    private Color primaryColor;

	// Use this for initialization
	void Start () {
		primaryColor = GetComponent<SpriteRenderer>().color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver () {
        ChangeColor(Color.red);
    }

    void OnMouseExit () {
        ChangeColor(primaryColor);
    }

    void ChangeColor (Color newColor) {
        transform.GetComponent<SpriteRenderer>().color = newColor;
    }
}
