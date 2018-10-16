using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameStates;

/* This script facilitates changes in the game board, and relays them to the game director when necessary */

public class BoardDirector : MonoBehaviour {

    // Objects containing tiles & items
    private GameObject tilesObj;
    // Tiles and item objects themselves
    public List<GameObject> tiles;
    private List<GameObject> items;

    private GameObject tooltips;
    private GameObject tooltipPrefab;

    private int addedScore;
    public delegate void UpdateScore(int newScore);
    public event UpdateScore updateScore;

    bool isTile(GameObject go) {
         return go.name.Substring(0,4) == "Tile";
    }

	public void Start () {
        tilesObj = GameObject.Find("Tiles");

        addedScore = 0;

        // Get and set tiles
        tiles = GetTiles();
        SetTileTypes();

        tooltips = GameObject.Find("Tooltips");
        tooltipPrefab = (GameObject) Resources.Load("Prefabs/UI/Tooltip");

        // Assumes directors are on same GameObject
        updateScore += GetComponent<GameDirector>().UpdateScore;

        // Adds tooltip to each tile
        SpawnTooltips();
        // Updates tooltips to correspond to tile types
        StartCoroutine(UpdateTooltips());
	}
    
    public void ChangeState(GameState newState) {
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

    // Randomly assigns tile type for each tile in scene
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

    // Spawns tooltips at start of game for each tile
    private void SpawnTooltips() {
        foreach(GameObject tile in tiles) {
            GameObject tooltip = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
            tooltip.transform.SetParent(tooltips.transform);
            tooltip.name = tile.name + " tooltip";
            tooltip.SetActive(false);

            tile.GetComponent<Tile>().tooltip = tooltip;
        }
    }

    // Updates all tooltips with relevant info
    private IEnumerator UpdateTooltips() {
        // Waits for tooltips to spawn/item info to change
        yield return new WaitForEndOfFrame();
        foreach(GameObject tile in tiles) tile.GetComponent<Tile>().UpdateTooltip();
    }

    // Handles score changes and pushes them up to game director
    public void ScoreChange() {
        int score = addedScore;
        foreach(GameObject tile in tiles) score += tile.GetComponent<Tile>().GetScore();
        updateScore(score);
    }
    public void AddScore(int addScore) {
        addedScore += addScore;
        ScoreChange();
    }
}
