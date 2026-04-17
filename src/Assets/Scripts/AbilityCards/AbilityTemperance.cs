using UnityEngine;

public class AbilityTemperance : AbilityCard
{
    public override string name => "Temperance";
    public override string description => "Reduces the threshold by 4 points.";
    public override int triesDecayTime => 1;
    
    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 4;
        if (gm.BlackjackThreshold < 9) gm.BlackjackThreshold = 9;

        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        canvasMngr.CalculateText(gm.ply1, gm.ply2, true, 0f);
        canvasMngr.playerTotalLabel.GetComponent<Animator>().Play("ThresholdChanged", 0, 0);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold += 4;
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
        } else if (difference == 4) {
            shouldDraw = Random.Range(1,6) == 1;
        } else if (difference < 0) {
            shouldDraw = true;
        }

        return shouldDraw;
    }
}