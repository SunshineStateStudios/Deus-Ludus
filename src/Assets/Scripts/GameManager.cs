using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player ply1;
    public Player ply2;
    public Deck deck;
    public GamePhase phase;

    void Start()
    {
        ply1 = new Player();
        ply2 = new Player();
        deck = new Deck();

        StartNewRound();
    }

    void StartNewRound()
    {
        ply1.NumberCards.Clear();
        ply2.NumberCards.Clear();

        for (int i = 0; i < 2; i++)
        {
            DrawNumberCard(ply1);
        }

        for (int i = 0; i < 2; i++)
        {
            DrawNumberCard(ply2);
        }

        phase = GamePhase.PlayerTurn;
    }

    void DrawNumberCard(Player ply)
    {
        NumberCard card = deck.Draw();
        ply.NumberCards.Add(card);
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
