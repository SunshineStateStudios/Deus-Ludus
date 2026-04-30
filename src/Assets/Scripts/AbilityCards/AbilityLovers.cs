using UnityEngine;
using TMPro;

public class AbilityLovers : AbilityCard
{
    public override string name => "Lovers";
    public override string description => "You and the opponent draw a random <color=#ff8282>ability</color> card (will not work for player with one or less ability cards).";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        if (owner.AbilityCards.Count > 0) gm.DrawAbilityCard(owner, Random.Range(0,owner.AbilityCards.Count-1));
        if (opponent.AbilityCards.Count > 0) gm.DrawAbilityCard(opponent, Random.Range(0,opponent.AbilityCards.Count-1));
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {

    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        Player opponent = gm.ply1;

        if (owner.AbilityCards.Count <= 1)
            return false;

        int myAbilities = owner.AbilityCards.Count;
        int oppAbilities = opponent.AbilityCards.Count;

        if (oppAbilities >= myAbilities)
            return false;

        if (myAbilities + 2 <= oppAbilities)
            return true;

        return Random.value < 0.25f;
    }
}