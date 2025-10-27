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
    public GameObject inventoryPanel;
    public GameObject abilityCardUIPrefab;

    private List<CardData> deck = new List<CardData>();
    private List<CardData> shuffledDeck = new List<CardData>();
    private AudioSource audioSource;
    private string[] possibleAbilityCards = { "test1", "test2" };
    private List<AbilityCard> playerInventory = new List<AbilityCard>();
    private List<AbilityCard> enemyInventory = new List<AbilityCard>();

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        CreateDeck();
        ShuffleDeck();
        for (int i = 0; i < 2; i++) { DrawCard(1); }
        for (int i = 0; i < 2; i++) { DrawCard(2); }

        for (int i = 0; i < 2; i++) { GiveAbilityCard(1); }
        for (int i = 0; i < 2; i++) { GiveAbilityCard(2); }
    }

    // Create a deck of 52 cards
    void CreateDeck()
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        for (int i = 0; i < suits.Length; i++)
        {
            for (int j = 1; j <= 11; j++)
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

            CardData ace = new CardData(13, suits[i]);
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

    private void UpdateInventory()
    {
        ClearAllChildren(inventoryPanel.transform);

        for (int i = 0; i < playerInventory.Count; i++)
        {
            AbilityCard card = playerInventory[i];
            GameObject uiCard = Instantiate(abilityCardUIPrefab, inventoryPanel.transform);
            AbilityCardUI uiCard_script = uiCard.GetComponent<AbilityCardUI>();
            TextMeshProUGUI uiCard_text = uiCard.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            uiCard_script.index = i;
            uiCard_text.SetText(card.GetName());
        }
    }

    private void GiveAbilityCard(int player)
    {
        if ((player == 1 && playerInventory.Count < 10) || (player == 2 && enemyInventory.Count < 10))
        {
            string chosenCardName = possibleAbilityCards[Random.Range(0, possibleAbilityCards.Length)];
            GameObject abilityCardPrefab = Resources.Load<GameObject>("Prefabs/AbilityCards/" + chosenCardName);
            AbilityCard abilityCard = abilityCardPrefab.GetComponent<AbilityCard>();

            if (player == 1)
            {
                playerInventory.Add(abilityCard);
                UpdateInventory();
            } else
            {
                enemyInventory.Add(abilityCard);
            }
        }
    }

    private void PlaySound(string pathToClip)
    {
        audioSource.clip = Resources.Load<AudioClip>(pathToClip);
        audioSource.Play();
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
        CardData drawnCard = shuffledDeck[0];
        shuffledDeck.RemoveAt(0);

        GameObject newCard = Instantiate(cardPrefab, playerHand);
        CardData data = newCard.GetComponent<CardData>();
        data.value = drawnCard.value;
        data.suit = drawnCard.suit;

        PlaySound("Sounds/draw");

        if (data.value == 13)
        {
            SetMaterial(newCard, "Textures/Cards/Materials/ace");
        } else
        {
            SetMaterial(newCard, "Textures/Cards/Materials/" + drawnCard.value.ToString());
        }

        if (player == 1)
        {
            placedNumberCards_Player.Add(newCard);
            newCard.transform.SetParent(playerHand);
            newCard.transform.position -= new Vector3(0, 0, 2.3f * (placedNumberCards_Player.Count - 1));

            int playerTotal = GetHandTotal(placedNumberCards_Player);
            TextMeshProUGUI deckText = deckValueText.GetComponent<TextMeshProUGUI>();

            deckText.SetText("Your deck's value: " + playerTotal.ToString());
            if (playerTotal > 21)
            {
                deckText.color = Color.red;
            }
            else
            {
                if (playerTotal == 21)
                {
                    deckText.color = Color.green;
                }
                else
                {
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

        if (shuffledDeck.Count == 0)
        {
            CreateDeck();
            ShuffleDeck();
        }


        return data;
    }

    public void PlayerDrawCard()
    {
        DrawCard(1);
        StartCoroutine(EndPlayerTurn(false));
    }

    public void PlayerStay()
    {
        StartCoroutine(EndPlayerTurn(true));
    }

    private string DetermineWinner(int playerValue, int enemyValue)
    {
        if (playerValue > 21 && enemyValue > 21)
        {
            if (playerValue < enemyValue)
            {
                return "Player";
            }
            else if (enemyValue < playerValue)
            {
                return "Enemey";
            }
            else
            {
                return "Draw";
            }
        }

        if (playerValue > 21)
        {
            return "Enemy";
        }

        if (enemyValue > 21)
        {
            return "Player";
        }

        if (playerValue > enemyValue)
        {
            return "Player";
        }
        else if (enemyValue > playerValue)
        {
            return "Enemy";
        } else
        {
            return "Draw";
        }
    }

    private IEnumerator EndPlayerTurn(bool didPlayerStay)
    {
        drawButton.SetActive(false);
        endTurnButton.SetActive(false);
        inventoryPanel.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        TextMeshProUGUI deckText = deckValueText.GetComponent<TextMeshProUGUI>();
        deckText.SetText("The AI is thinking...");
        deckText.color = Color.white;

        /*

        AI thought process:
            Are we at 21 or over 21?
                Stay (ends turn)
            Are we under 17?
                Draw (ends turn)
            Otherwise...
                Store "chance" variable, which determines how likely the AI is to draw from 0-100%.
                Are we at 18?
                    chance = 20%
                Are we at 19?
                    chance = 15%
                Are we at 20?
                    chance = 5%
                Are we at 21?
                    chance = 0%
                
                If "chance"/100 <= Random.value...
                    Draw (ends turn)
                Otherwise...
                    Stay (ends turn)

        */

        yield return new WaitForSeconds(2);

        bool didEnemyStay = false;
        int enemyHandTotal = GetHandTotal(placedNumberCards_Enemy);

        if (enemyHandTotal > 21 || enemyHandTotal == 21)
        {
            didEnemyStay = true;
        }
        else if (enemyHandTotal < 17)
        {
            DrawCard(2);
        }
        else
        {
            float chance = 0;

            switch (enemyHandTotal)
            {
                case 18:
                    chance = 0.2f;
                    break;
                case 19:
                    chance = 0.15f;
                    break;
                case 20:
                    chance = 0.05f;
                    break;
            }

            if (chance <= Random.value)
            {
                DrawCard(2);
            }
            else
            {
                didEnemyStay = true;
            }
        }

        // Checking to see if both players stayed. If so, end the round.
        if (didEnemyStay && didPlayerStay)
        {
            deckText.SetText("Both players have stayed, calculating results...");

            yield return new WaitForSeconds(1.5f);

            // Set first card of enemy to the correct texture
            GameObject firstEnemyCard = placedNumberCards_Enemy[0];
            CardData firstEnemyCardData = firstEnemyCard.GetComponent<CardData>();
            if (firstEnemyCardData.value == 13)
            {
                SetMaterial(firstEnemyCard, "Textures/Cards/Materials/ace");
            }
            else
            {
                SetMaterial(firstEnemyCard, "Textures/Cards/Materials/" + firstEnemyCardData.value.ToString());
            }
            PlaySound("Sounds/reveal_card");

            enemyHandTotal = GetHandTotal(placedNumberCards_Enemy);
            int playerTotalValue = GetHandTotal(placedNumberCards_Player);

            string whoWon = DetermineWinner(playerTotalValue, enemyHandTotal);

            switch (whoWon)
            {
                case "Player":
                    deckText.SetText("You won!");
                    deckText.color = Color.green;
                    break;
                case "Enemy":
                    deckText.SetText("You lost...");
                    deckText.color = Color.red;
                    break;
                default:
                    deckText.SetText("Stalemate! Nobody won.");
                    deckText.color = Color.yellow;
                    break;
            }

            yield return new WaitForSeconds(5);
            Cleanup();
            for (int i = 0; i < 2; i++) { GiveAbilityCard(1); }
            for (int i = 0; i < 2; i++) { GiveAbilityCard(2); }
            inventoryPanel.SetActive(true);
        }
        else
        {
            int playerTotalValue = GetHandTotal(placedNumberCards_Player);
            deckText.SetText("Your deck's value: " + playerTotalValue.ToString());

            if (playerTotalValue == 21)
            {
                deckText.color = Color.green;
                drawButton.SetActive(true);
            }
            else if (playerTotalValue > 21)
            {
                deckText.color = Color.red;
            }
            else
            {
                deckText.color = Color.white;
                drawButton.SetActive(true);
            }

            endTurnButton.SetActive(true);
            inventoryPanel.SetActive(true);
        }
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

        for (int i = 0; i < 2; i++) { DrawCard(1); }
        for (int i = 0; i < 2; i++) { DrawCard(2); }
    }

    public int GetHandTotal(List<GameObject> hand)
    {
        // Count everything regularly
        int total = 0;
        foreach (var cardObj in hand)
        {
            CardData card = cardObj.GetComponent<CardData>();

            int val = card.value;

            if (card.value == 13)
            {
                if (total > 21 || total + 11 > 21)
                {
                    val = 1;
                }
                else
                {
                    val = 11;
                }
            }
            
            total += val;
        }

        return total;
    }
}