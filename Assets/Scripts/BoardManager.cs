using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    private List<GameObject> tiles;

    bool isTile(GameObject go) {
         return go.name.Substring(0,4) == "Tile";
    }

	// Use this for initialization
	void Start () {
        tiles = GetTiles();
        SetTileTypes();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    List<GameObject> GetTiles () {
        List<GameObject> tileList = new List<GameObject>();

        foreach(Transform child in transform) {
            GameObject go = child.gameObject;
            if(isTile(go)) tileList.Add(child.gameObject);
        }

        return tileList;
    }

    void SetTileTypes () {
        // Set tile type
        foreach(GameObject tile in tiles) {
            int typeVal = Random.Range(0,4);
            switch (typeVal) {
                case 0:
                    tile.GetComponent<Tile>().SetTileType(new Tile.DirtTile());
                    break;
                case 1:
                    tile.GetComponent<Tile>().SetTileType(new Tile.SandTile());
                    break;
                case 2:
                    tile.GetComponent<Tile>().SetTileType(new Tile.GrassTile());
                    break;
                case 3:
                    tile.GetComponent<Tile>().SetTileType(new Tile.WaterTile());
                    break;
                default:
                    Debug.Log("Random type generator returned invalid int: " + typeVal);
                    break;
            }
        }
    }
}
