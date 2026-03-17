using System.Collections;
using UnityEngine;

public class AbilityHermit : AbilityCard
{
    public override string name => "Hermit";
    public override string description => "Reset your hand back to 2 Number Cards";
    public override int triesDecayTime => 1;
    int chosenCardIndex = 0;
    int chosenPly = 1; //default player is you

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        if (owner.NumberCards.Count <= 0) return;
        if (owner == gm.ply2) chosenPly = 2; //player var for opp use

        for (int i = owner.NumberCards.Count-1; i >= 0; i--)
        {
            NumberCard card = owner.NumberCards[i];
            chosenCardIndex = i;
            gm.RemoveNumberCard(chosenPly, chosenCardIndex);
        }
        gm.UpdateAttackDefendText();
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}