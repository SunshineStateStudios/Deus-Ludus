/*

Controls game loop, using other managers to do so.
Stores player & enemy cards.

Written by plexinator-9000.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> placedNumberCards_Player = new List<GameObject>();
    public List<GameObject> placedNumberCards_Enemy = new List<GameObject>();
    public GameObject cardPrefab;
    public Transform playerHand;
    public Transform enemyHand;

    private List<CardData> deck = new List<CardData>();
    private List<CardData> shuffledDeck = new List<CardData>();

    void Start()
    {
        CreateDeck();
        ShuffleDeck();
    }

    // Create a deck of 52 cards
    void CreateDeck()
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        for (int i = 0; i < suits.Length; i++)
        {
            for (int j = 2; j <= 10; j++)
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

    public void DrawCard(int player)
    {
        if (shuffledDeck.Count > 0)
        {
            CardData drawnCard = shuffledDeck[0];
            shuffledDeck.RemoveAt(0);

            GameObject newCard = Instantiate(cardPrefab, playerHand);
            CardData data = newCard.GetComponent<CardData>();
            data.value = drawnCard.value;
            data.suit = drawnCard.suit;

            if (player == 1)
            {
                placedNumberCards_Player.Add(newCard);
                newCard.transform.SetParent(playerHand);

                Debug.Log(placedNumberCards_Enemy.Count);
                newCard.transform.position -= new Vector3(0, 0, 2.3f * (placedNumberCards_Player.Count - 1));
            }
            else
            {
                placedNumberCards_Enemy.Add(newCard);
                newCard.transform.SetParent(enemyHand);
            }
        }
    }
    
    public void PlayerDrawCard()
    {
        DrawCard(1);
        int playerTotal = GetHandTotal(placedNumberCards_Player);
        Debug.Log("Player's Total: " + playerTotal);

        if (playerTotal > 21)
        {
            Debug.Log("Player Busts!");
        }
    }

    public void EnemyDrawCard()
    {
        DrawCard(2);
        int enemyTotal = GetHandTotal(placedNumberCards_Enemy);
        Debug.Log("Enemy's Total: " + enemyTotal);

        if (enemyTotal > 21)
        {
            Debug.Log("Enemy Busts!");
        }
    }

    public int GetHandTotal(List<GameObject> hand)
    {
        int total = 0;
        bool hasAce = false;
        foreach (var cardObj in hand)
        {
            CardData card = cardObj.GetComponent<CardData>();
            total += card.value;
            if (card.value == 11)
            {
                hasAce = true;
            }
        }

        // If the total is over 21 and the player has an Ace, use the Ace as 1
        if (total > 21 && hasAce)
        {
            total -= 10; // Subtract 10 from the total (making Ace worth 1 instead of 11)
        }

        return total;
    }
}