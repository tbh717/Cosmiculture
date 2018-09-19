using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {

    public GameObject director;

    private SpriteRenderer sr;
    public Color primaryColor;

    private List<GameObject> neighbors;

	// Use this for initialization
	void Start () {
        director = GameObject.Find("Director");
        sr = transform.GetComponent<SpriteRenderer>();
		primaryColor = sr.color;
        neighbors = GetNeighbors();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /* MOUSE INTERACTION */
    void OnMouseEnter () {
        // ChangeColor(Color.red, Color.blue);
    }

    void OnMouseExit () {
        // ResetColor();
    }

    void OnMouseDown () {
        Debug.Log(transform.name);
    }

    void ChangeColor (Color newColor, Color neighborColor) {
        sr.color = newColor;
        neighbors.ForEach(o => o.transform.GetComponent<SpriteRenderer>().color = neighborColor);
    }

    void ResetColor () {
        sr.color = primaryColor;
        neighbors.ForEach(o => o.transform.GetComponent<SpriteRenderer>().color = o.GetComponent<Hex>().primaryColor);
    }

    List<GameObject> GetNeighbors () {
        LayerMask hexLayer = LayerMask.NameToLayer("Hex");
        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(2f, 2f), 0f,
            layerMask: hexLayer);
        List<GameObject> overlappingObjects = new List<GameObject>();
        for(int i = 0; i < overlappingColliders.Length; ++i) {
            GameObject obj = overlappingColliders[i].gameObject;
            // Don't add self to list
            if(gameObject != obj) overlappingObjects.Add(obj);
        }
        return overlappingObjects;
    }
}
