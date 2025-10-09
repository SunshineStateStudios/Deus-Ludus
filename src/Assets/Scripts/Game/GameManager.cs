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
                    } else
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
        }
        
        if (shuffledDeck.Count == 0)
        {
            CreateDeck();
        }
    }
    
    public void PlayerDrawCard()
    {
        DrawCard(1);
        int playerTotal = GetHandTotal(placedNumberCards_Player);

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

    public void PlayerEndTurn()
    {
        drawButton.SetActive(false);
    }
    
    public void EnemyEndTurn()
    {
        
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