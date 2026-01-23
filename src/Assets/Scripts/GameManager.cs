using System.Collections;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public Player ply1;
    public Player ply2;
    public Deck deck;
    public GamePhase phase;

    public GameObject numberCard;
    public GameObject playerNumberCards;
    public GameObject enemyNumberCards;

    void Start()
    {
        ply1 = new Player();
        ply2 = new Player();
        deck = new Deck();

        StartCoroutine(StartNewRound());

        GameObject inventoryButton = GameObject.Find("Canvas/InventoryButton");
        GameObject stayButton = GameObject.Find("Canvas/StayButton");
        GameObject drawButton = GameObject.Find("Canvas/DrawNumberCardButton");

        RectTransform inventoryButtonRect = inventoryButton.GetComponent<RectTransform>();
        RectTransform stayButtonRect = stayButton.GetComponent<RectTransform>();
        RectTransform drawButtonRect = drawButton.GetComponent<RectTransform>();

        inventoryButtonRect.anchoredPosition = new Vector2(888f, 57f);
        stayButtonRect.anchoredPosition = new Vector2(-888f, 158f);
        drawButtonRect.anchoredPosition = new Vector2(-888f, 57f);

        StartCoroutine(ShowUIButtons());
    }

    IEnumerator ShowUIButtons()
    {
        yield return new WaitForSeconds(3.5f);

        GameObject inventoryButton = GameObject.Find("Canvas/InventoryButton");
        GameObject stayButton = GameObject.Find("Canvas/StayButton");
        GameObject drawButton = GameObject.Find("Canvas/DrawNumberCardButton");

        RectTransform inventoryButtonRect = inventoryButton.GetComponent<RectTransform>();
        RectTransform stayButtonRect = stayButton.GetComponent<RectTransform>();
        RectTransform drawButtonRect = drawButton.GetComponent<RectTransform>();

        inventoryButtonRect.DOAnchorPos(new Vector2(-60f, 57f), 0.75f).SetEase(Ease.OutSine);
        stayButtonRect.DOAnchorPos(new Vector2(52f, 158f), 0.75f).SetEase(Ease.OutSine);
        drawButtonRect.DOAnchorPos(new Vector2(52f, 57f), 0.75f).SetEase(Ease.OutSine);
    }

    IEnumerator StartNewRound()
    {
        ply1.NumberCards.Clear();
        ply2.NumberCards.Clear();

        for (int i = 0; i < 2; i++)
        {
            DrawNumberCard(ply1);
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < 2; i++)
        {
            DrawNumberCard(ply2);
            yield return new WaitForSeconds(0.5f);
        }

        phase = GamePhase.PlayerTurn;
    }

    void DrawNumberCard(Player ply)
    {
        NumberCard card = deck.Draw();
        ply.NumberCards.Add(card);

        GameObject parent = playerNumberCards;

        if (ply == ply2)
        {
            parent = enemyNumberCards;
        }

        GameObject cardRepresentation = Instantiate(numberCard, parent.transform, false);
        cardRepresentation.transform.localPosition = new Vector3(3f - (ply.NumberCards.Count - 1), 0f, 0f);
    }

    public void DrawNumberCard(int ply)
    {
        Player plyToUse = ply2;

        if (ply == 1)
        {
            plyToUse = ply1;
        }

        DrawNumberCard(plyToUse);
    }

    void DestroyAllSpawnedCardObjects()
    {
        // TO BE WRITTEN LATER
    }

    Player DetermineBlackjackWinner()
    {
        if (ply1.IsBust && ply2.IsBust)
        {
            if (ply1.BlackjackTotal == ply2.BlackjackTotal)
            {
                return null;
            } else if (ply1.BlackjackTotal < ply2.BlackjackTotal)
            {
                return ply1;
            } else
            {
                return ply2;
            }
        }

        if (ply1.IsBust) return ply2;
        if (ply2.IsBust) return ply1;

        int p1Diff = 21 - ply1.BlackjackTotal;
        int p2Diff = 21 - ply2.BlackjackTotal;

        if (p1Diff < p2Diff) return ply1;
        if (p2Diff < p1Diff) return ply2;

        return null;
    }

    void ResolveCombat(Player attacker, Player defender, float counterMultiplier = 1f)
    {
        int attackPower = attacker.TotalDamage;
        int defencePower = defender.TotalHealth;

        if (attackPower > defencePower) defender.Life -= 1;
    }

    void CombatPhase(Player first, Player second)
    {
        ResolveCombat(first, second);
        ResolveCombat(second, first, 0.5f);
    }

    void Cleanup()
    {
        ply1.NumberCards.Clear();
        ply1.AbilityCards.Clear();

        ply2.NumberCards.Clear();
        ply2.AbilityCards.Clear();

        DestroyAllSpawnedCardObjects();
    }
}
