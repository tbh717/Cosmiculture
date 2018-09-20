using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    Material mat;
    bool scrollEnabled;

    Vector2 movementVector;
    
	void Start() {
        mat = GetComponent<Renderer>().material;
        mat.SetTextureScale("_MainTex", new Vector2(transform.localScale.x/10f, transform.localScale.y/10f));
		scrollEnabled = true;

        movementVector = new Vector2(0.0005f, 0.0001f);
	}
	
	// Update is called once per frame
	void Update() {
		if(scrollEnabled) {
            Vector2 curOffset = mat.GetTextureOffset("_MainTex");
            Vector2 offset = curOffset + movementVector;
            mat.SetTextureOffset("_MainTex", offset);
        }
	}

    void Pause() {
        scrollEnabled = false;
    }

    void Unpause() {
        scrollEnabled = true;
    }
}
