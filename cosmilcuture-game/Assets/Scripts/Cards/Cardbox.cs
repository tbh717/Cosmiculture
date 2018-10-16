using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* These items are the squares at the bottom of the screen that the player selects items from during gameplay */
/* No corresponding Button component because hover functionaltiy is important */

public class Cardbox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    // This is the particular card (child of the selectedDeckCards object) that we activate upon hovering
    GameObject correspondingCard;

    InteractionDirector id;

    // Aesthetic information
    Image boxImage;
    Color defaultColor;
    Color selectedColor;
    Color hoverColor;

    public Item item;
    public Image itemImage;

    bool isSelected;

    public delegate void CardboxSelected(Cardbox cardbox);
    public event CardboxSelected cardboxSelected;
    public delegate void CardboxDeselected();
    public event CardboxDeselected cardboxDeselected;

    public GameObject CorrespondingCard {
        get { return correspondingCard; }
        // This might not be called - but exists if needed
        set { 
            correspondingCard = value; 
            itemImage = transform.GetChild(0).GetComponent<Image>();
            item = (Item) correspondingCard.GetComponent<ItemComponent>().Item;
            itemImage.sprite = item.Sprite;
            if(item is Colored) itemImage.color = (item as Colored).ItemColor.Color;
        }
    }

    public bool IsSelected {
        get { return isSelected; }
        set { isSelected = value; }
    }

	// Use this for initialization
	void Start () {
        id = GameObject.Find("Director").GetComponent<InteractionDirector>();

        boxImage = GetComponent<Image>();

        defaultColor = boxImage.color;
        selectedColor = Color.green;
        selectedColor.a = defaultColor.a;
        hoverColor = selectedColor;
        // Hover color is a shade lighter than selected color
        hoverColor.a = selectedColor.a + 0.1f;

        cardboxSelected += id.CardboxSelected;
        cardboxDeselected += id.CardboxDeselected;
	}

    public void OnPointerClick(PointerEventData pointerEventData) {
        isSelected = !isSelected;
        if(isSelected) {
            cardboxSelected(this);
            boxImage.color = selectedColor;
        }
        else {
            cardboxDeselected();
            boxImage.color = hoverColor;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        if(!isSelected) {
            boxImage.color = hoverColor;
            correspondingCard.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        if(!isSelected) {
            boxImage.color = defaultColor;
            correspondingCard.SetActive(false);
        }
    }

    public void CardPlaced() {
        Destroy(correspondingCard);
        Destroy(gameObject);
    }
}
