using System.Collections;
using UnityEngine;

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
            Debug.Log("jz");
            yield return new WaitForSeconds(0.5f);
        }

        phase = GamePhase.PlayerTurn;
    }

    void DrawNumberCard(Player ply)
    {
        NumberCard card = deck.Draw();
        ply.NumberCards.Add(card);

        GameObject parent = playerNumberCards;
        Vector3 origin = new Vector3(-1.065f,0.084f,-8.168f);

        if (ply == ply2)
        {
            Debug.Log("adljfshf");
            parent = enemyNumberCards;
            origin = new Vector3(-1.065f,0.084f,-6.78f);
        }
        origin += new Vector3(3 * (ply.NumberCards.Count-1), 0f, 0f);

        GameObject cardRepresentation = Instantiate(numberCard, origin, Quaternion.identity, parent.transform);
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
