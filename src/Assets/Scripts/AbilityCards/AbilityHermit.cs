using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHermit : AbilityCard
{
    public override string name => "Hermit";
    public override string description => "<color=#ff8282>Reset</color> your hand back to 2 Number Cards";
    public override int triesDecayTime => 1;
    int chosenCardIndex = 0;
    int chosenPly = 1; //default player is you
    NumberCard card;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        if (owner.NumberCards.Count <= 0) return;
        if (owner == gm.ply2) chosenPly = 2; //player var for opp use

        for (int i = owner.NumberCards.Count-1; i >= 0; i--)
        {
            card = owner.NumberCards[i];
            chosenCardIndex = i;
            gm.RemoveNumberCard(chosenPly, chosenCardIndex);
        }

        gm.DrawTwice(chosenPly);

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
        return owner.BlackjackTotal(gm.BlackjackThreshold, false) > gm.BlackjackThreshold;
    }
}