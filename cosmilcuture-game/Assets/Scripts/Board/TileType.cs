using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileType {
    public abstract Sprite Sprite {
        get;
    }
    public abstract string TileName {
        get;
    }
    public abstract string TileText {
        get;
    }
}

public class DirtTile : TileType {
    private Sprite sprite = Resources.Load<Sprite>("Sprites/Tiles/DirtTileSprite");
    private string tileText = "A fertile field of dirt.";
    private string tileName = "Dirt";


    public override Sprite Sprite {
        get { return sprite; }
    }
    public override string TileName {
        get { return tileName; }
    }
    public override string TileText {
        get { return tileText; }
    }
}

public class SandTile : TileType {
    private Sprite sprite = Resources.Load<Sprite>("Sprites/Tiles/SandTileSprite");
    private string tileText = "It's coarse, and rough, and irritating, and it gets everywhere.";
    private string tileName = "Sand";


    public override Sprite Sprite {
        get { return sprite; }
    }
    public override string TileName {
        get { return tileName; }
    }
    public override string TileText {
        get { return tileText; }
    }
}

public class GrassTile : TileType {
    private Sprite sprite = Resources.Load<Sprite>("Sprites/Tiles/GrassTileSprite");
    private string tileText = "Crisp, green grass sparkling with dew.";
    private string tileName = "Grass";


    public override Sprite Sprite {
        get { return sprite; }
    }
    public override string TileName {
        get { return tileName; }
    }
    public override string TileText {
        get { return tileText; }
    }
}

public class WaterTile : TileType {
    private Sprite sprite = Resources.Load<Sprite>("Sprites/Tiles/WaterTileSprite");
    private string tileText = "Soft, rolling waves crest with the wind into sparkling white foam.";
    private string tileName = "Water";


    public override Sprite Sprite {
        get { return sprite; }
    }
    public override string TileName {
        get { return tileName; }
    }
    public override string TileText {
        get { return tileText; }
    }
}