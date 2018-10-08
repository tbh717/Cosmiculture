using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour {

    private Item item = null;

    public Item Item {
        get {
            return item;
        }
        set {
            item = value;
        }
    }

}
