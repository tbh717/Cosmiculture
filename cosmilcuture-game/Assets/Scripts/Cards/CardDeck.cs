using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The script placed on the CardDeck game object
public class CardDeck : MonoBehaviour {

    GameObject selectedDeckCards;
    GameObject cardboxPrefab;

	// Use this for initialization
	void Start () {
        selectedDeckCards = GameObject.Find("SelectedDeckCards");
        cardboxPrefab = Resources.Load<GameObject>("Prefabs/Cards/Cardbox");
	}

    // Adds card to deck
    // Creates both cardbox and corresponding card in visual deck
    public void AddCard(GameObject card) {
        // Add card to selected deck
        card.transform.SetParent(selectedDeckCards.transform, true);
        card.transform.position = selectedDeckCards.transform.position;
        card.SetActive(false);

        // Create visual representation
        GameObject deckCardbox = Instantiate(cardboxPrefab, transform.position, Quaternion.identity);
        deckCardbox.transform.SetParent(gameObject.transform, false);
        deckCardbox.GetComponent<Cardbox>().CorrespondingCard = card;
    }
}
