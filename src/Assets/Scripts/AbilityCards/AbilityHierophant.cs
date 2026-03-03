using System.Collections;
using UnityEngine;

public class AbilityHierophant : AbilityCard
{
    public override string name => "Hierophant";
    public override string description => "Discard the number card you hold with the highest attack.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        if (owner.NumberCards.Count <= 1) return;
        int chosenCardIndex = 0;

        for (int i = 1; i < owner.NumberCards.Count; i++)
        {
            NumberCard card = owner.NumberCards[i];

            if (card.Damage > owner.NumberCards[chosenCardIndex].Damage)
            {
                chosenCardIndex = i;
            }
        }

        int chosenPly = 1;
        if (owner == gm.ply2) chosenPly = 2;

        gm.RemoveNumberCard(chosenPly, chosenCardIndex);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        
    }
}