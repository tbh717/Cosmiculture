using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemColor {
        Purple,
        Yellow,
        Red
}

public abstract class Item {
    public abstract GameObject tile {
        get;
    }
    public abstract Sprite sprite {
        get;
    }

    public abstract int baseScore {
        get;
    }

    public abstract int ItemScore();
}

interface Colored {
    ItemColor GetColor();
}

public abstract class Flora : Item, Colored {
    public ItemColor itemColor;

    public ItemColor GetColor() {
        return itemColor;
    }
}

public abstract class Crop : Item, Colored {
    public ItemColor itemColor;

    public ItemColor GetColor() {
        return itemColor;
    }
}

public abstract class Decor : Item {

}
