using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameStates;

public class InteractionDirector : MonoBehaviour {

    Cardbox selectedCardbox;
    float scaleFactor;

    public GameObject cbmt; // CardboxMouseTracker
    public GameObject cards;
    public GameObject effects;
    public AudioDirector ad;
    public GameObject sparklePrefab;

	// Use this for initialization
	void Start () {
		selectedCardbox = null;

        cards = GameObject.Find("Cards");
        cards.GetComponent<CanvasGroup>().alpha = 0;

        cbmt = GameObject.Find("CardboxMouseTracker");
        cbmt.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(cbmt.active) {
            Vector2 newPos = Input.mousePosition;
            cbmt.transform.position = newPos;
        }
	}

    public void CardboxSelected(Cardbox cardbox) {
        selectedCardbox = cardbox;
        
        cbmt.GetComponent<Image>().sprite = selectedCardbox.itemImage.sprite;
        cbmt.GetComponent<Image>().color = selectedCardbox.itemImage.color;

        cbmt.SetActive(true);
    }

    public void CardboxDeselected() {
        selectedCardbox = null;

        cbmt.SetActive(false);
    }

    public void OnTileClick(Tile tile) {
        if(selectedCardbox != null) {
            bool itemAdded = tile.ChangeItem(selectedCardbox.item);
            if(itemAdded) {
                StartCoroutine(SparkleEffect(tile.transform.position));
                cbmt.SetActive(false);
                selectedCardbox.CardPlaced();
            }
        }
        else {
            if(tile.Item is Crop) {
                Crop crop = tile.Item as Crop;
                if(crop.Harvestable) tile.Harvested();
            }
        }
    }

    public void ChangeState(GameState newState) {
        if(newState == GameState.Gameplay) cards.GetComponent<CanvasGroup>().alpha = 1;
        else cards.GetComponent<CanvasGroup>().alpha = 0;
    }

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
