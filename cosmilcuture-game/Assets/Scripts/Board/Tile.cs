using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

    private GameObject gameBoard;

    private SpriteRenderer sr;
    private Color defaultColor;

    public List<Tile> neighbors;

    public TileType tileType;
    
    public GameObject itemsObj;
    public GameObject itemPrefab;
    public GameObject boardItem;

    public GameObject tooltip;
    GameObject tooltipTextPrefab;
    int tooltipFontSize;
    Text scoreText;
    Text harvestText;

    Color hoverColor;

    public bool tileEnabled;

    GameObject director;
    BoardDirector bd;
    InteractionDirector id;
    AudioDirector ad;

    // Interaction
    public delegate void TileUpdate(Tile tile);
    public event TileUpdate onTileFocus;
    public event TileUpdate onTileUnfocus;
    public event TileUpdate onTileClick;

    public delegate void TileItemUpdate(Item item, Tile tile);
    public event TileItemUpdate itemPlaced;
    public event TileItemUpdate itemRemoved;

    // Announced whenever new item is added, item is changed, or item evolves
    public delegate void ScoreChange();
    public event ScoreChange scoreChange;
    // Adds permanent score (from activities like harvesting) to score board
    public delegate void AddScore(int addScore);
    public event AddScore scoreAdd;

    public delegate void CropHarvested();
    public event CropHarvested cropHarvested;

    // Return item from active boardItem GameObject
    public Item Item {
        get {
            if(boardItem != null) return boardItem.GetComponent<ItemComponent>().Item;
            else return null;
        }
    }

	void Start() {
        // Assumes that tiles are children of board object
        gameBoard = transform.parent.gameObject;

        sr = transform.GetComponent<SpriteRenderer>();
		defaultColor = sr.color;
        neighbors = AssignNeighbors();

        tooltipTextPrefab = (GameObject) Resources.Load("Prefabs/UI/TooltipText");
        tooltipFontSize = 24;

        itemPrefab = Resources.Load<GameObject>("Prefabs/Board/BoardItem");
        boardItem = null;
        itemsObj = GameObject.Find("Items");

        hoverColor = new Color(0.5f,0.5f,0.5f);

        director = GameObject.Find("Director");
        id = director.GetComponent<InteractionDirector>();
        bd = director.GetComponent<BoardDirector>();
        ad = GameObject.Find("AudioController").GetComponent<AudioDirector>();

        onTileUnfocus += ad.UnfocusTileAudio;
        onTileFocus += ad.FocusTileAudio;
        onTileClick += id.OnTileClick;
        scoreChange += bd.ScoreChange;
        scoreAdd += bd.AddScore;

        itemPlaced += ad.AddItemAudio;
        itemRemoved += ad.RemoveItemAudio;

        cropHarvested += ad.HarvestedCrop;

        // Disables a tile upon instantiation (because game begins in CardSelect screen)
        Disable();
	}

    // Disables interaction with tile, usually called upon leaving Gameplay state
    public void Disable() { 
        tileEnabled = false;
        // Unfocuses tile in the event that the mouse was hovering over tile while turn ended
        onTileUnfocus(this);
    }
    // Enables interaction with tile, usually called upon entering Gameplay state
    public void Enable() { tileEnabled = true; } 
	
    // Gathers neighbors using colliders, because tiles are not UI elements
    // Make sure to test neighbors upon changes of game scale, in the event that colliders grow too large or small relative to other tiles
	private List<Tile> AssignNeighbors() {
        LayerMask hexLayer = LayerMask.NameToLayer("Hex");
        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(2f, 2f), 0f,
            layerMask: hexLayer);
        List<Tile> overlappingObjects = new List<Tile>();
        for(int i = 0; i < overlappingColliders.Length; ++i) {
            GameObject obj = overlappingColliders[i].gameObject;
            // Don't add self to list
            if(gameObject != obj) {
                Tile posTile = obj.GetComponent<Tile>();
                if(posTile != null) overlappingObjects.Add(posTile);
            }
        }
        return overlappingObjects;
    }

    /* MOUSE INTERACTION */

    // Display tooltip when mouse enters tile
    void OnMouseEnter() {
        if(tileEnabled) {
            onTileFocus(this);
            DarkenColor();
            tooltip.SetActive(true);
        }
    }

    // Track mouse position and display tooltip there while mouse is over tile
    void OnMouseOver() {
        if(tileEnabled) {
            Vector2 ttSize = tooltip.GetComponent<RectTransform>().sizeDelta;
            Vector2 mousePos = Input.mousePosition;
            Vector2 newPos;

            // Check if mouse is low on screen
            Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);
            float newX;
            float newY;
            if(mousePosWorld.x > gameBoard.transform.position.x) newX = ttSize.x/2; 
            else newX = ttSize.x/-2;
            if(mousePosWorld.y > gameBoard.transform.position.y) newY = ttSize.y/-2; 
            else newY = ttSize.y/2;
            newPos = mousePos + new Vector2(newX, newY);

            tooltip.transform.position = newPos;
        }
    }

    // Remove tooltip when mouse enters tile
    void OnMouseExit() {
        onTileUnfocus(this);
        ResetColor();
        tooltip.SetActive(false);
    }

    // Announce that tile has been clicked for the InteractionManager to deal with
    void OnMouseDown() {
        onTileClick(this);
    }

    // Change tile type and sprite
    public void SetTileType(TileType tt) {
        tileType = tt;
        transform.GetComponent<SpriteRenderer>().sprite = tileType.Sprite;
    }

    // Darken on hover
    public void DarkenColor() {
        sr.color = hoverColor;
    }

    // Reset to default color after hovering
    public void ResetColor() {
        sr.color = defaultColor;
    }

    // Add item to tile
    // Returns true if successful, false otherwise
    public bool AddItem(Item newItem) {
        if(boardItem == null) {
            GameObject bi = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            boardItem = bi;

            bi.transform.SetParent(itemsObj.transform, true);
            bi.transform.position = new Vector3(bi.transform.position.x, bi.transform.position.y, itemsObj.transform.position.z);

            bi.GetComponent<ItemComponent>().Item = newItem;
            bi.GetComponent<ItemComponent>().Item.Tile = this;
            bi.GetComponent<SpriteRenderer>().sprite = newItem.Sprite;
            if(newItem is Colored) bi.GetComponent<SpriteRenderer>().color = (newItem as Colored).ItemColor.Color;

            UpdateTooltip();
            scoreChange();

            itemPlaced(newItem, this);

            return true;
        }
        else return false;
    }

    // Returns true if item was successfully removed, false otherwise
    public bool RemoveItem() {
        if(boardItem != null) {
            itemRemoved(boardItem.GetComponent<ItemComponent>().Item, this);
            Destroy(boardItem);
            boardItem = null;
            return true;
        }
        else return false;
    }

    // Removes item then adds new one
    // This is the default interaction, so that items can be changed to any other held item
    public bool ChangeItem(Item newItem) {
        RemoveItem();
        return AddItem(newItem);
    }

    // Adds a vertical block of text to the tile's tooltip
    public Text AddTooltipText(string t, float fs) {
        GameObject tileDescrip = Instantiate(tooltipTextPrefab, transform.position, Quaternion.identity);
        tileDescrip.transform.SetParent(tooltip.transform);
        Text text = tileDescrip.GetComponent<Text>();
        text.text = t;
        text.fontSize = Mathf.RoundToInt(fs);
        return text;
    }

    // Recreates tooltip from scratch
    // Simpler than checking for any changes
    // Comprehensive method for tile initilization w/ tile type, adding item, changing item, etc.
    public void UpdateTooltip() {
        // Delete all text items in tooltip
        foreach(Transform tooltipText in tooltip.transform) {
            Destroy(tooltipText.gameObject);
        }

        /* SPAWN NEW TEXT ITEMS */
        // Tile description
        AddTooltipText(tileType.TileText, tooltipFontSize);
        AddTooltipText("------", tooltipFontSize);
        if(boardItem != null) {
            ItemInfo itemInfo = boardItem.GetComponent<ItemComponent>().Item.Info;
            Text itemName = AddTooltipText(itemInfo.ItemName.ToUpper() + " (" + Item.TypeName + ")", tooltipFontSize*1.25f);
            itemName.fontStyle = FontStyle.Bold;
            if(Item is Colored) itemName.color = (Item as Colored).ItemColor.Color;
            AddTooltipText(itemInfo.ItemDescription, tooltipFontSize);
            AddTooltipText(itemInfo.ScoreDescription, tooltipFontSize);
            AddTooltipText("------", tooltipFontSize*1.25f);
            if(Item is Crop) {
                Crop crop = Item as Crop;
                harvestText = AddTooltipText("GROWS IN: " + crop.TimeToGrow.ToString(), tooltipFontSize*1.15f);
            }
        }
        scoreText = AddTooltipText("SCORETEXT", tooltipFontSize*1.25f);
        GetScore(); // also updates scoretext
    }

    private string ToolTipScoreText(int score) {
        return "<b>" + score + "</b> <i>HARMONY</i>";
    }

    public int GetScore() {
        int score;
        if(boardItem != null) score = boardItem.GetComponent<ItemComponent>().Item.Score();
        else score = 0;
        scoreText.text = ToolTipScoreText(score);
        return score;
    }

    public void Harvested() {
        if(Item is Crop) {
            Crop crop = Item as Crop;
            scoreAdd(crop.Harvest());
            UpdateTooltip();
            cropHarvested();
        }
        else Debug.Log("Harvested method called on non-crop tile.");
    }

    // Called by corresponding item
    public void UpdateHarvestText(int time) {
        if(harvestText != null) {
            harvestText.text = "GROWS IN: " + time;
        }
    }
}
