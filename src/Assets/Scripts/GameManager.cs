using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player ply1;
    public Player ply2;
    public Deck deck;
    public GamePhase phase;

    void Start()
    {
        StartNewRound();
    }

    void StartNewRound()
    {
        ply1.NumberCards.Clear();
        ply2.NumberCards.Clear();
        phase = GamePhase.Draw;
    }

    void DrawCard(Player ply)
    {
        Card card = deck.Draw();

        if (card is NumberCard numberCard)
        {
            ply.NumberCards.Add(numberCard);
            SpawnGod(numberCard);
        } else if (card is AbilityCard abilityCard)
        {
            ply.AbilityCards.Add(abilityCard);
            SpawnAbilityBehindLastCard();
        }
    }

    void SpawnGod(NumberCard numberCard)
    {
        // TO BE WRITTEN LATER
    }

    void SpawnAbilityBehindLastCard()
    {
        // TO BE WRITTEN LATER
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

    void ResolveCombat(Player attacker, Player defender)
    {
        int attackPower = attacker.TotalDamage;
        int defencePower = defender.TotalHealth;

        if (attackPower > defencePower) defender.Life -= 1;
    }

    void CombatPhase(Player first, Player second)
    {
        ResolveCombat(first, second);
        ResolveCombat(second, first);
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
