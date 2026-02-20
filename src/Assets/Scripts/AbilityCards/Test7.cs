public class Test7 : AbilityCard
{
    public override string name => "Test7";
    public override string description => "this is test7";
    public override int triesDecayTime => 7;
    
    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }
}