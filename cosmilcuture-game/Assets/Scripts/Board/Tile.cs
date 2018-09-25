using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    // public GameObject director;

    private SpriteRenderer sr;
    private Color defaultColor;

    public int score;

    private List<GameObject> neighbors;

    public TileType tileType;
    public GameObject item;

    /* TILE TYPES */
    public abstract class TileType {
        public abstract Sprite GetSprite ();
        public abstract string TileText ();
    }

    public class DirtTile : TileType {
        public override Sprite GetSprite () {
            return Resources.Load<Sprite>("Sprites/Tiles/DirtTileSprite");
        }
        public override string TileText () {
            return "A fertile field of dirt.";
        }
    }

    public class SandTile : TileType {
        public override Sprite GetSprite () {
            return Resources.Load<Sprite>("Sprites/Tiles/SandTileSprite");
        }
        public override string TileText () {
            return "A smooth bed of sand, built up over thousands of years from the erosion of this planet's strange stones." 
            + "\n" +
            "It's coarse, and rough, and irritating, and it gets everywhere.";
        }
    }

    public class GrassTile : TileType {
        public override Sprite GetSprite () {
            return Resources.Load<Sprite>("Sprites/Tiles/GrassTileSprite");
        }
        public override string TileText () {
            return "Crisp, green grass sparkling with dew.";
        }
    }

    public class WaterTile : TileType {
        public override Sprite GetSprite () {
            return Resources.Load<Sprite>("Sprites/Tiles/WaterTileSprite");
        }
        public override string TileText () {
            return "Soft, rolling waves crest with the wind into sparkling white foam.";
        }
    }

	// Use this for initialization
	void Start() {
        // director = GameObject.Find("Director");
        sr = transform.GetComponent<SpriteRenderer>();
		defaultColor = sr.color;
        neighbors = AssignNeighbors();
        item = null;

        score = 0;
	}
	
	// Update is called once per frame
	void Update() {
		
	}

    void OnMouseEnter() {
        sr.color = new Color(0.5f,0.5f,0.5f);
    }

    void OnMouseExit() {
        sr.color = defaultColor;
    }

    public void SetTileType(TileType tt) {
        tileType = tt;
        transform.GetComponent<SpriteRenderer>().sprite = tileType.GetSprite();
    }

    private List<GameObject> AssignNeighbors() {
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

    public int GetScore () {
        return score;
    }
}
