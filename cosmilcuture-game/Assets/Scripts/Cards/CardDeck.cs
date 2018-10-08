using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour {

    GameObject cardDeck;
    GameObject selectedDeckCards;
    GameObject cardboxPrefab;

	// Use this for initialization
	void Start () {
        cardDeck = GameObject.Find("CardDeck");
        selectedDeckCards = GameObject.Find("SelectedDeckCards");
        cardboxPrefab = Resources.Load<GameObject>("Prefabs/Cards/Cardbox");
	}

    public void AddCard(GameObject card) {
        // Add card to selected deck
        card.transform.SetParent(selectedDeckCards.transform, false);
        card.transform.position = selectedDeckCards.transform.position;
        card.SetActive(false);

        // Create visual representation
        GameObject deckCardbox = Instantiate(cardboxPrefab, transform.position, Quaternion.identity);
        deckCardbox.transform.SetParent(cardDeck.transform, false);
        deckCardbox.GetComponent<Cardbox>().CorrespondingCard = card;
    }
}
