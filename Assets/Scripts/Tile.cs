using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {


    /* TILE TYPES */
    public abstract class TileType {
        public abstract Color GetColor ();
        public abstract string TileText ();
    }

    public class DirtTile : TileType {
        public override Color GetColor () {
            return Color.grey;
        }
        public override string TileText () {
            return "";
        }
    }

    public class SandTile : TileType {
        public override Color GetColor () {
            return Color.yellow;
        }
        public override string TileText () {
            return "";
        }
    }

    public class GrassTile : TileType {
        public override Color GetColor () {
            return Color.green;
        }
        public override string TileText () {
            return "";
        }
    }

    public class WaterTile : TileType {
        public override Color GetColor () {
            return Color.cyan;
        }
        public override string TileText () {
            return "";
        }
    }

    public TileType tileType;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTileType(TileType tt) {
        tileType = tt;
        transform.GetComponent<SpriteRenderer>().color = tileType.GetColor();
    }
}
