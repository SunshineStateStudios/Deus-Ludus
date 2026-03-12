using UnityEngine;
using TMPro;

public class AbilityLovers : AbilityCard
{
    public override string name => "Lovers";
    public override string description => "You and the opponent draw a random ability card (will not work for player with one or less ability cards).";
    public override int triesDecayTime => 1;

    private int GetRandomAbilityCard(Player ply) {
        return Random.Range(0, ply.AbilityCards.Count);
    }

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        int abilityChosenOwner = GetRandomAbilityCard(owner);
        int abilityChosenOpponent = GetRandomAbilityCard(opponent);

        gm.DrawAbilityCard(owner, abilityChosenOwner);
        gm.DrawAbilityCard(opponent, abilityChosenOpponent);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {

    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}