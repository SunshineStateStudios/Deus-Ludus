/*

Controls game loop, using other managers to do so.
Stores player & enemy cards.

Written by plexinator-9000.

*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> placedNumberCards_Player = new List<GameObject>();
    public List<GameObject> placedNumberCards_Enemy = new List<GameObject>();
    public GameObject cardPrefab;
    public Transform playerHand;
    public Transform enemyHand;
    public GameObject drawButton;
    public GameObject endTurnButton;
    public GameObject deckValueText;

    private List<CardData> deck = new List<CardData>();
    private List<CardData> shuffledDeck = new List<CardData>();

    void Start()
    {
        CreateDeck();
        ShuffleDeck();
        for (int i = 0; i < 2; i++) { PlayerDrawCard(); }
        for (int i = 0; i < 2; i++) { EnemyDrawCard(); }
    }

    // Create a deck of 52 cards
    void CreateDeck()
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        for (int i = 0; i < suits.Length; i++)
        {
            for (int j = 1; j <= 10; j++)
            {
                CardData card = new CardData(j, suits[i]);
                deck.Add(card);
            }

            string[] faceCards = { "Jack", "Queen", "King" };
            foreach (var face in faceCards)
            {
                CardData card = new CardData(10, suits[i]);
                deck.Add(card);
            }

            CardData ace = new CardData(11, suits[i]);
            deck.Add(ace);
        }
    }

    void ShuffleDeck()
    {
        shuffledDeck = new List<CardData>(deck);
        for (int i = 0; i < shuffledDeck.Count; i++)
        {
            CardData temp = shuffledDeck[i];
            int randomIndex = Random.Range(i, shuffledDeck.Count);
            shuffledDeck[i] = shuffledDeck[randomIndex];
            shuffledDeck[randomIndex] = temp;
        }
    }

    private void SetMaterial(GameObject obj, string texture)
    {
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        Material[] materials = meshRenderer.materials;
        materials[0] = Resources.Load<Material>(texture);
        meshRenderer.materials = materials;
    }

    public object DrawCard(int player)
    {
        if (shuffledDeck.Count > 0)
        {
            CardData drawnCard = shuffledDeck[0];
            shuffledDeck.RemoveAt(0);

            GameObject newCard = Instantiate(cardPrefab, playerHand);
            CardData data = newCard.GetComponent<CardData>();
            data.value = drawnCard.value;
            data.suit = drawnCard.suit;
            SetMaterial(newCard, "Textures/Cards/Materials/" + drawnCard.value.ToString());

            if (player == 1)
            {
                placedNumberCards_Player.Add(newCard);
                newCard.transform.SetParent(playerHand);
                newCard.transform.position -= new Vector3(0, 0, 2.3f * (placedNumberCards_Player.Count - 1));

                int playerTotal = GetHandTotal(placedNumberCards_Player);
                TextMeshProUGUI deckText = deckValueText.GetComponent<TextMeshProUGUI>();
                if (playerTotal > 21)
                {
                    drawButton.SetActive(false);
                    deckText.SetText("Oh no! It's a bust! (Went over 21, you had " + playerTotal.ToString() + " points)");
                    deckText.color = Color.red;
                }
                else
                {
                    if (playerTotal == 21)
                    {
                        drawButton.SetActive(false);
                        deckText.SetText("Well in! You got exactly 21 points in your deck!");
                        deckText.color = Color.green;
                    }
                    else
                    {
                        deckText.SetText("Your deck's value: " + playerTotal.ToString());
                        deckText.color = Color.white;
                    }
                }
            }
            else
            {
                placedNumberCards_Enemy.Add(newCard);
                newCard.transform.SetParent(enemyHand);
                newCard.transform.position -= new Vector3(-8, 0, 2.3f * (placedNumberCards_Enemy.Count - 1));

                if (placedNumberCards_Enemy.Count == 1)
                {
                    SetMaterial(newCard, "Textures/Cards/Materials/unknown");
                }
            }

            return data;
        }

        if (shuffledDeck.Count == 0)
        {
            CreateDeck();
        }

        return null;
    }
    
    public void PlayerDrawCard()
    {
        DrawCard(1);
    }

    public CardData EnemyDrawCard()
    {
        CardData data = (CardData) DrawCard(2);
        return data;
    }

    private IEnumerator PlayerEndTurn()
    {
        drawButton.SetActive(false);
        endTurnButton.SetActive(false);

        TextMeshProUGUI deckText = deckValueText.GetComponent<TextMeshProUGUI>();
        deckText.SetText("The enemy is thinking...");
        deckText.color = Color.white;

        // AI's turn

        // Set first card of enemy to the correct texture
        GameObject firstEnemyCard = placedNumberCards_Enemy[0];
        SetMaterial(firstEnemyCard, "Textures/Cards/Materials/" + firstEnemyCard.GetComponent<CardData>().value.ToString());

        int enemyTotal = GetHandTotal(placedNumberCards_Enemy);
        int playerTotal = GetHandTotal(placedNumberCards_Player);
        // Guarantee a hit if total is < 17.
        while (enemyTotal < 17)
        {
            CardData data = EnemyDrawCard();
            enemyTotal += data.value;
            yield return new WaitForSecondsRealtime(0.65f);
        }

        // Take a gamble to draw an extra time (30% chance)
        if (Random.value <= 0.3)
        {
            CardData data = EnemyDrawCard();
            enemyTotal += data.value;
        }

        int gameResult;
        /*

            0 = draw
            1 = player win
            2 = opponent win

        */

        if (playerTotal > 21 && enemyTotal > 21)
        {
            gameResult = 0;
        }
        else if (playerTotal > 21)
        {
            if (enemyTotal > 21 && playerTotal < enemyTotal)
            {
                gameResult = 1;
            } else
            {
                gameResult = 2;
            }
        }
        else if (enemyTotal > 21)
        {
            if (playerTotal > 21 && enemyTotal < playerTotal)
            {
                gameResult = 2;
            } else
            {
                gameResult = 1;
            }
        }
        else if (playerTotal > enemyTotal)
        {
            gameResult = 1;
        }
        else if (enemyTotal > playerTotal)
        {
            gameResult = 2;
        }
        else
        {
            gameResult = 0;
        }

        string resultingText = "";
        switch (gameResult)
        {
            case 0:
                resultingText = "Draw! Nobody won.";
                deckText.color = Color.yellow;
                break;
            case 1:
                resultingText = "You won!";
                deckText.color = Color.green;
                break;
            case 2:
                resultingText = "You lost...";
                deckText.color = Color.red;
                break;
        }
        resultingText = resultingText + "\nYour deck: " + playerTotal + " | Their deck: " + enemyTotal + " | A new round will begin in 5 seconds.";
        deckText.SetText(resultingText);

        yield return new WaitForSecondsRealtime(5);
        Cleanup();
    }

    public void PlayerEndTurnCoroutine()
    {
        StartCoroutine(PlayerEndTurn());
    }

    private void ClearAllChildren(Transform parentObject)
    {
        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void Cleanup()
    {
        ClearAllChildren(playerHand);
        ClearAllChildren(enemyHand);

        endTurnButton.SetActive(true);
        drawButton.SetActive(true);

        placedNumberCards_Player = new List<GameObject>();
        placedNumberCards_Enemy = new List<GameObject>();

        for (int i = 0; i < 2; i++) { PlayerDrawCard(); }
        for (int i = 0; i < 2; i++) { EnemyDrawCard(); }
    }

    public int GetHandTotal(List<GameObject> hand)
    {
        // Count everything regularly
        int total = 0;
        foreach (var cardObj in hand)
        {
            CardData card = cardObj.GetComponent<CardData>();
            total += card.value;
        }

        // Subtract by 10 for each ace there are (whenever applicable)
        if (total > 21)
        {
            foreach (var cardObj in hand)
            {
                CardData card = cardObj.GetComponent<CardData>();
                if (card.value == 11)
                {
                    total -= 10;
                    if (total <= 21) { break; }
                }
            }
        }

        return total;
    }
}