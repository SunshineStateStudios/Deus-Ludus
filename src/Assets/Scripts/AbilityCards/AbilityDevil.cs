using UnityEngine;

public class AbilityDevil : AbilityCard
{
    public override string name => "The Devil";
    public override string description => "Increases the threshold by 6 points.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold += 6;
        if (gm.BlackjackThreshold > 30) gm.BlackjackThreshold = 30;
        gm.canvasObject.GetComponent<CanvasManager>().CalculateText(gm.ply1, gm.ply2, true, 0f);
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
