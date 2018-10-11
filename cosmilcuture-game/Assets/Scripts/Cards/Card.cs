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

    Color purpleColor = new Color32(145,80,190,255);
    Color yellowColor = new Color32(250,200,60,255);
    Color redColor = new Color32(225,70,90,255);
    
    Color floraColor = new Color32(255, 140, 80, 255);
    Color cropColor = new Color32(75, 175, 80, 255);
    Color decorColor = new Color32(160, 140, 255, 255);

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
        // Assumes that card also has itemObject placed on it
		 ic = GetComponent<ItemComponent>();

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

    // Pulls from itemObject to update card info
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

    public void Select() {
        if(canSelect) isSelected = !isSelected;
        if(isSelected) GetComponent<Image>().color = selectedColor;
        else GetComponent<Image>().color = defaultColor;
    }
}
