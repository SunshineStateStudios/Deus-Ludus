public class Test6 : AbilityCard
{
    public override string name => "Test6";
    public override string description => "this is test6";
    public override int triesDecayTime => 6;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }
}