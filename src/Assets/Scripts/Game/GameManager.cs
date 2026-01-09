/*

Controls game loop, using other managers to do so.
Stores player & enemy cards.

Written by plexinator-9000.

*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public List<GameObject> placedNumberCards_Player = new List<GameObject>();
    public List<GameObject> placedNumberCards_Enemy = new List<GameObject>();
    public GameObject cardPrefab;
    public GameObject abilityCardPrefab;
    public Transform playerHand;
    public Transform playerAbilityHand;
    public Transform enemyAbilityHand;
    public Transform enemyHand;
    public GameObject drawButton;
    public GameObject endTurnButton;
    public GameObject deckValueText;
    public GameObject inventoryPanel;
    public GameObject descriptionMenu;
    public TextMeshProUGUI descriptionText;
    public GameObject abilityCardUIPrefab;
    public TextMeshProUGUI DmgOpp; // Ryzer damage display
    public TextMeshProUGUI DmgYour; // Your Damage display
    public TextMeshProUGUI playerHPs; // HP display
    public int threshold = 21;
    public string state = "PlayerTurn";
    public GameObject camera;

    public int oppDMG = 1; //ryzer's damage 
    public int youDMG = 1; //your damage 
    public int oppHP = 10; //ryzer's HP
    public int youHP = 10; //your HP 

    private List<CardData> deck = new List<CardData>();
    private List<CardData> shuffledDeck = new List<CardData>();
    private AudioSource audioSource;
    private List<AbilityCard> playerInventory = new List<AbilityCard>();
    private List<AbilityCard> enemyInventory = new List<AbilityCard>();
    private Vector3 originalCamPosition;

    void Start()
    {
	if (camera != null) {
	     originalCamPosition = camera.transform.position;
	}

        audioSource = gameObject.GetComponent<AudioSource>();

        CreateDeck();
        ShuffleDeck();
        for (int i = 0; i < 2; i++) { DrawCard(1); }
        for (int i = 0; i < 2; i++) { DrawCard(2); }

        for (int i = 0; i < 2; i++) { GiveAbilityCard(1); }
        for (int i = 0; i < 2; i++) { GiveAbilityCard(2); }

        foreach (AbilityCard card in Resources.LoadAll<AbilityCard>("Scripts/AbilityCards")) {
            Debug.Log(card.GetName());
        }
        DmgOpp.SetText("</color><color=#ff0000>" + oppDMG);
        DmgYour.SetText("</color><color=#1e90ff>" + youDMG);
        playerHPs.SetText("</color><color=#ff0000>" + oppHP + "</color> --- </color><color=#1e90ff>" + youHP);
    }

    // Create a deck of 52 cards
    private void CreateDeck()
    {
        deck = new List<CardData>();
        string[] suits = { "hearts", "diamonds", "clubs", "spades" };

        for (int j = 1; j <= 11; j++)
        {
            CardData card = new CardData(j, suits[Random.Range(0,3)]);
            deck.Add(card);
        }

        CardData ace = new CardData(13, suits[Random.Range(0,3)]);
        deck.Add(ace);
    }

    public void DrawAbilityCard(int index, int player)
    {
        List<AbilityCard> inventory = (player == 1) ? playerInventory : enemyInventory;
        Transform abilityHand = (player == 1) ? playerAbilityHand : enemyAbilityHand;

        if (index < 0 || index >= inventory.Count) return;

        AbilityCard card = inventory[index];
        if (card.drawn) return;

        card.Execute(player);
        card.drawn = true;
        PlaySound("Sounds/draw_ability");

        if (card.GetLifeTime() > 0) {
            GameObject physicalCard = Instantiate(abilityCardPrefab, abilityHand);

            physicalCard.name = index.ToString();

            int drawnCards = 0;
            for (int i = 0; i < inventory.Count; i++) {
                AbilityCard loopCard = inventory[i];
                if (!loopCard.drawn) { continue; }
                drawnCards++;
            }

            physicalCard.transform.position += new Vector3(0, 0, 2f * (drawnCards-1));

            AbilityCardDescMenu physicalCardScript = physicalCard.GetComponent<AbilityCardDescMenu>();
            physicalCardScript.description = card.GetDescription();
            physicalCardScript.descriptionMenu = descriptionMenu;
            physicalCardScript.descriptionLabel = descriptionText;
        } else {
            inventory.RemoveAt(index);
        }

        if (player == 1) UpdateInventory();
    }

    private void ShuffleDeck()
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
        if (inventoryPanel == null) {
            Debug.LogWarning("Inventory panel is missing in the inspector!");
            return;
        }

        if (abilityCardUIPrefab == null) {
            Debug.LogWarning("Ability card UI prefab is missing in the inspector!");
            return;
        }

        ClearAllChildren(inventoryPanel.transform);

        if (playerInventory == null || playerInventory.Count == 0) {
            Debug.Log("Player has no ability cards to display.");
            return;
        }

        for (int i = 0; i < playerInventory.Count; i++) {
            AbilityCard card = playerInventory.ElementAt(i);
            if (card == null) {
                Debug.LogWarning($"Null ability card at index {i} -- skipping.");
                continue;
            }

            if (card.drawn) {
                Debug.Log("Skipping drawn card.");
                continue;
            }

            GameObject uiCard = Instantiate(abilityCardUIPrefab, inventoryPanel.transform);
            AbilityCardUI uiCardScript = uiCard.GetComponent<AbilityCardUI>();

            AbilityCardDescMenu2D uiCardDescScript = uiCard.GetComponent<AbilityCardDescMenu2D>();
            uiCardDescScript.description = card.GetDescription();
            uiCardDescScript.descriptionMenu = descriptionMenu;
            uiCardDescScript.descriptionLabel = descriptionText;

            TextMeshProUGUI uiCardText = uiCard.transform.Find("Text")?.GetComponent<TextMeshProUGUI>();

            if (uiCardScript == null || uiCardText == null) {
                Debug.LogWarning($"AbilityCardUI prefab missing required components (AbilityCardUI or Text).");
                Destroy(uiCard);
                continue;
            }

            uiCardScript.index = i;
            uiCardText.SetText(card.GetName());
        }
    }

    private void GiveAbilityCard(int player)
    {
        List<AbilityCard> inventory = (player == 1) ? playerInventory : enemyInventory;

        if (inventory.Count >= 10) return;

        AbilityCard[] allCards = Resources.LoadAll<AbilityCard>("AbilityCards");

        if (allCards.Length == 0) {
            Debug.LogWarning("No ability cards found in Resources/AbilityCards!");
            return;
        }

        AbilityCard baseCard = allCards[Random.Range(0, allCards.Length)];
        AbilityCard newCard = ScriptableObject.Instantiate(baseCard);

        while (inventory.Any(c => c.GetName() == newCard.GetName()) && inventory.Count < allCards.Length) {
            for (int i = 0; i < allCards.Length; i++) {
                AbilityCard iteratedCard = allCards[i];
                int cardTier = iteratedCard.GetTier();
                float chanceToGive = 0;

                switch(cardTier) {
                    case 1:
                        chanceToGive = 0.75f;
                        break;
                    case 2:
                        chanceToGive = 0.35f;
                        break;
                    case 3:
                        chanceToGive = 0.15f;
                        break;
                }

                if (chanceToGive >= Random.value) {
                    newCard = iteratedCard;
                    break;
                }
            }
        }

        newCard.drawn = false;
        inventory.Add(newCard);
        inventory.Sort();

        if (player == 1) UpdateInventory();
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

    public int UpdateHandValueText() {
        int playerTotal = GetHandTotal(placedNumberCards_Player);
        TextMeshProUGUI deckText = deckValueText.GetComponent<TextMeshProUGUI>();

        deckText.SetText("Your deck's value: " + playerTotal.ToString() + " (" + threshold.ToString() + ")");

        if (playerTotal > threshold)
        {
            deckText.color = Color.red;
            return 2;
        }
        else
        {
            if (playerTotal == threshold)
            {
                deckText.color = Color.green;
                return 1;
            }
            else
            {
                deckText.color = Color.white;
                return 0;
            }
        }
    }

    private void MoveCamera() {
        int OutOfBoundCards = 0;

        if (placedNumberCards_Enemy.Count > 3) {
            OutOfBoundCards = placedNumberCards_Enemy.Count - 4;
        }

        if (placedNumberCards_Player.Count > 3 && placedNumberCards_Player.Count - 4 > OutOfBoundCards) {
            OutOfBoundCards = placedNumberCards_Player.Count - 4;
        }

        camera.transform.DOMove(originalCamPosition + new Vector3(0, 0, OutOfBoundCards), 0.07f);
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
            SetMaterial(newCard, "Textures/Cards/Materials/ace_" + data.suit);
        } else
        {
            SetMaterial(newCard, "Textures/Cards/Materials/" + drawnCard.value.ToString() + "_" + data.suit);
        }

        if (player == 1)
        {
            placedNumberCards_Player.Add(newCard);
            newCard.transform.SetParent(playerHand);
            newCard.transform.position += new Vector3(0, 0, 2.3f * (placedNumberCards_Player.Count - 1));

            UpdateHandValueText();
        }
        else
        {
            placedNumberCards_Enemy.Add(newCard);
            newCard.transform.SetParent(enemyHand);
            newCard.transform.position += new Vector3(-43.5f, 0, 2.3f * (placedNumberCards_Enemy.Count - 1));

	    data.isFromEnemy = true;

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

        MoveCamera();

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
        if (playerValue > threshold && enemyValue > threshold)
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

        if (playerValue > threshold)
        {
            return "Enemy";
        }

        if (enemyValue > threshold)
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

    private void DestroyRedundantAbilityCards(List<AbilityCard> inventory)
    {
        int whichPlayer = 1;
        if (inventory == enemyInventory) whichPlayer = 2;

        Transform abilityHand = (whichPlayer == 1) ? playerAbilityHand : enemyAbilityHand;

        for (int i = inventory.Count - 1; i >= 0; i--) {
            AbilityCard card = inventory[i];
            if (card.drawn) {
                card.IncrementDecayTime();
                if (card.GetDecayTime() >= card.GetLifeTime()) {
                    card.Destroyed(whichPlayer);
                    inventory.RemoveAt(i);

                    Transform physicalCardTransform = abilityHand.transform.Find(i.ToString());
                    if (physicalCardTransform == null) continue;
                    GameObject physicalCard = physicalCardTransform.gameObject;
                    Destroy(physicalCard);

                    foreach (Transform child in abilityHand.transform) {
                        child.position -= new Vector3(0,0,1.85f);
                    }
                }
            }
        }

        inventory.Sort();
    }

    private IEnumerator EndPlayerTurn(bool didPlayerStay)
    {
        drawButton.SetActive(false);
        endTurnButton.SetActive(false);
        inventoryPanel.SetActive(false);
        state = "AITurn";

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

        // Draw ability cards
        int currentAbilityIndex = 0;
        while (currentAbilityIndex < enemyInventory.Count)
        {
            AbilityCard currentCard = enemyInventory[currentAbilityIndex];
            currentAbilityIndex++;
            if (!currentCard.AIDrawAbilityCard()) continue;
            DrawAbilityCard(currentAbilityIndex-1, 2);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        bool didEnemyStay = false;
        int enemyHandTotal = GetHandTotal(placedNumberCards_Enemy);

        if (enemyHandTotal > threshold || enemyHandTotal == threshold)
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

            if (enemyHandTotal == (threshold-4)) {
                chance = 0.2f;
            } else if (enemyHandTotal == (threshold-3)) {
                chance = 0.15f;
            } else if (enemyHandTotal == (threshold-2)) {
                chance = 0.05f;
            } else if (enemyHandTotal == (threshold-1)) {
                chance = 0.01f;
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
            state = "Results";

            yield return new WaitForSeconds(1.5f);

            // Set first card of enemy to the correct texture
            GameObject firstEnemyCard = placedNumberCards_Enemy[0];
            CardData firstEnemyCardData = firstEnemyCard.GetComponent<CardData>();
            if (firstEnemyCardData.value == 13)
            {
                SetMaterial(firstEnemyCard, "Textures/Cards/Materials/ace_" + firstEnemyCardData.suit);
            }
            else
            {
                SetMaterial(firstEnemyCard, "Textures/Cards/Materials/" + firstEnemyCardData.value.ToString() + "_" + firstEnemyCardData.suit);
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
            setHP(oppDMG, youDMG, oppHP, youHP); // calculates hp changes

            yield return new WaitForSeconds(5);
            Cleanup();
            
            CreateDeck();

            DestroyRedundantAbilityCards(playerInventory);
            DestroyRedundantAbilityCards(enemyInventory);

            for (int i = 0; i < 2; i++) { GiveAbilityCard(1); }
            for (int i = 0; i < 2; i++) { GiveAbilityCard(2); }

            inventoryPanel.SetActive(true);
        }
        else
        {
            int didPlayerLose = UpdateHandValueText();

            if (didPlayerLose == 0) drawButton.SetActive(true);

            endTurnButton.SetActive(true);
            inventoryPanel.SetActive(true);
        }

        state = "PlayerTurn";
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
                val = 11;
            }
            total += val;
        }

        foreach (var cardObj in hand)
        {
            CardData card = cardObj.GetComponent<CardData>();

            if (card.value == 13 && total > threshold)
            {
                total -= 10;
            }

        }

        return total;
    }

    // Methods that are specific to ability cards

    public void OuroborosCard(int player) {
        if (player == 1)
        {
            ClearAllChildren(playerHand);
            placedNumberCards_Player = new List<GameObject>();
            for (int i = 0; i < 2; i++) { DrawCard(1); }
            return;
        }
        ClearAllChildren(enemyHand);
        placedNumberCards_Enemy = new List<GameObject>();
        for (int i = 0; i < 2; i++) { DrawCard(2); }
    }

    public void setHP(int oppdmg, int youdmg, int opphp, int youhp)
    {
        int playerTotalValue2 = GetHandTotal(placedNumberCards_Player);
        int enemyHandTotal2 = GetHandTotal(placedNumberCards_Enemy);
        string whoWon2 = DetermineWinner(playerTotalValue2, enemyHandTotal2);
        switch (whoWon2)
            {
                case "Player":
                    youdmg *= 2;
                    oppdmg -= youdmg;
                    if(oppdmg < 0)
                    {
                        opphp += oppdmg;
                        youhp -= oppdmg;
                    }

                    playerHPs.SetText("</color><color=#ff0000>" + opphp + "</color> --- </color><color=#1e90ff>" + youhp);
                    youHP = youhp;
                    oppHP = opphp;
                    youDMG = 1;
                    oppDMG = 1;
                    DmgOpp.SetText("</color><color=#ff0000>" + oppDMG);
                    DmgYour.SetText("</color><color=#1e90ff>" + youDMG);

                    break;
                case "Enemy":
                    oppdmg *= 2;
                    youdmg -= oppdmg;
                    if(youdmg < 0)
                    {
                        youhp += youdmg;
                        opphp -= youdmg;
                    }

                    playerHPs.SetText("</color><color=#ff0000>" + opphp + "</color> --- </color><color=#1e90ff>" + youhp);
                    youHP = youhp;
                    oppHP = opphp;
                    youDMG = 1;
                    oppDMG = 1;
                    DmgOpp.SetText("</color><color=#ff0000>" + oppDMG);
                    DmgYour.SetText("</color><color=#1e90ff>" + youDMG);
            

                    break;
                default:
                    break;
            }
    }
}
