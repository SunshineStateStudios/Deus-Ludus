using UnityEngine;

public class AbilityDeath : AbilityCard
{
    public override string name => "Death";
    public override string description => "Increases the threshold by 3 points.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold += 3;
        if (gm.BlackjackThreshold > 30) gm.BlackjackThreshold = 30;
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 3;
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        int cardsTotal = owner.BlackjackTotal(gm.BlackjackThreshold, false);
        int difference = gm.BlackjackThreshold - cardsTotal;

        bool shouldDraw = false;

        if (difference == 1) {
            shouldDraw = Random.Range(1,3) == 1;
        } else if (difference == 2) {
            shouldDraw = Random.Range(1,4) == 1;
        } else if (difference == 3) {
            shouldDraw = Random.Range(1,5) == 1;
        } else if (difference < 0) {
            shouldDraw = true;
        }

        return shouldDraw;
    }
}