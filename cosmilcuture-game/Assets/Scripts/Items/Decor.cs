using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Decor : Item {
    public override int TileScore() {
        if(Tile.tileType is SandTile) return 3;
        else return 0;
    }
    public override int Score() { 
        return ItemScore() + TileScore();
    }
    public override string TypeName { get { return "Decor"; }}
}

public class Hive : Decor {

    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;

    private BoardDirector bd;

    // Scoring
    private int pointsPerFlora = 1;
    private int baseScore = 5;

    public Hive() {
        sprite = Resources.Load<Sprite>("Sprites/Items/Hive");
        audio = Resources.Load<AudioClip>("Sounds/Items/Hive");

        bd = GameObject.Find("Director").GetComponent<BoardDirector>();
        
        string name = "Beehive";
        string itemDescripYoung =
            "Busied winged creatures buzz about their nest. More like beatles than bees, their boast a hard white shell, harvesting minty nectar from your garden's flora.";
        string scoreDescrip =
            baseScore + " Harmony plus " + pointsPerFlora + " Harmony per Flora on the board.";
        info = new ItemInfo(name, null, itemDescripYoung, scoreDescrip);
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

    public override int ItemScore() {
        int numFlora = 0;
        foreach(GameObject tileObj in bd.tiles) {
            if(tileObj.GetComponent<Tile>().Item is Flora) numFlora++;
        }
        return baseScore + (pointsPerFlora*numFlora);
    }

    public override void Increment() {}
}

public class Obelisk : Decor {

    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;

    // Scoring
    private int pointsPerDecor = 1;
    private int baseScore = 15;

    public Obelisk() {
        sprite = Resources.Load<Sprite>("Sprites/Items/Obelisk");
        audio = Resources.Load<AudioClip>("Sounds/Items/Obelisk");
        
        string name = "Obelisk";
        string itemDescripYoung =
            "₳₦₵łɆ₦₮ ₴Ɇ₵ⱤɆ₮₴ ₣ØⱤɆVɆⱤ ₣ⱤØⱫɆ₦ ł₦ Đ₳Ɽ₭ Ø฿₴łĐł₳₦";
        string scoreDescrip =
            baseScore + " Harmony plus " + pointsPerDecor + " Harmony per adjacent Decor.";
        info = new ItemInfo(name, null, itemDescripYoung, scoreDescrip);
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

    public override int ItemScore() {
        int numDecor = 0;
        foreach(Tile neighbor in tile.neighbors) {
            if(tile.Item is Decor) numDecor++;
        }
        return baseScore + (pointsPerDecor*numDecor);
    }

    public override void Increment() {}
}

public class Tent : Decor {

    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;

    // Scoring
    private int pointsPerFlora = 2;
    private int baseScore = 9;

    public Tent() {
        sprite = Resources.Load<Sprite>("Sprites/Items/Tent");
        audio = Resources.Load<AudioClip>("Sounds/Items/Tent");
        
        string name = "Tent";
        string itemDescripYoung =
            "A cozy adventure out in the garden awaits!";
        string scoreDescrip =
            baseScore + " Harmony plus " + pointsPerFlora + " Harmony per adjacent Flora.";
        info = new ItemInfo(name, null, itemDescripYoung, scoreDescrip);
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

    public override int ItemScore() {
        int numFlora = 0;
        foreach(Tile neighbor in tile.neighbors) {
            if(tile.Item is Flora) numFlora++;
        }
        return baseScore + (pointsPerFlora*numFlora);
    }

    public override void Increment() {}
}

public class Meteorite : Decor {

    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;

    // Scoring
    private int pointsInTile = 8;
    private int baseScore = 5;

    public Meteorite() {
        sprite = Resources.Load<Sprite>("Sprites/Items/Meteorite");
        audio = Resources.Load<AudioClip>("Sounds/Items/Meteorite");
        
        string name = "Meteorite";
        string itemDescrip =
            "Halfway stuck into the earth, the meteorite proves too difficult to remove. Its gem-like pores flood the garden with brilliant golden light.";
        string scoreDescrip =
            baseScore + " Harmony plus " + pointsInTile + " Harmony if in a Sand or Water tile.";
        info = new ItemInfo(name, null, itemDescrip, scoreDescrip);
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

    public override int ItemScore() {
        if(tile.tileType is SandTile || tile.tileType is WaterTile) return baseScore + pointsInTile;
        else return baseScore;
    }

    public override void Increment() {}
}

public class Picnic : Decor {

    private ItemInfo info;
    private Tile tile;
    private Sprite sprite;
    private AudioClip audio;

    // Scoring
    private int pointsPerCrop = 3;
    private int baseScore = 5;

    public Picnic() {
        sprite = Resources.Load<Sprite>("Sprites/Items/Picnic");
        audio = Resources.Load<AudioClip>("Sounds/Items/Picnic");
        
        string name = "Picnic";
        string itemDescrip =
            "Fresh harvest from the garden sits temptingly in a bamboo picnic basket, draped with checkered red cloth. A souvenir from home.";
        string scoreDescrip =
            baseScore + " Harmony plus " + pointsPerCrop + " Harmony per neighboring Crop.";
        info = new ItemInfo(name, null, itemDescrip, scoreDescrip);
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

    public override int ItemScore() {
        int numCrop = 0;
        foreach(Tile neighbor in tile.neighbors) if(neighbor.Item is Crop) numCrop += 3;
        return baseScore + (numCrop * pointsPerCrop);
    }

    public override void Increment() {}
}