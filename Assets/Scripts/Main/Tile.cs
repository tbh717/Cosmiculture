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
            return "";
        }
    }

    public class SandTile : TileType {
        public override Sprite GetSprite () {
            return Resources.Load<Sprite>("Sprites/Tiles/SandTileSprite");
        }
        public override string TileText () {
            return "";
        }
    }

    public class GrassTile : TileType {
        public override Sprite GetSprite () {
            return Resources.Load<Sprite>("Sprites/Tiles/GrassTileSprite");
        }
        public override string TileText () {
            return "";
        }
    }

    public class WaterTile : TileType {
        public override Sprite GetSprite () {
            return Resources.Load<Sprite>("Sprites/Tiles/WaterTileSprite");
        }
        public override string TileText () {
            return "";
        }
    }

	// Use this for initialization
	void Start() {
        // director = GameObject.Find("Director");
        sr = transform.GetComponent<SpriteRenderer>();
		defaultColor = sr.color;
        neighbors = AssignNeighbors();

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
