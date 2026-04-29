using UnityEngine;

public class AbilityDevil : AbilityCard
{
    public override string name => "The Devil";
    public override string description => "Increases the <color=#ff85f7>threshold</color> by <color=#ff8282>6</color> points.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold += 6;
        if (gm.BlackjackThreshold > 30) gm.BlackjackThreshold = 30;
        
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        canvasMngr.CalculateText(gm.ply1, gm.ply2, true, true, 0f);
        canvasMngr.playerTotalLabel.GetComponent<Animator>().Play("ThresholdChanged", 0, 0);

        if (owner != gm.ply1) return;

        if (gm.ply1.NumberCards.Count < 6) {
            if (gm.ply1.BlackjackTotal(gm.BlackjackThreshold, false) >= gm.BlackjackThreshold) {
                canvasMngr.ShowDrawButton(false);
            } else {
                canvasMngr.ShowDrawButton(true);
            }
        }
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 6;
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
        } else if (difference == 5) {
            shouldDraw = Random.Range(1,7) == 1;
        } else if (difference == 6) {
            shouldDraw = Random.Range(1,8) == 1;
        } else if (difference < 0) {
            shouldDraw = true;
        }

        return shouldDraw;
    }
}
