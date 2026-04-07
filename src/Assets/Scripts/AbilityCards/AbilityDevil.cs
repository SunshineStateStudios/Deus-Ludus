public class AbilityDevil : AbilityCard
{
    public override string name => "The Devil";
    public override string description => "Increases the threshold by 6 points.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold += 6;
        if (gm.BlackjackThreshold > 30) gm.BlackjackThreshold = 30;
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 6;
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}
