using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameStates;

public class BoardDirector : MonoBehaviour {

    private GameObject board;

    // Objects containing tiles & items
    private GameObject tilesObj;
    private GameObject itemsObj;
    // Tiles and item objects themselves
    private List<GameObject> tiles;
    private List<GameObject> items;

    private GameObject tooltips;
    private GameObject tooltipPrefab;

    private GameState gameState;

    public delegate void UpdateScore(int newScore);
    public event UpdateScore updateScore;

    bool isTile(GameObject go) {
         return go.name.Substring(0,4) == "Tile";
    }

	// Use this for initialization
	public void Start () {
        board = GameObject.Find("Board");
        tilesObj = GameObject.Find("Tiles");
        itemsObj = GameObject.Find("Items");

        tiles = GetTiles();
        SetTileTypes();

        tooltips = GameObject.Find("Tooltips");
        tooltipPrefab = (GameObject) Resources.Load("Prefabs/UI/Tooltip");

        updateScore += GetComponent<GameDirector>().UpdateScore;

        SpawnTooltips();
        StartCoroutine(UpdateTooltips());
	}
    
    public void ChangeState(GameState newState) {
        gameState = newState;
        if(newState == GameState.Gameplay) {
            foreach(GameObject tile in tiles) {
                tile.GetComponent<Tile>().Enable();
            }
        }
        else {
            if(newState == GameState.CardSelect) {
                List<Item> itemsToInc = new List<Item>();
                foreach(GameObject tile in tiles) {
                    Item item = tile.GetComponent<Tile>().Item;
                    if(item != null) itemsToInc.Add(item);
                }
                foreach(Item item in itemsToInc) item.Increment();
                ScoreChange();
            }
            foreach(GameObject tile in tiles) tile.GetComponent<Tile>().Disable();
        }
    }

    private List<GameObject> GetTiles() {
        List<GameObject> tileList = new List<GameObject>();

        foreach(Transform child in tilesObj.transform) {
            GameObject go = child.gameObject;
            if(isTile(go)) tileList.Add(child.gameObject);
        }

        return tileList;
    }

    private void SetTileTypes() {
        // Set tile type
        foreach(GameObject tile in tiles) {
            int typeVal = Random.Range(0,4);
            switch (typeVal) {
                case 0:
                    tile.GetComponent<Tile>().SetTileType(new DirtTile());
                    break;
                case 1:
                    tile.GetComponent<Tile>().SetTileType(new SandTile());
                    break;
                case 2:
                    tile.GetComponent<Tile>().SetTileType(new GrassTile());
                    break;
                case 3:
                    tile.GetComponent<Tile>().SetTileType(new WaterTile());
                    break;
                default:
                    Debug.Log("Random type generator returned invalid int: " + typeVal);
                    break;
            }
        }
    }

    private void SpawnTooltips() {
        foreach(GameObject tile in tiles) {
            GameObject tooltip = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
            tooltip.transform.SetParent(tooltips.transform);
            tooltip.name = tile.name + " tooltip";
            tooltip.SetActive(false);

            tile.GetComponent<Tile>().tooltip = tooltip;

        }
    }

    private IEnumerator UpdateTooltips() {
        yield return new WaitForEndOfFrame();
        foreach(GameObject tile in tiles) tile.GetComponent<Tile>().UpdateTooltip();
    }

    public void ScoreChange() {
        int score = 0;
        foreach(GameObject tile in tiles) score += tile.GetComponent<Tile>().GetScore();
        updateScore(score);
    }
}
