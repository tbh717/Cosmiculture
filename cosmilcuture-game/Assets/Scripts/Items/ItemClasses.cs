using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Item Colors
public abstract class ItemColor {
    public abstract override int GetHashCode();
    public abstract override bool Equals(object o);
    public abstract override string ToString();
    public abstract Color Color { get; }
}
public class PurpleItemColor : ItemColor {
    public override int GetHashCode() { return 1; }
    public override bool Equals(object o) {
        if(o is ItemColor) { return ((ItemColor) o is PurpleItemColor); }
        else return false;
    }
    public override string ToString() { return "purple"; }
    public override Color Color { get { return new Color32(185,120,230,255); }}
}
public class YellowItemColor : ItemColor {
    public override int GetHashCode() { return 2; }
    public override bool Equals(object o) {
        if(o is ItemColor) { return ((ItemColor) o is YellowItemColor); }
        else return false;
    }
    public override string ToString() { return "yellow"; }
    public override Color Color { get { return new Color32(255,240,100,255); }}
}
public class RedItemColor : ItemColor {
    public override int GetHashCode() { return 3; }
    public override bool Equals(object o) {
        if(o is ItemColor) { return ((ItemColor) o is RedItemColor); }
        else return false;
    }
    public override string ToString() { return "red"; }
    public override Color Color { get { return new Color32(255,110,130,255); }}
}

// Item Info
public class ItemInfo {
    private string itemName;
    private string itemDescrip;
    private string scoreDescrip;
    private string itemType;
    private string itemColor;

    public string ItemName {
        get {
            return itemName;
        }
        set {
            itemName = value;
        }
    }
    public string ItemType {
        get {
            return itemType;
        }
        set {
            itemType = value;
        }
    }
    public string ItemColor {
        get {
            return itemColor;
        }
        set {
            itemColor = value;
        }
    }
    public string ItemDescription {
        get {
            return itemDescrip;
        }
        set {
            itemDescrip = value;
        }
    }
    public string ScoreDescription {
        get {
            return scoreDescrip;
        }
        set {
            scoreDescrip = value;
        }
    }

    public ItemInfo(string n, string t, string c, string id, string sd) {
        itemName = n;
        itemType = t;
        itemColor = c;
        itemDescrip = id;
        scoreDescrip = sd;
    }
}

// Item
public abstract class Item : System.ICloneable {
    public delegate void RelayMessage(Item item, string message);

    public object Clone() {
        return this.MemberwiseClone();
    }

    // The name of the tree
    public abstract ItemInfo Info {
        get;
    }
    public abstract Tile Tile {
        get;
        set;
    }
    public abstract Sprite Sprite {
        get;
        set;
    }
    public abstract AudioClip Audio {
        get;
        set;
    }
    public abstract int BaseScore {
        get;
        set;
    }

    public virtual int Score() { return TileScore() + ItemScore(); }
    public abstract int TileScore();
    public abstract int ItemScore();

    // Invoked upon next turn
    public abstract void Increment();
}

// Item properties
interface Colored {
    ItemColor ItemColor {
        get;
        set;
    }
    int ColorScore();
}

interface Timed {
    int Turns {
        get;
        set;
    }
}