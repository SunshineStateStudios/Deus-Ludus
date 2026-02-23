public class AbilityDeath : AbilityCard
{
    public override string name => "Death";
    public override string description => "Increases the threshold by 3 points.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold += 3;
        if (gm.BlackjackThreshold > 30) gm.BlackjackThreshold = 30;
        gm.UpdateProgressText();
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 3;
    }
}