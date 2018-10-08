using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Flora : Item, Colored {
    public override int TileScore() {
        if(Tile.tileType is GrassTile) return 2;
        else return 0;
    }
    public int ColorScore() {
        int score = 0;
        foreach(Tile neighbor in Tile.neighbors) {
            if(neighbor.Item is Colored) if(ItemColor.Equals((neighbor.Item as Colored).ItemColor)) score += 2;
        }
        return score;
    }
    public override int Score() { 
        return ItemScore() + TileScore() + ColorScore();
    }
    
    public abstract ItemColor ItemColor {
        get;
        set;
    }
}

public class Magnolium : Flora, Timed {

    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;
    private ItemColor itemColor;
    private int turns;

    // Scoring
    private int pointsPerTurn = 1;
    private int baseScore = 2;

    // Aging
    private int midAge = 3;
    private int oldAge = 8;

    public Magnolium(ItemColor ic) {
        itemColor = ic;
        turns = 0;

        sprite = Resources.Load<Sprite>("Sprites/Items/Magnolium");
        audio = Resources.Load<AudioClip>("Sounds/Items/Magnolium");
        
        string name = "Magnolium Tree";
        string itemDescripYoung =
            "A weak sapling, reaching out for life from the planet's rich humus.";
        string scoreDescrip =
            baseScore + " Harmony plus " + pointsPerTurn + " Harmony per turn spent on the board.";
        info = new ItemInfo(name, "Flora", ic.ToString(), itemDescripYoung, scoreDescrip);
    }

    // Method overrides
    public override ItemInfo Info {
        get { return info; }
    }
    public override Tile Tile {
        get { return tile; }
        set { tile = value; }
    }
    public override Sprite Sprite {
        get { return sprite; }
        set { sprite = value; }
    }
    public override AudioClip Audio {
        get { return audio; }
        set { audio = value; }
    }
    public override int BaseScore {
        get { return baseScore; }
        set { baseScore = value; }
    }
    public int Turns {
        get { return turns; }
        set { turns = value; }
    }
    public override ItemColor ItemColor {
        get { return itemColor; }
        set { itemColor = value; }
    }

    public override int ItemScore() {
        return baseScore + (pointsPerTurn*turns);
    }

    public override void Increment() {
        turns++;
        // Medium aged tree
        if(turns == midAge) {
            info.ItemDescription =
                "A proud tree, standing tall with a sturdy copper trunk and a lush smattering of " + itemColor.ToString() + " leaves.";
            baseScore = 6;
            info.ScoreDescription = baseScore + " Harmony plus " + pointsPerTurn + " Harmony per turn spent on the board.";
            tile.UpdateTooltip();
        }
        else if(turns == oldAge) {
            info.ItemDescription =
                "A beautiful, aged behemoth, towering hundreds of Distance Units above your makeshift farm. A marvelous sight to behold.";
            baseScore = 12;
            info.ScoreDescription = baseScore + " Harmony plus " + pointsPerTurn + " Harmony per turn spent on the board.";
            tile.UpdateTooltip();
        }
    }
}

public class Glowlilies : Flora {
    // Defualt methods
    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;
    private int baseScore = 4;
    private ItemColor itemColor;

    private int scoreInWater = 3;
    private int scoreAdjacentWater = 1;

    // Method overrides
    public Glowlilies(ItemColor ic) {
        itemColor = ic;

        sprite = Resources.Load<Sprite>("Sprites/Items/Glowlilies");
        audio = Resources.Load<AudioClip>("Sounds/Items/Glowlilies");
        
        string name = "Glowlilies";
        string itemDescrip =
            "A menagerie of dainty floral specimins, flopping out with brilliant white leaves, speckled with " + itemColor.ToString() + ".";
        string scoreDescrip =
            baseScore + " Harmony, plus " + scoreInWater + " if placed in a water tile, " + scoreAdjacentWater + " for each adjacent water tile.";
        info = new ItemInfo(name, "Flora", ic.ToString(), itemDescrip, scoreDescrip);
    }
    public override ItemInfo Info {
        get { return info; }
    }
    public override Tile Tile {
        get { return tile; }
        set { tile = value; }
    }
    public override Sprite Sprite {
        get { return sprite; }
        set { sprite = value; }
    }
    public override AudioClip Audio {
        get { return audio; }
        set { audio = value; }
    }
    public override int BaseScore {
        get { return baseScore; }
        set { baseScore = value; }
    }
    public override ItemColor ItemColor {
        get { return itemColor; }
        set { itemColor = value; }
    }

    public override int ItemScore() {
        int score = baseScore;
        if(tile.tileType is WaterTile) score += scoreInWater;
        foreach(Tile neighbor in tile.neighbors) if(neighbor.tileType is WaterTile) score += scoreAdjacentWater;
        return score;
    }

    public override void Increment() {
        // Do nothing
    }
}

public class Gargantua : Flora {
    // Defualt methods
    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;
    private int baseScore = 10;
    private ItemColor itemColor;

    private int scoreAdjacentFlora = 2;

    public Gargantua(ItemColor ic) {
        itemColor = ic;

        sprite = Resources.Load<Sprite>("Sprites/Items/Gargantua");
        audio = Resources.Load<AudioClip>("Sounds/Items/Gargantua");

        string name = "Gargantua";
        string itemDescrip =
            "A massive flower, spreading over 5 Distance Units across. Floppy " + itemColor.ToString() + " leaves seem to breathe independent of the breeze. Or is that just an illusion?";
        string scoreDescrip =
            baseScore + " Harmony, minus " + scoreAdjacentFlora + " for each adjacent flora.";
        info = new ItemInfo(name, "Flora", ic.ToString(), itemDescrip, scoreDescrip);
    }

    // Method overrides
    public override ItemInfo Info {
        get { return info; }
    }
    public override Tile Tile {
        get { return tile; }
        set { tile = value; }
    }
    public override Sprite Sprite {
        get { return sprite; }
        set { sprite = value; }
    }
    public override AudioClip Audio {
        get { return audio; }
        set { audio = value; }
    }
    public override int BaseScore {
        get { return baseScore; }
        set { baseScore = value; }
    }
    public override ItemColor ItemColor {
        get { return itemColor; }
        set { itemColor = value; }
    }

    public override int ItemScore() {
        int score = baseScore;
        // For each flora neighbor, deduct a point
        foreach(Tile neighbor in tile.neighbors) if(neighbor.Item is Flora) score -= scoreAdjacentFlora;
        return score;
    }

    public override void Increment() {
        // Do nothing
    }
}

public class Flytrap : Flora {
    // Defualt methods
    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;
    private int baseScore = 10;
    private ItemColor itemColor;

    // 1/chanceDenom chance of eating adjacent crop
    private int chanceDenom = 3;

    public Flytrap(ItemColor ic) {
        itemColor = ic;

        sprite = Resources.Load<Sprite>("Sprites/Items/Flytrap");
        audio = Resources.Load<AudioClip>("Sounds/Items/Flytrap");

        string name = "Giant Saturn Flytrap";
        string itemDescrip =
            "Big ol flytrap.";
        string scoreDescrip =
            baseScore + " Harmony. Each turn there is a one in " + chanceDenom + " chance that the flytrap will consume an adjacent crop plant.";
        info = new ItemInfo(name, "Flora", ic.ToString(), itemDescrip, scoreDescrip);
    }

    // Method overrides
    public override ItemInfo Info {
        get { return info; }
    }
    public override Tile Tile {
        get { return tile; }
        set { tile = value; }
    }
    public override Sprite Sprite {
        get {
            return sprite;
        }
        set { sprite = value; }
    }
    public override AudioClip Audio {
        get { return audio; }
        set { audio = value; }
    }
    public override int BaseScore {
        get { return baseScore; }
        set { baseScore = value; }
    }
    public override ItemColor ItemColor {
        get { return itemColor; }
        set { itemColor = value; }
    }

    // 10 harmony
    public override int ItemScore() {
        return baseScore;
    }

    // 1/3 chance of consuming adjacent food item
    public override void Increment() {
        int r = Random.Range(0,chanceDenom);
        if(r == 0) {
            foreach(Tile neighbor in tile.neighbors) {
                if(neighbor.Item != null) {
                    if(neighbor.Item is Crop) neighbor.RemoveItem();
                    break;
                }
            }
        }
    }
}

public class Shrub : Flora {
    // Defualt methods
    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;
    private int baseScore = 10;
    private ItemColor itemColor;
    
    private int chanceDenom = 1;

    public Shrub(ItemColor ic) {
        itemColor = ic;

        sprite = Resources.Load<Sprite>("Sprites/Items/Shrub");
        audio = Resources.Load<AudioClip>("Sounds/Items/Shrub");

        string name = "SuperShrub";
        string itemDescrip =
            "Big ol shrub";
        string scoreDescrip =
            baseScore + " Harmony.";
        info = new ItemInfo(name, "Flora", ic.ToString(), itemDescrip, scoreDescrip);
    }

    // Method overrides
    public override ItemInfo Info {
        get { return info; }
    }
    public override Tile Tile {
        get { return tile; }
        set { tile = value; }
    }
    public override Sprite Sprite {
        get { return sprite; }
        set { sprite = value; }
    }
    public override AudioClip Audio {
        get { return audio; }
        set { audio = value; }
    }
    public override int BaseScore {
        get { return baseScore; }
        set { baseScore = value; }
    }
    public override ItemColor ItemColor {
        get { return itemColor; }
        set { itemColor = value; }
    }

    // 10 harmony
    public override int ItemScore() {
        return baseScore;
    }

    // 1/4 chance of spreading to adjacent non-water tile
    public override void Increment() {
        int r = Random.Range(0,chanceDenom);
        if(r == 0) {
            Debug.Log("getting this shrub");
            List<Tile> validNeighbors = new List<Tile>();
            foreach(Tile neighbor in tile.neighbors) {
                Debug.Log("Checking on neighboring tile " + neighbor.name + ", " + neighbor.tileType + " from " + tile.name);
                if(!(neighbor.tileType is WaterTile)) validNeighbors.Add(neighbor);
            }
            if(validNeighbors.Count > 1) {
                Debug.Log("Valid neighbors for " + tile.gameObject.name + ": " + validNeighbors.Count);
                int rn = Random.Range(0,validNeighbors.Count-1);
                if(!(validNeighbors[rn].Item is Shrub)) validNeighbors[rn].ChangeItem(new Shrub(itemColor));
            }
        }
    }
}