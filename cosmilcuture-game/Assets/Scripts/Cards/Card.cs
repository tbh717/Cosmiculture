using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    private ItemComponent ic;

    private Text cardName;
    private Image cardImage;
    private Text itemDescrip;
    private Text scoreDescrip;
    private Text cardType;
    private Image cardTypePanel;

    private Color defaultColor;
    private Color selectedColor = new Color32(175, 255, 175, 255);

    private Button button;
    
    // Colors for "type boxes" displayed at bottom of card
    static Color floraColor = new Color32(255, 140, 80, 255);
    static Color cropColor = new Color32(75, 175, 80, 255);
    static Color decorColor = new Color32(160, 140, 255, 255);

    // Properties pertaining to selection on the CardSelect screen
    private bool canSelect;
    private bool isSelected;

    public bool CanSelect {
        get { return canSelect; }
        set { canSelect = value; }
    }
    public bool IsSelected {
        get { return isSelected; }
        set { isSelected = value; }
    }

	// Use this for initialization
	void Start() {
        // ItemComponent corresponds to Card prefab, so this assumes that it is there
        ic = GetComponent<ItemComponent>();

        // Fetches each child, as part of prefab
        foreach(Transform child in transform) {
            GameObject go = child.gameObject;
            if(go.name == "CardName") cardName = go.GetComponent<Text>();
            if(go.name == "Image") cardImage = go.GetComponent<Image>();
            if(go.name == "DescriptionPanel") {
                foreach(Transform descriptionChild in child) {
                    GameObject dgo = descriptionChild.gameObject;
                    if(dgo.name == "ItemDescription") itemDescrip = dgo.GetComponent<Text>();
                    if(dgo.name == "ScoreDescription") scoreDescrip = dgo.GetComponent<Text>();
                }
            }
            if(go.name == "CardTypePanel") {
                cardTypePanel = go.GetComponent<Image>();
                cardType = go.GetComponentInChildren<Text>();
            }
        }

        isSelected = false;
        canSelect = true;

        defaultColor = GetComponent<Image>().color;

        // Link to attached button
        button = GetComponent<Button>();
        button.onClick.AddListener(Select);
	}

    // Pulls from itemObject to update card info and appearance
    public void UpdateCard() {
        Item i = ic.Item;

        string itemType = i.TypeName;

        cardName.text = i.Info.ItemName;
        cardImage.sprite = i.Sprite;
        itemDescrip.text = i.Info.ItemDescription;
        scoreDescrip.text = i.Info.ScoreDescription;

        if(i is Colored) {
            Colored c = i as Colored;
            cardImage.color = c.ItemColor.Color;
            cardType.text = itemType + " (" + c.ItemColor.ToString() + ")";
        }
        else {
            cardImage.color = Color.white;
            cardType.text = itemType;
        }
        
        switch(itemType.ToLower()) {
            case "flora":
                cardTypePanel.color = floraColor;
                break;
            case "crop":
                cardTypePanel.color = cropColor;
                break;
            case "decor":
                cardTypePanel.color = decorColor;
                break;
        }
    }

    // Called when card button is clicked
    // Color is handled by button component
    public void Select() {
        if(canSelect) isSelected = !isSelected;
        if(isSelected) GetComponent<Image>().color = selectedColor;
        else GetComponent<Image>().color = defaultColor;
    }
}
