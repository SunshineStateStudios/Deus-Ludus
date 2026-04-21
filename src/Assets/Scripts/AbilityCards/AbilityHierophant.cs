using System.Collections;
using UnityEngine;

public class AbilityHierophant : AbilityCard
{
    public override string name => "Hierophant";
    public override string description => "Discard the number card you hold with the <color=#ff8282>highest attack</color>.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
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
        if (owner == gm.ply1)
        {
            if (gm.ply1.NumberCards.Count >= 6)
            {
                canvasMngr.ShowDrawButton(false);
            } else
            {
                canvasMngr.ShowDrawButton(true);
            }
        }
        canvasMngr.CalculateText(gm.ply1, gm.ply2, true);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        if (owner.NumberCards.Count <= 1)
            return false;

        int blackjackTotal = owner.BlackjackTotal(gm.BlackjackThreshold, false);

        if (blackjackTotal > gm.BlackjackThreshold)
            return true;

        NumberCard highestCard = owner.NumberCards[0];
        foreach (NumberCard card in owner.NumberCards)
        {
            if (card.Damage > highestCard.Damage)
                highestCard = card;
        }

        if (highestCard.Value >= 6 && blackjackTotal >= gm.BlackjackThreshold - 2)
            return true;

        return false;
    }
}