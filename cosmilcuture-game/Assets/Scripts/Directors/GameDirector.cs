using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameStates;

public class GameDirector : MonoBehaviour {

    public int score;
    public int turn;
    public int maxTurns;

    public Text turnText;
    public Text scoreText;

    public GameObject cardSelectScreen;
    public GameObject cardSelectDeck;
    public List<GameObject> cardSelectCards = new List<GameObject>();
    private Text cardSelectInstructionText;
    private int cardSelectNum;

    private CardDeck cardDeck;

    private GameState state;

    GameState State {
        get { return state; }
    }

    public delegate void NewState(GameState state);
    public event NewState newState;

    private InteractionDirector id;
    private BoardDirector bd;
    private AudioDirector ad;

	// Use this for initialization
	void Start() {
        score = 0;
		turn = 0;
        maxTurns = 15;
        
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        turnText = GameObject.Find("TurnText").GetComponent<Text>();

        cardSelectScreen = GameObject.Find("CardSelectScreen");
        UpdateTurnText();

        // Find cards in selection deck
        cardSelectDeck = GameObject.Find("CardSelectDeck");
        foreach(Transform cardTransform in cardSelectDeck.transform) cardSelectCards.Add(cardTransform.gameObject);
        cardSelectInstructionText = GameObject.Find("CardSelectInstructionText").GetComponent<Text>();
        cardSelectNum = 2;

        // Add active card deck
        cardDeck = GameObject.Find("CardDeck").GetComponent<CardDeck>();

        bd = GetComponent<BoardDirector>();
        id = GetComponent<InteractionDirector>();
        ad = GameObject.Find("AudioController").GetComponent<AudioDirector>();
        newState += bd.ChangeState;
        newState += id.ChangeState;
        newState += ad.ChangeState;

        StartCoroutine(GameControl());
        StartCoroutine(InvokeState(GameState.StartGame));
	}

    public void ChangeState(GameState ns) {
        state = ns;
        newState(ns);
    }
	
    // Invokes the next state, circumstantially
	public IEnumerator NextState() {
        Debug.Log("NextState called on " + state.ToString().ToUpper());
        switch(state) {
            case GameState.StartGame:
                // Generate first round of cards
                SpawnCards();
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(InvokeState(GameState.CardSelect));
                break;
            case GameState.CardSelect:
                StartCoroutine(InvokeState(GameState.Gameplay));
                // Spawn new cards in background
                yield return new WaitForSeconds(1.5f);
                SpawnCards();
                break;
            case GameState.Gameplay:
                turn++;
                if(turn < maxTurns) {
                    UpdateTurnText();
                    StartCoroutine(InvokeState(GameState.CardSelect));
                }
                else StartCoroutine(InvokeState(GameState.EndGame));
                break;
            default:
                Debug.Log("NextState should not be invoked from state: " + state.ToString());
                break;
        }
        yield break;
    }

    // Takes a new state, sets it, and begins performing actions
    public IEnumerator InvokeState(GameState ns) {
        Debug.Log("State " + ns.ToString().ToUpper() + " invoked");
        switch(ns) {
            case GameState.StartGame:
                // Load card screen
                StartCoroutine(FadeTo(cardSelectScreen.GetComponent<CanvasGroup>(), 1.0f, 0.0f));
                // Wait for game objects to initialize before announcing state
                yield return new WaitForSeconds(0.3f);
                ChangeState(ns);
                // Succeed to stat after StartGame
                StartCoroutine(NextState());
                break;
            case GameState.CardSelect:
                StartCoroutine(FadeTo(cardSelectScreen.GetComponent<CanvasGroup>(), 1.0f, 1.5f));
                ChangeState(ns);
                yield return new WaitForSeconds(1.5f);
                StartCoroutine(CardSelect());
                break;
            case GameState.Gameplay:
                StartCoroutine(FadeTo(cardSelectScreen.GetComponent<CanvasGroup>(), 0.0f, 1.5f));
                yield return new WaitForSeconds(0.5f);
                ChangeState(ns);
                StartCoroutine(Gameplay());
                break;
            case GameState.EndGame:
                EndGame();
                break;
        }

        yield break;
    }

    ItemColor RandomItemColor() {
        int n = Random.Range(0,3);
        switch (n) {
            case 0:
                return new PurpleItemColor();
            case 1:
                return new YellowItemColor();
            case 2:
                return new RedItemColor();
            default:
                throw new System.Exception("Unknown number chosen in RandomItemColor: " + n);
        }
    }

    Item RandomItem() {
        // Item type
        int n = Random.Range(0,3);
        ItemColor ic;
        int nn;
        switch(n) {
            // Flora
            case 0:
                ic = RandomItemColor();
                nn = Random.Range(0,5);
                switch(nn) {
                    case 0: return new Magnolium(ic);
                    case 1: return new Glowlilies(ic);
                    case 2: return new Gargantua(ic);
                    case 3: return new Flytrap(ic);
                    case 4: return new Shrub(ic);
                    default: throw new System.Exception("Unknown number chosen in RandomItemColor during Flora selection: " + nn);
                }
            // Crop
            case 1:
                ic = RandomItemColor();
                nn = Random.Range(0,3);
                switch(nn) {
                    case 0: return new Mushroom(ic);
                    case 1: return new Fruit(ic);
                    case 2: return new Vegetable(ic);
                    default: throw new System.Exception("Unknown number chosen in RandomItemColor during Crop selection: " + nn);
                }
            case 2:
                nn = Random.Range(0,5);
                switch(nn) {
                    case 0: return new Hive();
                    case 1: return new Obelisk();
                    case 2: return new Tent();
                    case 3: return new Meteorite();
                    case 4: return new Picnic();
                    default: throw new System.Exception("Unknown number chosen in RandomItemColor during Decor selection: " + nn);
                }
            default:
                throw new System.Exception("Unknown number chosen in RandomItemColor during item class selection: " + n);
        }
            
    }

    // Randomly spawns cards for CardSelect
    void SpawnCards() {
        foreach(GameObject cardObj in cardSelectCards) {
            Card card = cardObj.GetComponent<Card>();
            ItemComponent ic = cardObj.GetComponent<ItemComponent>();

            ic.Item = RandomItem();

            card.UpdateCard();
        }
    }

    // Take selected objects from SpawnCards and add to deck
    void AddCardsToDeck(HashSet<GameObject> newCards) {
        if(newCards.Count > cardSelectNum) throw new System.Exception("More than " + cardSelectNum + " cards selected!");
        foreach(GameObject cardObj in newCards) {
            GameObject newCardObj = Instantiate(cardObj, cardObj.transform.position, Quaternion.identity);
            newCardObj.GetComponent<ItemComponent>().Item = cardObj.GetComponent<ItemComponent>().Item;
            newCardObj.GetComponent<RectTransform>().sizeDelta = new Vector2(
                cardObj.GetComponent<RectTransform>().sizeDelta.x,
                cardObj.GetComponent<RectTransform>().sizeDelta.y
            );
            cardDeck.AddCard(newCardObj);
        }
    }
    
    IEnumerator CardSelect() {
        StartCoroutine(FadeTo(cardSelectDeck.GetComponent<CanvasGroup>(), 1.0f, 1.0f));
        // Get selected cards during loop
        HashSet<GameObject> selectedCards = new HashSet<GameObject>();
        while(true) {
            // Add new selected cards
            foreach(GameObject cardObj in cardSelectCards) {
                Card card = cardObj.GetComponent<Card>();
                if(card.IsSelected && card.CanSelect) selectedCards.Add(cardObj);
                else selectedCards.Remove(cardObj);
                if(selectedCards.Count >= cardSelectNum) {
                    if(!selectedCards.Contains(cardObj)) card.CanSelect = false;
                    else card.CanSelect = true;
                }
                else card.CanSelect = true;
            }
            // Try to end turn
            if(Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space)) break;
            else yield return new WaitForEndOfFrame();
        }
        // Deselect each selected card
        foreach(GameObject cardObj in cardSelectCards) cardObj.GetComponent<Card>().CanSelect = true;
        foreach(GameObject selectedCardObj in selectedCards) selectedCardObj.GetComponent<Card>().Select();
        // Move onto gameplay
        AddCardsToDeck(selectedCards);
        StartCoroutine(NextState());
        yield break;
    }

    void DisableGameplay() {}
    void EnableGameplay() {}

    IEnumerator Gameplay() {
        while(true) {
            if(Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space)) break;
            else yield return new WaitForEndOfFrame();
        }
        // Move onto next turn select
        StartCoroutine(NextState());
        yield break;
    }

    void EndGame() {
        PlayerPrefs.SetInt("score", score);
        SceneManager.LoadScene("EndGame");
    }

    // Controls start menu (if implemented) / ending game
    IEnumerator GameControl() {
        while(true) {
            if(Input.GetKey(KeyCode.Escape)) EndGame();
            yield return new WaitForEndOfFrame();
        }
    }

    private void UpdateTurnText() {
        int turnsLeft = maxTurns - turn;
        turnText.text = turnsLeft.ToString();
    }

    public void UpdateScore(int newScore) {
        score = newScore;
        scoreText.text = score.ToString();
    }

    private IEnumerator FadeTo(CanvasGroup cg, float avalue, float atime) {
        float alpha = cg.alpha;
        // Fading from a low value into a high
        if(avalue > alpha) {
            for(float t = cg.alpha; t < avalue; t += Time.deltaTime / atime) {
                cg.alpha = Mathf.Lerp(alpha,avalue,t);
                yield return null;
            }
        }
        // Fading from a high value into a low
        else {
            for(float t = cg.alpha; t > avalue; t -= Time.deltaTime / atime) {
                cg.alpha = Mathf.Lerp(avalue,alpha,t);
                yield return null;
            }
        }
        
        cg.alpha = avalue;
        yield break;
    }
}
