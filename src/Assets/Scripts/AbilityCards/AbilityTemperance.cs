public class AbilityTemperance : AbilityCard
{
    public override string name => "Temperance";
    public override string description => "Reduces the threshold by 4 points.";
    public override int triesDecayTime => 1;
    
    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 4;
        if (gm.BlackjackThreshold < 9) gm.BlackjackThreshold = 9;
        gm.UpdateProgressText();
        gm.UpdateDrawNumberCardText();
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold += 4;
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}