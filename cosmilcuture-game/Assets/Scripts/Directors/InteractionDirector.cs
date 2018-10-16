using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameStates;

/* This director is called during Gameplay, to handle selection of cardboxes and placement of items */

public class InteractionDirector : MonoBehaviour {

    Cardbox selectedCardbox;
    float scaleFactor;

    GameObject cbmt; // CardboxMouseTracker
    GameObject cards;
    GameObject effects;
    AudioDirector ad;
    GameObject sparklePrefab;

    // Runs when mouse should be tracked, displaying selected object
    Coroutine mouseTracking;

	void Start () {
		selectedCardbox = null;

        effects = GameObject.Find("Effects");
        sparklePrefab = Resources.Load<GameObject>("Prefabs/Sparkle");

        cards = GameObject.Find("Cards");
        cards.GetComponent<CanvasGroup>().alpha = 0;

        mouseTracking = null;

        cbmt = GameObject.Find("CardboxMouseTracker");
        cbmt.SetActive(false);
	}

    // Called by cardbox when selected
    public void CardboxSelected(Cardbox cardbox) {
        selectedCardbox = cardbox;
        
        cbmt.GetComponent<Image>().sprite = selectedCardbox.itemImage.sprite;
        cbmt.GetComponent<Image>().color = selectedCardbox.itemImage.color;

        cbmt.SetActive(true);

        mouseTracking = StartCoroutine(TrackMouse());
    }

    // Run when mouse is being tracked
    IEnumerator TrackMouse() {
        while(true) {
            Vector2 newPos = Input.mousePosition;
            cbmt.transform.position = newPos;
            yield return null;
        }
    }

    // Called by cardbox upon placement of item or deseletion
    public void CardboxDeselected() {
        selectedCardbox = null;

        StopCoroutine(mouseTracking);
        mouseTracking = null;

        cbmt.SetActive(false);
    }

    // Tile emits this when clicked
    public void OnTileClick(Tile tile) {
        // If cardbox is selected, place item
        if(selectedCardbox != null) {
            bool itemAdded = tile.ChangeItem(selectedCardbox.item);
            if(itemAdded) {
                StartCoroutine(SparkleEffect(tile.transform.position));

                selectedCardbox.CardPlaced();
                CardboxDeselected();
            }
            else Debug.Log("Failure to place item from OnTileClick on Tile: " + tile.name);
        }
        else {
            // If item is being harvested, harvest it!
            if(tile.Item is Crop) {
                Crop crop = tile.Item as Crop;
                if(crop.Harvestable) tile.Harvested();
            }
            // If anything else, do nothing
        }
    }

    // Make card items immediately visible on new turn, or disappear when leaving gameplay so they can't be selected
    public void ChangeState(GameState newState) {
        if(newState == GameState.Gameplay) cards.GetComponent<CanvasGroup>().alpha = 1;
        else cards.GetComponent<CanvasGroup>().alpha = 0;
    }

    // Create sparkle animation over tile that item is placed on
    IEnumerator SparkleEffect(Vector2 position) {
        GameObject sparkle = Instantiate(sparklePrefab, position, Quaternion.identity);
        sparkle.transform.parent = effects.transform;
        Animator anim = sparkle.GetComponent<Animator>();
        anim.SetBool("sparkleIn", true);
        anim.SetBool("end", true);
        yield return new WaitForSeconds(1.5f);
        Destroy(sparkle);
        yield return null;
    }
}
