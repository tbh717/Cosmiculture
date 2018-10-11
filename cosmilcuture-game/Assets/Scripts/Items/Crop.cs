using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Crop : Item, Colored, Timed {

    public abstract int Turns {
        get;
        set;
    }
    public abstract ItemColor ItemColor {
        get;
        set;
    }
    public abstract int GrowTime {
        get;
        set;
    }
    public int TimeToGrow {
        get { return Mathf.Max(0, GrowTime - Turns); }
    }
    public abstract int BaseHarvestScore {
        get;
    }
    public bool Harvestable {
        get { return (Turns >= GrowTime); }
    }
    public int HarvestScore {
        get { return ColorScore() + TileScore() + BaseHarvestScore; }
    }
    public int Harvest() {
        GrowTime = Turns+GrowTime;
        return HarvestScore;
    }
    public override void Increment() {
        Turns++;
        Tile.UpdateHarvestText(TimeToGrow);
    }

    public override int TileScore() {
        if(Tile.tileType is DirtTile) return 2;
        else return 0;
    }
    public int ColorScore() {
        int score = 0;
        foreach(Tile neighbor in Tile.neighbors) {
            if(neighbor.Item is Colored) {
                if(ItemColor == (neighbor.Item as Colored).ItemColor) score +=2;
            }
        }
        return score;
    }
    public new int Score() { return ItemScore() + TileScore() + ColorScore(); }
    
    public override string TypeName { get { return "Crop"; }}
}

public class Mushroom : Crop {

    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;
    private ItemColor itemColor;
    private int turns;
    private int growTime = 5;

    // Scoring
    private int pointsPerTurn = 2;
    private int baseScore = 0;
    private int baseHarvestScore = 6;

    public Mushroom(ItemColor ic) {
        itemColor = ic;
        turns = 0;

        sprite = Resources.Load<Sprite>("Sprites/Items/Mushroom");
        audio = Resources.Load<AudioClip>("Sounds/Items/Mushroom");
        
        string name = "Mushroom";
        string itemDescrip =
            "Bulbous heads, gritty to the touch, and sparkling " + ic.ToString() + " iridescent. Almost as alien as the fungi on Earth.";
        string scoreDescrip =
            baseHarvestScore + " Harmony on harvest. " + pointsPerTurn + " Harmony per turn on the board.";
        info = new ItemInfo(name, ic.ToString(), itemDescrip, scoreDescrip);
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
    public override int BaseHarvestScore {
        get { return baseHarvestScore; }
    }
    public override int Turns {
        get { return turns; }
        set { turns = value; }
    }
    public override ItemColor ItemColor {
        get { return itemColor; }
        set { itemColor = value; }
    }
    public override int GrowTime {
        get { return growTime; }
        set { growTime = value; }
    }

    // Called if Item still exists at end of game
    public override int ItemScore() {
        return pointsPerTurn * turns;
    }
}

public class Fruit : Crop {

    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;
    private ItemColor itemColor;
    private int turns;
    private int growTime = 5;

    // Scoring
    private float pointsPerTurn = 1.5f;
    private int baseScore = 0;
    private int baseHarvestScore = 4;  

    public Fruit(ItemColor ic) {
        itemColor = ic;
        turns = 0;

        sprite = Resources.Load<Sprite>("Sprites/Items/Fruit");
        audio = Resources.Load<AudioClip>("Sounds/Items/Fruit");
        
        string name = "Fruit";
        string itemDescrip =
            "Glistening " + ic.ToString() + " lumpy fruits. Many in number, they weigh heavy on outstretched fractal branches.";
        string scoreDescrip =
            baseHarvestScore + " Harmony on harvest. " + pointsPerTurn + " Harmony per turn on the board.";
        info = new ItemInfo(name, ic.ToString(), itemDescrip, scoreDescrip);
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
    public override int BaseHarvestScore {
        get { return baseHarvestScore; }
    }
    public override int Turns {
        get { return turns; }
        set { turns = value; }
    }
    public override ItemColor ItemColor {
        get { return itemColor; }
        set { itemColor = value; }
    }
    public override int GrowTime {
        get { return growTime; }
        set { growTime = value; }
    }

    // Called if Item still exists at end of game
    public override int ItemScore() {
        return Mathf.FloorToInt(pointsPerTurn * turns);
    }
}

public class Vegetable : Crop {

    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;
    private ItemColor itemColor;
    private int turns;
    private int growTime = 3;

    // Scoring
    private float pointsPerTurn = 0.5f;
    private int baseScore = 0;
    private int baseHarvestScore = 3;

    public Vegetable(ItemColor ic) {
        itemColor = ic;
        turns = 0;

        sprite = Resources.Load<Sprite>("Sprites/Items/Vegetable");
        audio = Resources.Load<AudioClip>("Sounds/Items/Vegetable");
        
        string name = "Vegetable";
        string itemDescrip =
            "Rooted vegetables, displaying a hearty " + ic.ToString() + " skin. Great in spacey soups.";
        string scoreDescrip =
            baseScore + " Harmony on harvest." + pointsPerTurn + " Harmony per turn spent on the board.";
        info = new ItemInfo(name, ic.ToString(), itemDescrip, scoreDescrip);
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
    public override int BaseHarvestScore {
        get { return baseHarvestScore; }
    }
    public override int Turns {
        get { return turns; }
        set { turns = value; }
    }
    public override ItemColor ItemColor {
        get { return itemColor; }
        set { itemColor = value; }
    }
    public override int GrowTime {
        get { return growTime; }
        set { growTime = value; }
    }

    // Called if Item still exists at end of game
    public override int ItemScore() {
        return Mathf.FloorToInt(turns * pointsPerTurn);
    }
}